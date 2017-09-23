using System;
using System.Drawing;

namespace RayTraceX
{
    public delegate void RenderUpdateDelegate(int progress, double duration, double ETA, int scanline);

    /// <summary>
    /// 反锯齿程度
    /// </summary>
    public enum AntiAliasing
    {
        None =      0,
        Quick =     1,
        Low =       4,
        Medium =    8,
        High =     16,
        VeryHigh = 32
    }

    public class RayTracer
    {
        public bool RenderDiffuse;
        public bool RenderHighlights;
        public bool RenderShadow;
        public bool RenderReflection;
        public bool RenderRefraction;
        public AntiAliasing AntiAliasing;

        public event RenderUpdateDelegate RenderUpdate;

        public RayTracer() : this(AntiAliasing.Medium, true, true, true, true, true)
        {
        }

        public RayTracer(AntiAliasing antialiasing, bool renderDiffuse, bool renderHighlights, bool renderShadow, bool renderReflection, bool renderRefraction)
        {
            RenderDiffuse = renderDiffuse;
            RenderHighlights = renderHighlights;
            RenderShadow = renderShadow;
            RenderReflection = renderReflection;
            RenderRefraction = renderRefraction;
            AntiAliasing = antialiasing;
        }

        /// <summary>
        /// 随机数...
        /// </summary>
        /// <param name="x">生成随机数的种子</param>
        /// <returns>[-1,1]</returns>
        private double IntNoise(int x)
        {
            x = (x << 13) ^ x;
            return (1.0 - ((x * (x * x * 15731 + 789221) + 1376312589) & 0x7fffffff) / (int.MaxValue / 2.0));
        }

        /// <summary>
        /// 用于载入渲染数据
        /// </summary>
        /// <param name="g">画图用的画笔</param>
        /// <param name="viewport">图片的尺寸</param>
        /// <param name="scene">场景</param>
        public void RayTraceScene(Graphics g, Rectangle viewport, Scene scene)
        {
            int maxsamples = (int)AntiAliasing;
            DateTime timestart = DateTime.Now;

            g.FillRectangle(Brushes.Black, viewport);

            Color[] scanline1;
            Color[] scanline2 = null;
            Color[] scanline3 = null;

            Color[,] buffer = new Color[viewport.Width + 2, viewport.Height + 2];

            for (int y = 0; y < viewport.Height + 2; y++)
            {
                //使用反锯齿
                scanline1 = scanline2;
                scanline2 = scanline3;
                scanline3 = new Color[viewport.Width + 2];

                for (int x = 0; x < viewport.Width + 2; x++)
                {
                    double yp = y * 1.0f / viewport.Height * 2 - 1;
                    double xp = x * 1.0f / viewport.Width * 2 - 1;

                    Ray ray = scene.Camera.GetRay(xp, yp);
                
                    // 进行光线跟踪
                    buffer[x, y] = CalculateColor(ray, scene);

                    if ((x > 1) && (y > 1))
                    {
                        if (AntiAliasing != AntiAliasing.None)
                        {
                            //平均采样
                            Color avg = (buffer[x - 2, y - 2] + buffer[x - 1, y - 2] + buffer[x, y - 2] +
                                         buffer[x - 2, y - 1] + buffer[x - 1, y - 1] + buffer[x, y - 1] +
                                         buffer[x - 2, y] + buffer[x - 1, y] + buffer[x, y]) / 9;

                            if (AntiAliasing == AntiAliasing.Quick)
                            {
                                buffer[x - 1, y - 1] = avg;
                            }
                            else
                            {   
                                if (avg.Distance(buffer[x - 1, y - 1]) > 0.18) 
                                {
                                    for (int i = 0; i < maxsamples; i++)
                                    {
                                        // 相关范围内的 随机采样
                                        double rx = Math.Sign(i % 4 - 1.5) * (IntNoise(x + y * viewport.Width * maxsamples * 2 + i) + 1) / 4; 
                                        double ry = Math.Sign(i % 2 - 0.5) * (IntNoise(x + y * viewport.Width * maxsamples * 2 + 1 + i) + 1) / 4; 

                                        xp = (x - 1 + rx) * 1.0f / viewport.Width * 2 - 1;
                                        yp = (y - 1 + ry) * 1.0f / viewport.Height * 2 - 1;

                                        ray = scene.Camera.GetRay(xp, yp);
     
                                        buffer[x - 1, y - 1] += CalculateColor(ray, scene);
                                    }
                                    buffer[x - 1, y - 1] /= (maxsamples + 1);
                                }
                            }
                        }

                        Brush br = new SolidBrush(buffer[x - 1, y - 1].ToArgb());
                        g.FillRectangle(br, viewport.Left + x - 2, viewport.Top + y - 2, 1, 1);
                        br.Dispose();
                    }
                }

                //更新显示
                if (RenderUpdate != null)
                {
                    double progress = (y) / (double)(viewport.Height);
                    double duration = DateTime.Now.Subtract(timestart).TotalMilliseconds;

                    double ETA = duration / progress - duration;
                    RenderUpdate.Invoke((int)progress * 100, duration, ETA, y - 1);
                }
            }
        }

        /// <summary>
        /// 返回光线跟踪后的颜色
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="scene">场景</param>
        /// <returns></returns>
        public Color CalculateColor(Ray ray, Scene scene)
        {
            IntersectInfo info = TestIntersection(ray, scene, null);
            if (info.IsHit)
            {
                // 进行光线跟踪
                Color c = RayTrace(info, ray, scene, 0);
                return c;
            }

            return scene.Background.Color;

        }

        /// <summary>
        /// 进行真正的光线跟踪
        /// </summary>
        /// <param name="info">击中信息</param>
        /// <param name="ray">射线</param>
        /// <param name="scene">场景</param>
        /// <param name="depth">递归深度</param>
        /// <returns></returns>
        private Color RayTrace(IntersectInfo info, Ray ray, Scene scene, int depth)
        {
            // 计算环境光
            Color color = info.Color * scene.Background.Ambience;
            double shininess = Math.Pow(10, info.Element.Material.Gloss + 1);

            foreach (Light light in scene.Lights)
            {

                // 计算漫射光
                Vector v = (light.Position - info.Position).Normalize();

                if (RenderDiffuse)
                {
                    double L = v.Dot(info.Normal);
                    if (L > 0.0f)
                        color += info.Color * light.Color * L;
                }


                //最大深度
                if (depth < 10)
                {

                    // 计算反射
                    if (RenderReflection && info.Element.Material.Reflection > 0)
                    {
                        Ray reflectionray = GetReflectionRay(info.Position, info.Normal, ray.Direction);
                        IntersectInfo refl = TestIntersection(reflectionray, scene, info.Element);
                        if (refl.IsHit && refl.Distance > 0)
                        {
                            // 递归计算反射 ----要改成迭代
                            refl.Color = RayTrace(refl, reflectionray, scene, depth + 1);
                        }
                        else //MISS 返回背景色
                            refl.Color = scene.Background.Color;
                        color = color.Blend(refl.Color, info.Element.Material.Reflection);
                    }

                    //计算折射
                    if (RenderRefraction && info.Element.Material.Transparency > 0)
                    {
                        Ray refractionray = GetRefractionRay(info.Position, info.Normal, ray.Direction, info.Element.Material.Refraction);
                        IntersectInfo refr = info.Element.Intersect(refractionray);
                        if (refr.IsHit)
                        {
                            refractionray = GetRefractionRay(refr.Position, refr.Normal, refractionray.Direction, refr.Element.Material.Refraction);
                            refr = TestIntersection(refractionray, scene, info.Element);
                            if (refr.IsHit && refr.Distance > 0)
                            {
                                // 递归计算折射 ----要改成迭代
                                refr.Color = RayTrace(refr, refractionray, scene, depth + 1);
                            }
                            else
                                refr.Color = scene.Background.Color;
                        }
                        else
                            refr.Color = scene.Background.Color;
                        color = color.Blend(refr.Color, info.Element.Material.Transparency);
                    }
                }


                IntersectInfo shadow = new IntersectInfo();
                if (RenderShadow)
                {
                    // 计算阴影
                    Ray shadowray = new Ray(info.Position, v);

                    // 该点的直射光线是否被遮挡
                    shadow = TestIntersection(shadowray, scene, info.Element);
                    if (shadow.IsHit && shadow.Element != info.Element)
                    {
                        color *= 0.5 + 0.5 * Math.Pow(shadow.Element.Material.Transparency, 0.5); // Math.Pow(.5, shadow.HitCount);
                    }
                }

                if (RenderHighlights && !shadow.IsHit && info.Element.Material.Gloss > 0)
                {
                    // Phong 模型计算
                    Vector Lv = (info.Element.Position - light.Position).Normalize();
                    Vector E = (scene.Camera.Position - info.Element.Position).Normalize();
                    Vector H = (E - Lv).Normalize();

                    double Glossweight = 0.0;
                    Glossweight = Math.Pow(Math.Max(info.Normal.Dot(H), 0), shininess);
                    color += light.Color * (Glossweight);
                }
            }

            //整合光照
            color.Limit();
            return color;
        }


        private IntersectInfo TestIntersection(Ray ray, Scene scene, IShape exclude)
        {
            int hitcount = 0;
            IntersectInfo best = new IntersectInfo();
            best.Distance = double.MaxValue;

            foreach (IShape elt in scene.Shapes)
            {
                if (elt == exclude)
                    continue;

                IntersectInfo info = elt.Intersect(ray);
                if (info.IsHit && info.Distance < best.Distance && info.Distance >= 0)
                {
                    best = info;
                    hitcount++;
                }
            }
            best.HitCount = hitcount;
            return best;
        }

        /// <summary>
        /// 得到反射后的射线
        /// </summary>
        /// <param name="P"></param>
        /// <param name="N"></param>
        /// <param name="V"></param>
        /// <returns></returns>
        private Ray GetReflectionRay(Vector P, Vector N, Vector V)
        {
            double c1 = -N.Dot(V);
            Vector Rl = V + (N * 2 * c1);
            return new Ray(P, Rl);
        }

        /// <summary>
        /// 计算折射射线
        /// </summary>
        /// <param name="P"></param>
        /// <param name="N"></param>
        /// <param name="V"></param>
        /// <param name="refraction"></param>
        /// <returns></returns>
        private Ray GetRefractionRay(Vector P, Vector N, Vector V, double refraction)
        {
  
            double c1 = N.Dot(V);
            double c2 = 1 - refraction * refraction * (1 - c1 * c1);
            if (c2 < 0)
                c2 = Math.Sqrt(c2);

            Vector T = (N * (refraction * c1 - c2) - V * refraction) * -1;
            T.Normalize();
            return new Ray(P, T);
        }
    }
}

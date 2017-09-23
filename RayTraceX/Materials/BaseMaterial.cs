using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{
    public interface IMaterial
    {
        /// <summary>
        /// 指定成员的亮度
        /// 数值在[1,5]
        /// </summary>
        double Gloss { get; set; }

        /// <summary>
        /// 指定成员的透明度
        /// 数值在 0(不透明) 到 1(全透明) 之间
        /// </summary>
        double Transparency { get; set; }

        /// <summary>
        /// 指定成员的反射率
        /// 数值在 0(不反射) 到 1(全反色“镜子”) 之间
        /// </summary>
        double Reflection { get; set; }


        /// <summary>
        /// 指定成员的折射率
        /// </summary>
        double Refraction { get; set; }

        /// <summary>
        /// 用来确定该成员是否有贴图
        /// 可以计算 u,v ，来使用GetColor 
        /// 来确定某个位置的映射的贴图颜色
        /// </summary>
        bool HasTexture { get; }

        /// <summary>
        /// 返回成员某位置映射的贴图颜色
        /// 通过 u,v来获取对应的贴图颜色
        /// </summary>
        /// <param name="u">贴图的 u 坐标</param>
        /// <param name="v">贴图的 v 坐标</param>
        /// <returns></returns>
        Color GetColor(double u, double v);
    }
    public abstract class BaseMaterial : IMaterial
    {
        private double gloss;
        private double transparency;
        private double reflection;
        private double refraction;

        public double Reflection
        {
            get { return reflection; }
            set { reflection = value; }
        }

        public double Refraction
        {
            get { return refraction; }
            set { refraction = value; }
        }

        public double Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }

        public double Gloss
        {
            get { return gloss; }
            set { gloss = value; }
        }

        public abstract bool HasTexture { get; }
        public abstract Color GetColor(double u, double v);

        public BaseMaterial()
        {
            gloss = 2; // 
            transparency = 0;   // 默认不透明
            reflection = 0;     // 默认不反射
            refraction = 0.50;  // 默认折射率
        }

        /// <summary>
        /// 映射 t 在 [0,1]范围内
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected double WrapUp(double t)
        {
            t = t % 2.0;
            if (t < -1) t = t + 2.0;
            if (t >= 1) t -= 2.0;
            return t;
        }
    }
}

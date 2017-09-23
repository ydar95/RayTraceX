using System;
using System.Drawing;

namespace RayTraceX
{
    public class Texture
    {
        public int Width;
        public int Height;
        public Color[,] ColorMap;

        public Texture(Color[,] colormap)
        {
            Width = colormap.GetLength(0);
            Height = colormap.GetLength(1);
            ColorMap = colormap;
        }

        /// <summary>
        /// 将Bitmap 转换为 Texture
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        static public Texture FromBitmap(Bitmap bm)
        {

            Color[,] colormap = new Color[bm.Width, bm.Height];
            Texture texture = new Texture(colormap);

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    System.Drawing.Color pix = bm.GetPixel(x,y); // GetPixel is a relative slow function
                    Color c = new Color(pix.R / 255.0, pix.G / 255.0, pix.B / 255.0);
                    colormap[x, y] = c;
                }
            }
            return texture;
        }

        /// <summary>
        /// 载入图片
        /// </summary>
        /// <param name="filename"></param>
        static public Texture FromFile(string filename)
        {
            Image image = Image.FromFile(filename);
            Bitmap bm = new Bitmap(image);
            return FromBitmap(bm);
        }
    }

    public class TextureMaterial : BaseMaterial
    {
        public Texture Texture;
        public double Density;
        public double u_offset;
        public double v_offset;

        public TextureMaterial(Texture texture, double reflection, double transparency, double gloss, double density, double u_offset = 0, double v_offset = 0)
        {
            this.Reflection = reflection;
            this.Transparency = transparency;
            this.Gloss = gloss;
            this.Density = density;
            this.v_offset = v_offset;
            this.u_offset = u_offset;
            Texture = texture;
        }

        public override bool HasTexture
        {
            get { return true; }
        }

        /// <summary>
        /// 返回贴图坐标 
        /// </summary>
        /// <param name="u">[0,1]</param>
        /// <param name="v">[0,1]</param>
        /// <returns></returns>
        public override Color GetColor(double u, double v)
        {
            double nu1 = (u+u_offset)%1.0 * Texture.Width ;
            double nv1 = (v+v_offset)%1.0 * Texture.Height ;
            return Texture.ColorMap[(int)nu1, (int)nv1];
        }
    }
}

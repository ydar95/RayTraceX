using System;
using Drawing = System.Drawing;
namespace RayTraceX
{
    /// <summary>
	/// 颜色 class.
	/// </summary>
    public class Color
    {
        public double Red;
        public double Green;
        public double Blue;

        public Color()
        {
            // 初始化为黑色
            Red = 0;
            Green = 0;
            Blue = 0;
        }

        public Color(double r, double g, double b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        public Color(Color col)
        {
            // copy over members
            Red = col.Red;
            Green = col.Green;
            Blue = col.Blue;
        }


        /// 重载操作符

        static public Color operator +(Color c1, Color c2)
        {
            Color result = new Color();
            result.Red = c1.Red + c2.Red;
            result.Green = c1.Green + c2.Green;
            result.Blue = c1.Blue + c2.Blue;

            return result;
        }

        static public Color operator -(Color c1, Color c2)
        {
            Color result = new Color();

            result.Red = c1.Red - c2.Red;
            result.Green = c1.Green - c2.Green;
            result.Blue = c1.Blue - c2.Blue;

            return result;
        }

        static public Color operator *(Color c1, Color c2)
        {
            Color result = new Color();

            result.Red = c1.Red * c2.Red;
            result.Green = c1.Green * c2.Green;
            result.Blue = c1.Blue * c2.Blue;

            return result;
        }

        static public Color operator *(Color col, double f)
        {
            Color result = new Color();

            result.Red = col.Red * f;
            result.Green = col.Green * f;
            result.Blue = col.Blue * f;

            return result;
        }

        static public Color operator /(Color col, double f)
        {
            Color result = new Color();

            result.Red = col.Red / f;
            result.Green = col.Green / f;
            result.Blue = col.Blue / f;

            return result;
        }

        /// R G B 限制在 [0,1] ，大于 1 强制变成 1
        public void Limit()
        {
            Red = (Red > 0.0) ? ((Red > 1.0) ? 1.0f : Red) : 0.0f;
            Green = (Green > 0.0) ? ((Green > 1.0) ? 1.0f : Green) : 0.0f;
            Blue = (Blue > 0.0) ? ((Blue > 1.0) ? 1.0f : Blue) : 0.0f;
        }

        /// 变成黑色
        public void ToBlack()
        {
            Red = 0;
            Green = 0;
            Blue = 0;
        }

        /// <summary>
        /// 计算颜色之间的绝对值距离
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public double Distance(Color color)
        {
            double dist = Math.Abs(Red - color.Red) + Math.Abs(Green - color.Green) + Math.Abs(Blue - color.Blue);
            return dist;
        }
        /// <summary>
        /// 混合两种颜色
        /// </summary>
        /// <param name="other">另一个用来混合的颜色</param>
        /// <param name="weight">混颜色的权重</param>
        /// <returns></returns>
        public Color Blend(Color other, double weight)
        {
            Color result = new Color(this);
            result = this * (1 - weight) + other * weight;
            return result;
        }

        public Drawing.Color ToArgb()
        {
            return Drawing.Color.FromArgb((int)(Red * 255), (int)(Green * 255), (int)(Blue * 255));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{
    //光源集合
    public class Lights : List<Light>
    {
    }

    /// <summary>
    /// 光源属性
    /// </summary>
    public class Light
    {
        public Vector Position;
        public Color Color;
        public double strength;//光强

        public Light(Vector pos, Color color)
        {
            Position = pos;
            Color = color;

            strength = 10;
        }

        public double Strength(double distance)
        {
            if (distance >= strength) return 0;

            return Math.Pow((strength - distance) / strength, .2);
        }
        public override string ToString()
        {
            return string.Format("Light ({0},{1},{2})", Position.x, Position.y, Position.z);
        }
    }
}

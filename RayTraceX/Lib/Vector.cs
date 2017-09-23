using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{
    /// <summary>
    /// 三维向量
    /// </summary>
    public class Vector
    {
        public double x;
        public double y;
        public double z;

        static public Vector Null;
        static public Vector Infinate;

        static Vector()
        {
            Null = new Vector(0, 0, 0);
            Infinate = new Vector(double.MaxValue, double.MaxValue, double.MaxValue);
        }

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(Vector v) : this(v.x, v.y, v.z)
        {
        }

        public Vector Normalize()
        {
            double t = (double)this.Magnitude();
            return new Vector(x / t, y / t, z / t);
        }

        static public Vector operator +(Vector v, Vector w)
        {
            return new Vector(w.x + v.x, w.y + v.y, w.z + v.z);
        }

        static public Vector operator -(Vector v, Vector w)
        {
            return new Vector(v.x - w.x, v.y - w.y, v.z - w.z);
        }

        static public Vector operator *(Vector v, Vector w)
        {
            return new Vector(v.x * w.x, v.y * w.y, v.z * w.z);
        }

        static public Vector operator *(Vector v, double f)
        {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }

        static public Vector operator /(Vector v, double f)
        {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }

        public double Dot(Vector w)
        {
            return this.x * w.x + this.y * w.y + this.z * w.z;
        }

        public Vector Cross(Vector w)
        {
            return new Vector(-this.z * w.y + this.y * w.z,
                               this.z * w.x - this.x * w.z,
                              -this.y * w.x + this.x * w.y);
        }

        /// <summary>
        /// 模
        /// </summary>
        /// <returns></returns>
        public double Magnitude()
        {
            return Math.Sqrt((double)((x * x) + (y * y) + (z * z)));
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", this.x, this.y, this.z);
        }
    }
}

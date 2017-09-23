using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{

    public class SphereShape : BaseShape
    {
        public double R;
        public SphereShape(Vector pos, double r, IMaterial material)
        {
            R = r;
            Position = pos;
            Material = material;
        }

        #region IShape Members


        public override IntersectInfo Intersect(Ray ray)
        {
            IntersectInfo info = new IntersectInfo();
            info.Element = this;

            Vector dst = ray.Position - this.Position;
            double B = dst.Dot(ray.Direction);
            double C = dst.Dot(dst) - (R * R);
            double D = B * B - C;

            if (D > 0) 
            {
                info.IsHit = true;
                info.Distance = -B - (double)Math.Sqrt(D);
                info.Position = ray.Position + ray.Direction * info.Distance;
                info.Normal = (info.Position - Position).Normalize();

                if (Material.HasTexture)
                {
                    Vector vn = new Vector(0, 1, 0).Normalize(); 
                    Vector ve = new Vector(0, 0, 1).Normalize(); 
                    Vector vp = (info.Position - Position).Normalize(); 

                    double phi = Math.Acos(-vp.Dot(vn));
                    double v = (phi / Math.PI);

                    double sinphi = ve.Dot(vp) / Math.Sin(phi);
                    double theta = Math.Acos(sinphi)  /(Math.PI*2);
              
                    double u;
                    if (vn.Cross(ve).Dot(vp) > 0)
                        u = theta;
                    else
                        u = 1 - theta;

                    info.Color = this.Material.GetColor(1-u,1-v);
                }
                else
                {
                    info.Color = this.Material.GetColor(0, 0);
                }
            }
            else
            {
                info.IsHit = false;
            }
            return info;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("Sphere ({0},{1},{2}) Radius: {3}", Position.x, Position.y, Position.z, R);
        }

    }
}

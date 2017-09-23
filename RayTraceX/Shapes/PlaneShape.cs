
namespace RayTraceX
{
    public class PlaneShape : BaseShape
    {
        public double D;
        public Color OddColor;

        public PlaneShape(Vector pos, double d, IMaterial material)
        {
            Position = pos;
            D = d;
            Material = material;

        }
        public PlaneShape(Vector pos, double d, Color color, Color oddcolor, double reflection, double transparency)
        {
            Position = pos;
            D = d;
            //Color = color;
            OddColor = oddcolor;
            //Transparency = transparency;
            //Reflection = reflection;
        }

        public override IntersectInfo Intersect(Ray ray)
        {

            IntersectInfo info = new IntersectInfo();
            double Vd = Position.Dot(ray.Direction);
            if (Vd == 0) return info; //Miss

            double t = -(Position.Dot(ray.Position) + D) / Vd;

            if (t <= 0) return info;

            info.Element = this;
            info.IsHit = true;
            info.Position = ray.Position + ray.Direction * t;
            info.Normal = Position;// *-1;
            info.Distance = t;

            if (Material.HasTexture)
            {
                Vector vecU = new Vector(Position.y, Position.z, -Position.x);
                Vector vecV = vecU.Cross(Position);

                double u = info.Position.Dot(vecU);
                double v = info.Position.Dot(vecV);
                info.Color = Material.GetColor(u, v);
            }
            else
                info.Color = Material.GetColor(0, 0);

            return info;
        }

        public override string ToString()
        {
            return string.Format("Sphere {0}x+{1}y+{2}z+{3}=0)", Position.x, Position.y, Position.z, D);
        }
    }
}

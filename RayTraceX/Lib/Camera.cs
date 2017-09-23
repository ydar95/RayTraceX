using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{
    public class Camera
    {
        public Vector Position;
        public Vector LookAt;
        public Vector Equator;
        public Vector Up; 
        public Vector Screen; 

        public Camera(Vector position, Vector lookat) : this(position, lookat, new Vector(0, 1, 0))
        {
        }

        public Camera(Vector position, Vector lookat, Vector up)
        {
            Up = up.Normalize();
            Position = position;
            LookAt = lookat;
            Equator = LookAt.Normalize().Cross(Up);
            Screen = Position + LookAt;
        }


        public Ray GetRay(double vx, double vy)
        {
            Vector pos = Screen - Up * vy - Equator * vx;
            Vector dir = pos - Position;

            Ray ray = new Ray(pos, dir.Normalize());
            return ray;
        }

    }
}

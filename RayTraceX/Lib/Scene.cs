using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{

    public class Scene
    {
        public Background Background;
        public Camera Camera;
        public Shapes Shapes;
        public Lights Lights;

        public Scene()
        {
            Camera = new Camera(new Vector(0, 0, -5), new Vector(0, 0, 1));
            Shapes = new Shapes();
            Lights = new Lights();
            Background = new Background(new Color(0.2, 0.2, 0.2), 0.2);
        }

    }
}

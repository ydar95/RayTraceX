using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Drawing = System.Drawing;
namespace RayTraceX
{
    public partial class Form1 : Form
    {
        private Scene scene;
        Texture marbleTexture;
        Texture woodTexture;
        Texture wallTexture;
        Bitmap bitmap;

        public Form1()
        {
            InitializeComponent();
        }
        private void SetupScene0()
        {

            scene = new Scene();
            scene.Background = new Background(new Color(1, 1, 1), 1);
            scene.Camera = new Camera(new Vector(0, 0, -10), new Vector(-.2, 0, 5), new Vector(0, 1, 0));

            scene.Shapes.Add(new SphereShape(new Vector(-1.5, 0.5, 0), 0.6,
                               new SolidMaterial(new Color(.1, .1, .1), 0.9, 0.0, 2.0)));


            scene.Shapes.Add(new SphereShape(new Vector(0, 0, 0), 1,
                                new TextureMaterial(marbleTexture, 0.0, 0.0, 2, 2, .25, 0.85)));

            scene.Shapes.Add(new PlaneShape(new Vector(0.1, 0.9, -0.5).Normalize(), 1.2,
                               new ChessboardMaterial(new Color(1, 1, 1), new Color(0, 0, 0), 0.2, 0, 1, 0.7)));

            scene.Lights.Add(new Light(new Vector(5, 10, -1), new Color(0.8, 0.8, 0.8)));
            scene.Lights.Add(new Light(new Vector(-3, 5, -15), new Color(0.8, 0.8, 0.8)));

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Application.StartupPath;
            woodTexture = Texture.FromFile(path + @"\wood2.png");
            marbleTexture = Texture.FromFile(path + @"\earth.jpg");
            wallTexture = Texture.FromFile(path + @"\wall1.png");
            SetupScene0();
        }

        void tracer_RenderUpdate(int progress, double duration, double ETA, int scanline)
        {

            pbScene.Invalidate(new Drawing.Rectangle(0, scanline - 1, pbScene.Image.Width, 2));
            Application.DoEvents();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RayTracer raytracer = new RayTracer(AntiAliasing.VeryHigh, true,true,true,true,true);
 
            raytracer.RenderUpdate += new RenderUpdateDelegate(tracer_RenderUpdate);
            Drawing.Rectangle rect = new Drawing.Rectangle(0, 0, 512, 512);
            bitmap = new Drawing.Bitmap(rect.Width, rect.Height);
            Drawing.Graphics g = Drawing.Graphics.FromImage(bitmap);
            pbScene.Image = bitmap;
            raytracer.RayTraceScene(g, rect, scene);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bitmap.Save(@".\b.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}


namespace RayTraceX
{
    public class SolidMaterial : BaseMaterial
    {
        private Color color;
        public SolidMaterial(Color color, double reflection, double transparency, double gloss)
        {
            this.color = color;
            this.Reflection = reflection;
            this.Transparency = transparency;
            this.Gloss = gloss;

        }

        public override bool HasTexture
        {
            get { return false; }
        }

        public override Color GetColor(double u, double v)
        {
            return color;
        }
    }
}

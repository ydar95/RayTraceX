

namespace RayTraceX
{
    /// <summary>
    /// 国际象棋棋盘材质设定
    /// </summary>
    public class ChessboardMaterial : BaseMaterial
    {
        /// <summary>
        /// 编号为双数的块的颜色（黑）
        /// </summary>
        public Color ColorEven;

        /// <summary>
        /// 编号为单数的块的颜色（白）
        /// </summary>
        public Color ColorOdd;

        /// <summary>
        /// 黑白相替浓密度
        /// 值必须大于0
        /// </summary>
        public double Density;

        public ChessboardMaterial(Color coloreven, Color colorodd, double reflection, double transparency, double gloss, double density)
        {
            this.ColorEven = coloreven;
            this.ColorOdd = colorodd;
            this.Reflection = reflection;
            this.Transparency = transparency;
            this.Gloss = gloss;
            this.Density = density;
        }

        public override bool HasTexture
        {
            get { return true; }
        }

        public override Color GetColor(double u, double v)
        {
            double t = WrapUp(u) * WrapUp(v);

            if (t < 0.0)
                return ColorEven;
            else
                return ColorOdd;
        }
    }
}

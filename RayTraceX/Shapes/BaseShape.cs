using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{
    /// <summary>
    /// 面的属性
    /// </summary>
    public interface IShape
    {
        /// <summary>
        /// 位置
        /// </summary>
        Vector Position { get; set; }

        /// <summary>
        /// 材质
        /// </summary>
        IMaterial Material { get; set; }

        /// <summary>
        /// 计算面与射线相交
        /// </summary>
        /// <param name="ray">射线</param>
        /// <returns></returns>
        IntersectInfo Intersect(Ray ray);
    }

    public class Shapes : List<IShape>
    {
    }


    public abstract class BaseShape : IShape
    {
        #region IShape Members
        private Vector position;
        private IMaterial material;

        public IMaterial Material
        {
            get { return material; }
            set { material = value; }
        }

        public Vector Position
        {
            get { return position; }
            set { position = value; }
        }

        public BaseShape()
        {
            position = new Vector(0, 0, 0);
            Material = new SolidMaterial(new Color(1, 0, 1), 0, 0, 0);
        }

        public abstract IntersectInfo Intersect(Ray ray);
        #endregion
    }
}

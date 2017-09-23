using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{
    /// <summary>
    /// 保存射线击中后的信息
    /// </summary>
    public class IntersectInfo
    {
        public bool IsHit;      // 是否被击中
        public int HitCount;    // 被击中的面的数量
        public IShape Element;  // 最近被击中的面
        public Vector Position; // 击中的坐标
        public Vector Normal;   // 被击中点的法向量
        public Color Color;     // 击中点的颜色
        public double Distance; // 点到屏幕的距离


        public IntersectInfo()
        {
        }
    }

    /// <summary>
    /// 射线，R=P+Dt
    /// </summary>
    public class Ray
    {
        public Vector Position;
        public Vector Direction;

        public Ray(Vector position, Vector direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}

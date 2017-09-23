using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceX
{

    public class Background
    {

        public Color Color;
        public double Ambience;

        /// <summary>
        /// 场景的背景属性
        /// </summary>
        /// <param name="color">环境颜色</param>
        /// <param name="ambience">环境颜色亮度</param>
        public Background(Color color, double ambience)
        {
            Color = color;
            Ambience = ambience;
        }

    }
}

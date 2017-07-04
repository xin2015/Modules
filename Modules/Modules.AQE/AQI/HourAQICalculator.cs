using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE.AQI
{
    /// <summary>
    /// 小时空气质量指数计算器
    /// </summary>
    public class HourAQICalculator : AQICalculator
    {
        /// <summary>
        /// 小时空气质量指数计算器构造函数
        /// </summary>
        public HourAQICalculator() : base(new double[] { 0, 150, 500, 650, 800 }, new double[] { 0, 100, 200, 700, 1200, 2340, 3090, 3840 }, new double[] { 0, 5, 10, 35, 60, 90, 120, 150 }, new double[] { 0, 160, 200, 300, 400, 800, 1000, 1200 })
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE.AQI
{
    /// <summary>
    /// 日均空气质量指数计算器
    /// </summary>
    public class DayAQICalculator : AQICalculator
    {
        /// <summary>
        /// 日均空气质量指数计算器构造函数
        /// </summary>
        public DayAQICalculator() : base(new double[] { 0, 50, 150, 475, 800, 1600, 2100, 2620 }, new double[] { 0, 40, 80, 180, 280, 565, 750, 940 }, new double[] { 0, 2, 4, 14, 24, 36, 48, 60 }, new double[] { 0, 100, 160, 215, 265, 800 })
        {

        }
    }
}

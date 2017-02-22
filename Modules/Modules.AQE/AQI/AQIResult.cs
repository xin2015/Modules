using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数结果
    /// </summary>
    public class AQIResult : IAQIResult
    {
        /// <summary>
        /// 空气质量指数
        /// </summary>
        public int? AQI { get; set; }
        /// <summary>
        /// 首要污染物
        /// </summary>
        public string PrimaryPollutant { get; set; }
        /// <summary>
        /// 空气质量指数类别
        /// </summary>
        public string Type { get; set; }
    }
}

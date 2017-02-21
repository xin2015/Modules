using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE
{
    /// <summary>
    /// 空气质量指数详细结果
    /// </summary>
    public class AQIResultDetail : AQIResult, IAQIResultDetail
    {
        /// <summary>
        /// 超标污染物
        /// </summary>
        public string NonAttainmentPollutant { get; set; }
    }
}

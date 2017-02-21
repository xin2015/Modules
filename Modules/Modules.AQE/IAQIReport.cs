using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE
{
    /// <summary>
    /// 空气质量指数报表接口
    /// </summary>
    public interface IAQIReport : IAQICalculate, IIAQIData
    {
        /// <summary>
        /// 空气质量指数级别
        /// </summary>
        string Level { get; set; }
        /// <summary>
        /// 空气质量指数表示颜色
        /// </summary>
        string Color { get; set; }
    }
}

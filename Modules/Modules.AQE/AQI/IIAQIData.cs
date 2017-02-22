using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量分指数数据接口
    /// </summary>
    public interface IIAQIData
    {
        /// <summary>
        /// 二氧化硫（SO2）分指数
        /// </summary>
        int? ISO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）分指数
        /// </summary>
        int? INO2 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）分指数
        /// </summary>
        int? IPM10 { get; set; }
        /// <summary>
        /// 一氧化碳（CO）分指数
        /// </summary>
        int? ICO { get; set; }
        /// <summary>
        /// 臭氧（O3）分指数
        /// </summary>
        int? IO3 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）分指数
        /// </summary>
        int? IPM25 { get; set; }
    }
}

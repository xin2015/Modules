﻿namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数结果接口
    /// </summary>
    public interface IAQIResult
    {
        /// <summary>
        /// 空气质量指数
        /// </summary>
        int? AQI { get; set; }
        /// <summary>
        /// 首要污染物
        /// </summary>
        string PrimaryPollutant { get; set; }
        /// <summary>
        /// 空气质量指数类别
        /// </summary>
        string Type { get; set; }
    }
}

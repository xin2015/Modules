﻿namespace Modules.AQE.AQCI
{
    /// <summary>
    /// 空气质量综合指数结果
    /// </summary>
    public class AQCIResult : IAQCIResult
    {
        /// <summary>
        /// 空气质量综合指数
        /// </summary>
        public decimal? AQCI { get; set; }
        /// <summary>
        /// 首要污染物
        /// </summary>
        public string PrimaryPollutant { get; set; }
    }
}

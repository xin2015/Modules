﻿namespace Modules.AQE.AQCI
{
    /// <summary>
    /// 空气质量综合指数详细结果
    /// </summary>
    public class AQCIResultDetail : AQCIResult, IAQCIResultDetail
    {
        /// <summary>
        /// 空气质量最大指数
        /// </summary>
        public decimal? AQMI { get; set; }
    }
}

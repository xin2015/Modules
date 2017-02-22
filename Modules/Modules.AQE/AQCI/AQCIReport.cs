namespace Modules.AQE.AQCI
{
    /// <summary>
    /// 空气质量综合指数报表
    /// </summary>
    public class AQCIReport : AQCICalculate, IAQCIReport
    {
        /// <summary>
        /// 二氧化硫（SO2）单项指数
        /// </summary>
        public decimal? ISO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）单项指数
        /// </summary>
        public decimal? INO2 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）单项指数
        /// </summary>
        public decimal? IPM10 { get; set; }
        /// <summary>
        /// 一氧化碳（CO）单项指数
        /// </summary>
        public decimal? ICO { get; set; }
        /// <summary>
        /// 臭氧（O3）单项指数
        /// </summary>
        public decimal? IO3 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）单项指数
        /// </summary>
        public decimal? IPM25 { get; set; }
    }
}

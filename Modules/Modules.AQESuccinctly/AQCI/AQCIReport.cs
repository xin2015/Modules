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
        public double? ISO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）单项指数
        /// </summary>
        public double? INO2 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）单项指数
        /// </summary>
        public double? IPM10 { get; set; }
        /// <summary>
        /// 一氧化碳（CO）单项指数
        /// </summary>
        public double? ICO { get; set; }
        /// <summary>
        /// 臭氧（O3）单项指数
        /// </summary>
        public double? IO3 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）单项指数
        /// </summary>
        public double? IPM25 { get; set; }
    }
}

namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数报表
    /// </summary>
    public class AQIReport : AQICalculate, IAQIReport
    {
        /// <summary>
        /// 二氧化硫（SO2）平均分指数
        /// </summary>
        public double? ISO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）平均分指数
        /// </summary>
        public double? INO2 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）平均分指数
        /// </summary>
        public double? IPM10 { get; set; }
        /// <summary>
        /// 一氧化碳（CO）平均分指数
        /// </summary>
        public double? ICO { get; set; }
        /// <summary>
        /// 臭氧（O3）平均分指数
        /// </summary>
        public double? IO3 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）平均分指数
        /// </summary>
        public double? IPM25 { get; set; }
        /// <summary>
        /// 空气质量指数级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 空气质量指数表示颜色
        /// </summary>
        public string Color { get; set; }
    }
}

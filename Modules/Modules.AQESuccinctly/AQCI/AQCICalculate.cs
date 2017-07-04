namespace Modules.AQE.AQCI
{
    /// <summary>
    /// 空气质量综合指数计算类
    /// </summary>
    public class AQCICalculate : IAQCICalculate
    {
        /// <summary>
        /// 二氧化硫（SO2）平均浓度（μg/m³）/第98百分位数
        /// </summary>
        public double? SO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）平均浓度（μg/m³）/第98百分位数
        /// </summary>
        public double? NO2 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）平均浓度（μg/m³）/第95百分位数
        /// </summary>
        public double? PM10 { get; set; }
        /// <summary>
        /// 一氧化碳（CO）平均浓度（mg/m³）/第95百分位数
        /// </summary>
        public double? CO { get; set; }
        /// <summary>
        /// 臭氧（O3）平均浓度（μg/m³）/第90百分位数
        /// </summary>
        public double? O3 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）平均浓度（μg/m³）/第95百分位数
        /// </summary>
        public double? PM25 { get; set; }
        /// <summary>
        /// 空气质量综合指数
        /// </summary>
        public double? AQCI { get; set; }
        /// <summary>
        /// 首要污染物
        /// </summary>
        public string PrimaryPollutant { get; set; }
    }
}

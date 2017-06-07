namespace Modules.AQE.AQCI
{
    /// <summary>
    /// 空气质量单项指数数据接口
    /// </summary>
    public interface IIAQIData
    {
        /// <summary>
        /// 二氧化硫（SO2）单项指数
        /// </summary>
        double? ISO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）单项指数
        /// </summary>
        double? INO2 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）单项指数
        /// </summary>
        double? IPM10 { get; set; }
        /// <summary>
        /// 一氧化碳（CO）单项指数
        /// </summary>
        double? ICO { get; set; }
        /// <summary>
        /// 臭氧（O3）单项指数
        /// </summary>
        double? IO3 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）单项指数
        /// </summary>
        double? IPM25 { get; set; }
    }
}

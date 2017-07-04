namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数报表接口
    /// </summary>
    public interface IAQIReport : IAQICalculate, IIAQI
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

namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数详细结果接口
    /// </summary>
    public interface IAQIResultDetail : IAQIResult
    {
        /// <summary>
        /// 超标污染物
        /// </summary>
        string NonAttainmentPollutant { get; set; }
    }
}

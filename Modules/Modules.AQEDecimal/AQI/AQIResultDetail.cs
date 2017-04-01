namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数详细结果
    /// </summary>
    public class AQIResultDetail : AQIResult, IAQIResultDetail
    {
        /// <summary>
        /// 超标污染物
        /// </summary>
        public string NonAttainmentPollutant { get; set; }
    }
}

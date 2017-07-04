namespace Modules.AQE.AQCI
{
    /// <summary>
    /// 空气质量综合指数结果接口
    /// </summary>
    public interface IAQCIResult
    {
        /// <summary>
        /// 空气质量综合指数
        /// </summary>
        double? AQCI { get; set; }
        /// <summary>
        /// 首要污染物
        /// </summary>
        string PrimaryPollutant { get; set; }
    }
}

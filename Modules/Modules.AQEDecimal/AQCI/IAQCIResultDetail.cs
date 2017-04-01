namespace Modules.AQE.AQCI
{
    /// <summary>
    /// 空气质量综合指数详细结果接口
    /// </summary>
    public interface IAQCIResultDetail : IAQCIResult
    {
        /// <summary>
        /// 空气质量最大指数
        /// </summary>
        decimal? AQMI { get; set; }
    }
}

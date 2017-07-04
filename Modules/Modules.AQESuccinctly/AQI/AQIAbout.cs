namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数相关信息
    /// </summary>
    public class AQIAbout
    {
        /// <summary>
        /// 空气质量指数上限
        /// </summary>
        public double AQIUpperLimit { get; set; }
        /// <summary>
        /// 空气质量指数级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 空气质量指数类别
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 空气质量指数表示颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 对健康影响情况
        /// </summary>
        public string Effect { get; set; }
        /// <summary>
        /// 建议采取的措施
        /// </summary>
        public string Measure { get; set; }
        /// <summary>
        /// 空气质量等级数字表示
        /// </summary>
        public int NumberLevel { get; set; }
    }
}

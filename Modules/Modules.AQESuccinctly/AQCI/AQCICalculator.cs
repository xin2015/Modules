using Modules.FastReflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modules.AQE.AQCI
{
    /// <summary>
    /// 空气质量综合指数计算器
    /// </summary>
    public class AQCICalculator : AirQualityCalculator
    {
        /// <summary>
        /// 污染物监测项浓度二级标准字典
        /// </summary>
        protected Dictionary<string, double> LimitDic { get; set; }

        /// <summary>
        /// 空气质量综合指数计算器构造函数
        /// </summary>
        public AQCICalculator()
        {
            LimitDic = new Dictionary<string, double>(){
                {"SO2",60},
                {"NO2",40},
                {"PM10",70},
                {"CO",4},
                {"O3",160},
                {"PM25",35}
            };
        }

        #region 私有方法
        /// <summary>
        /// 计算空气质量单项指数
        /// </summary>
        /// <param name="pollutant">污染物监测项，命名同IAQMData</param>
        /// <param name="value">浓度值</param>
        /// <returns>空气质量单项指数</returns>
        protected virtual double GetIAQI(string pollutant, double value)
        {
            return Math.Round(value / LimitDic[pollutant], 2);
        }

        /// <summary>
        /// 计算AQCI和PrimaryPollutant，并赋值IAQCIResult
        /// </summary>
        /// <param name="result">空气质量综合指数结果接口</param>
        /// <param name="IAQIDic">空气质量单项指数字典</param>
        protected virtual void CalculateAQCI(IAQCIResult result, Dictionary<string, double> IAQIDic)
        {
            if (IAQIDic.Count == 6)
            {
                result.AQCI = IAQIDic.Sum(o => o.Value);
                double AQMI = IAQIDic.Max(o => o.Value);
                result.PrimaryPollutant = string.Join(",", IAQIDic.Where(o => o.Value == AQMI).Select(o => o.Key));
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 计算百分位数
        /// </summary>
        /// <param name="values">按从小到大排序的数据，且数组长度需大于1</param>
        /// <param name="p">百分位</param>
        /// <returns>百分位数</returns>
        public virtual double CalculatePercentile(double[] values, double p)
        {
            double k = (values.Length - 1) * p;
            int s = (int)Math.Floor(k);
            double percentile = values[s] + (k - s) * (values[s + 1] - values[s]);
            return percentile;
        }

        /// <summary>
        /// 计算空气质量单项指数
        /// </summary>
        /// <param name="pollutant">污染物监测项，命名同IAirQuality</param>
        /// <param name="value">浓度值</param>
        /// <returns>空气质量单项指数</returns>
        public virtual double? GetIAQI(string pollutant, double? value)
        {
            if (value.HasValue && value >= 0)
            {
                return GetIAQI(pollutant, value.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 计算空气质量单项指数字典
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值/百分位数数据接口</param>
        /// <returns>空气质量单项指数字典</returns>
        public virtual Dictionary<string, double> GetIAQIDic(IAirQuality data)
        {
            Dictionary<string, double> IAQIDic = new Dictionary<string, double>();
            foreach (var item in IAirQualityPropertyAccessorDic)
            {
                double? value = item.Value.GetValue(data) as double?;
                if (value.HasValue && value >= 0)
                {
                    IAQIDic.Add(item.Key, GetIAQI(item.Key, value.Value));
                }
            }
            return IAQIDic;
        }

        /// <summary>
        /// 计算空气质量综合指数结果
        /// </summary>
        /// <param name="IAQIDic">空气质量单项指数字典</param>
        /// <returns>空气质量综合指数结果</returns>
        public virtual AQCIResult GetAQCIResultByIAQIDic(Dictionary<string, double> IAQIDic)
        {
            AQCIResult result = new AQCIResult();
            CalculateAQCI(result, IAQIDic);
            return result;
        }

        /// <summary>
        /// 计算空气质量综合指数结果
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值/百分位数数据接口</param>
        /// <returns>空气质量综合指数结果</returns>
        public virtual AQCIResult GetAQCIResult(IAirQuality data)
        {
            Dictionary<string, double> IAQIDic = GetIAQIDic(data);
            return GetAQCIResultByIAQIDic(IAQIDic);
        }

        /// <summary>
        /// 计算空气质量综合指数
        /// </summary>
        /// <param name="calculate">空气质量综合指数计算接口</param>
        public virtual void CalculateAQCI(IAQCICalculate calculate)
        {
            Dictionary<string, double> IAQIDic = GetIAQIDic(calculate);
            CalculateAQCI(calculate, IAQIDic);
        }

        /// <summary>
        /// 计算空气质量综合指数报表
        /// </summary>
        /// <param name="report">空气质量综合指数报表接口</param>
        public virtual void CalculateAQCI(IAQCIReport report)
        {
            Dictionary<string, double> IAQIDic = GetIAQIDic(report);
            CalculateIAQI(report, IAQIDic);
            CalculateAQCI(report, IAQIDic);
        }

        /// <summary>
        /// 计算最大空气质量单项指数
        /// </summary>
        /// <param name="IAQIDic">空气质量单项指数字典</param>
        /// <returns>最大空气质量单项指数</returns>
        public virtual double? GetAQMIByIAQIDic(Dictionary<string, double> IAQIDic)
        {
            if (IAQIDic.Count > 0)
            {
                return IAQIDic.Max(o => o.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 计算最大空气质量单项指数
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值/百分位数数据接口</param>
        /// <returns>最大空气质量单项指数</returns>
        public virtual double? GetAQMI(IAirQuality data)
        {
            Dictionary<string, double> IAQIDic = GetIAQIDic(data);
            return GetAQMIByIAQIDic(IAQIDic);
        }
        #endregion
    }
}

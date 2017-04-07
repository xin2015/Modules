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
    public class AQCICalculator : AQCalculator
    {
        #region 字段参数
        /// <summary>
        /// 污染物监测项浓度二级标准字典
        /// </summary>
        private static Dictionary<string, int> limitDic;
        #endregion
        #region 属性参数
        /// <summary>
        /// ISAQIData属性字典
        /// </summary>
        public static Dictionary<string, IPropertyAccessor> IIAQIDataPropertiesDic { get; }
        #endregion

        static AQCICalculator()
        {
            limitDic = new Dictionary<string, int>(){
                {"SO2",60},
                {"NO2",40},
                {"PM10",70},
                {"CO",4},
                {"O3",160},
                {"PM25",35}
            };
            PropertyAccessorFactory factory = new PropertyAccessorFactory();
            PropertyInfo[] IIAQIDataProperties = typeof(IIAQIData).GetProperties();
            IIAQIDataPropertiesDic = new Dictionary<string, IPropertyAccessor>();
            foreach (string pollutant in limitDic.Keys)
            {
                IIAQIDataPropertiesDic.Add(pollutant, factory.Get(IIAQIDataProperties.First(o => o.Name == string.Format("I{0}", pollutant))));
            }
        }

        #region 私有方法
        /// <summary>
        /// 计算空气质量单项指数
        /// </summary>
        /// <param name="pollutant">污染物监测项，命名同IAQMData</param>
        /// <param name="value">浓度值</param>
        /// <returns>空气质量单项指数</returns>
        private static decimal GetIAQI(string pollutant, decimal value)
        {
            return Math.Round(value / limitDic[pollutant], 2);
        }

        /// <summary>
        /// 赋值IIAQIData
        /// </summary>
        /// <param name="result">空气质量单项指数数据接口</param>
        /// <param name="IAQIDic">空气质量单项指数字典</param>
        private static void CalculateIAQI(IIAQIData result, Dictionary<string, decimal> IAQIDic)
        {
            foreach (var item in IAQIDic)
            {
                IIAQIDataPropertiesDic[item.Key].SetValue(result, item.Value);
            }
        }

        /// <summary>
        /// 计算AQCI和PrimaryPollutant，并赋值IAQCIResult
        /// </summary>
        /// <param name="result">空气质量综合指数结果接口</param>
        /// <param name="IAQIDic">空气质量单项指数字典</param>
        private static void CalculateAQCI(IAQCIResult result, Dictionary<string, decimal> IAQIDic)
        {
            if (IAQIDic.Count == 6)
            {
                result.AQCI = IAQIDic.Sum(o => o.Value);
                decimal AQMI = IAQIDic.Max(o => o.Value);
                result.PrimaryPollutant = string.Join(",", IAQIDic.Where(o => o.Value == AQMI).Select(o => o.Key));
            }
        }

        /// <summary>
        /// 计算AQCI，AQMI和PrimaryPollutant，并赋值IAQCIResultDetail
        /// </summary>
        /// <param name="result">空气质量综合指数详细结果接口</param>
        /// <param name="IAQIDic">空气质量单项指数字典</param>
        private static void CalculateAQCI(IAQCIResultDetail result, Dictionary<string, decimal> IAQIDic)
        {
            if (IAQIDic.Count == 6)
            {
                result.AQCI = IAQIDic.Sum(o => o.Value);
                result.AQMI = IAQIDic.Max(o => o.Value);
                result.PrimaryPollutant = string.Join(",", IAQIDic.Where(o => o.Value == result.AQMI).Select(o => o.Key));
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
        public static decimal CalculatePercentile(decimal[] values, decimal p)
        {
            decimal k = (values.Length - 1) * p;
            int s = (int)Math.Floor(k);
            decimal percentile = values[s] + (k - s) * (values[s + 1] - values[s]);
            return percentile;
        }

        /// <summary>
        /// 计算空气质量单项指数
        /// </summary>
        /// <param name="pollutant">污染物监测项，命名同IAQMData</param>
        /// <param name="value">浓度值</param>
        /// <returns>空气质量单项指数</returns>
        public static decimal? GetIAQI(string pollutant, decimal? value)
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
        /// <param name="concentrationsDic">空气质量基本评价项目浓度值字典</param>
        /// <returns>空气质量单项指数字典</returns>
        public static Dictionary<string, decimal> GetIAQIDic(Dictionary<string, decimal?> concentrationsDic)
        {
            Dictionary<string, decimal> IAQIDic = new Dictionary<string, decimal>();
            foreach (var item in concentrationsDic)
            {
                if (item.Value.HasValue && item.Value >= 0)
                {
                    IAQIDic.Add(item.Key, GetIAQI(item.Key, item.Value.Value));
                }
            }
            return IAQIDic;
        }

        /// <summary>
        /// 计算空气质量单项指数字典
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>空气质量单项指数字典</returns>
        public static Dictionary<string, decimal> GetIAQIDic(IAQData data)
        {
            Dictionary<string, decimal> IAQIDic = new Dictionary<string, decimal>();
            foreach (var item in IAQMDataPropertiesDic)
            {
                decimal? value = item.Value.GetValue(data) as decimal?;
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
        public static AQCIResult GetAQCIResult(Dictionary<string, decimal> IAQIDic)
        {
            AQCIResult result = new AQCIResult();
            CalculateAQCI(result, IAQIDic);
            return result;
        }

        /// <summary>
        /// 计算空气质量综合指数结果
        /// </summary>
        /// <param name="concentrationsDic">空气质量基本评价项目浓度值字典</param>
        /// <returns>空气质量综合指数结果</returns>
        public static AQCIResult GetAQCIResult(Dictionary<string, decimal?> concentrationsDic)
        {
            Dictionary<string, decimal> IAQIDic = GetIAQIDic(concentrationsDic);
            return GetAQCIResult(IAQIDic);
        }

        /// <summary>
        /// 计算空气质量综合指数结果
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>空气质量综合指数结果</returns>
        public static AQCIResult GetAQCIResult(IAQData data)
        {
            Dictionary<string, decimal> IAQIDic = GetIAQIDic(data);
            return GetAQCIResult(IAQIDic);
        }

        /// <summary>
        /// 计算空气质量综合指数详细结果
        /// </summary>
        /// <param name="IAQIDic">空气质量单项指数字典</param>
        /// <returns>空气质量综合指数详细结果</returns>
        public static AQCIResultDetail GetAQCIResultDetail(Dictionary<string, decimal> IAQIDic)
        {
            AQCIResultDetail result = new AQCIResultDetail();
            CalculateAQCI(result, IAQIDic);
            return result;
        }

        /// <summary>
        /// 计算空气质量综合指数
        /// </summary>
        /// <param name="calculate">空气质量综合指数计算接口</param>
        public static void CalculateAQCI(IAQCICalculate calculate)
        {
            Dictionary<string, decimal> IAQIDic = GetIAQIDic(calculate);
            CalculateAQCI(calculate, IAQIDic);
        }

        /// <summary>
        /// 计算空气质量综合指数报表
        /// </summary>
        /// <param name="report">空气质量综合指数报表接口</param>
        public static void CalculateAQCI(IAQCIReport report)
        {
            Dictionary<string, decimal> IAQIDic = GetIAQIDic(report);
            CalculateIAQI(report, IAQIDic);
            CalculateAQCI(report, IAQIDic);
        }
        #endregion
    }
}

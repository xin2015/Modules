using Modules.FastReflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数计算器
    /// </summary>
    public class AQICalculator : AirQualityCalculator
    {
        /// <summary>
        /// 空气质量分指数限值数组
        /// </summary>
        protected double[] IAQILimits { get; set; }
        /// <summary>
        /// 二氧化硫（SO2）平均浓度限值
        /// </summary>
        protected double[] SO2ConcentrationLimits { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）平均浓度限值
        /// </summary>
        protected double[] NO2ConcentrationLimits { get; set; }
        /// <summary>
        /// 颗粒物（PM10）平均浓度限值
        /// </summary>
        protected double[] PM10ConcentrationLimits { get; set; }
        /// <summary>
        /// 一氧化碳（CO）平均浓度限值
        /// </summary>
        protected double[] COConcentrationLimits { get; set; }
        /// <summary>
        /// 臭氧（O3）平均浓度限值
        /// </summary>
        protected double[] O3ConcentrationLimits { get; set; }
        /// <summary>
        /// 细颗粒物（PM2.5）平均浓度限值
        /// </summary>
        protected double[] PM25ConcentrationLimits { get; set; }
        /// <summary>
        /// 污染物浓度限值字典
        /// </summary>
        protected Dictionary<string, double[]> ConcentrationLimitsDic { get; set; }
        /// <summary>
        /// 首要污染物限值
        /// </summary>
        protected double PrimaryPollutantLimit { get; set; }
        /// <summary>
        /// 超标污染物限值
        /// </summary>
        protected double NonAttainmentPollutantLimit { get; set; }
        /// <summary>
        /// AQI相关信息集合
        /// </summary>
        protected List<AQIAbout> AQIAbouts { get; }
        /// <summary>
        /// AQI相关信息字典
        /// </summary>
        protected Dictionary<string, AQIAbout> AQIAboutDic { get; }

        /// <summary>
        /// 空气质量指数计算器构造函数
        /// </summary>
        /// <param name="SO2ConcentrationLimits">二氧化硫（SO2）平均浓度限值</param>
        /// <param name="NO2ConcentrationLimits">二氧化氮（NO2）平均浓度限值</param>
        /// <param name="COConcentrationLimits">一氧化碳（CO）平均浓度限值</param>
        /// <param name="O3ConcentrationLimits">臭氧（O3）平均浓度限值</param>
        public AQICalculator(double[] SO2ConcentrationLimits, double[] NO2ConcentrationLimits, double[] COConcentrationLimits, double[] O3ConcentrationLimits)
        {
            IAQILimits = new double[] { 0, 50, 100, 150, 200, 300, 400, 500 };
            PrimaryPollutantLimit = 50;
            NonAttainmentPollutantLimit = 100;
            AQIAbouts = new List<AQIAbout>();
            AQIAbouts.Add(new AQIAbout()
            {
                AQIUpperLimit = IAQILimits[1],
                Level = "一级",
                NumberLevel = 1,
                Type = "优",
                Color = "绿色",
                Effect = "空气质量令人满意，基本无空气污染",
                Measure = "各类人群可正常活动"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQIUpperLimit = IAQILimits[2],
                Level = "二级",
                NumberLevel = 2,
                Type = "良",
                Color = "黄色",
                Effect = "空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响",
                Measure = "极少数异常敏感人群应减少户外活动"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQIUpperLimit = IAQILimits[3],
                Level = "三级",
                NumberLevel = 3,
                Type = "轻度污染",
                Color = "橙色",
                Effect = "易感人群症状有轻度加剧，健康人群出现刺激症状",
                Measure = "儿童、老年人及心脏病、呼吸系统疾病患者应减少长时间、高强度的户外锻炼"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQIUpperLimit = IAQILimits[4],
                Level = "四级",
                NumberLevel = 4,
                Type = "中度污染",
                Color = "红色",
                Effect = "进一步加剧易感人群症状，可能对健康人群心脏、呼吸系统有影响",
                Measure = "儿童、老年人及心脏病、呼吸系统疾病患者避免长时间、高强度的户外锻炼，一般人群适量减少户外运动"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQIUpperLimit = IAQILimits[5],
                Level = "五级",
                NumberLevel = 5,
                Type = "重度污染",
                Color = "紫色",
                Effect = "心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出现症状",
                Measure = "儿童、老年人和心脏病、肺病患者应停留在室内，停止户外运动，一般人群减少户外运动"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQIUpperLimit = IAQILimits[7],
                Level = "六级",
                NumberLevel = 6,
                Type = "严重污染",
                Color = "褐红色",
                Effect = "健康人群运动耐受力降低，有明显强烈症状，提前出现某些疾病",
                Measure = "儿童、老年人和病人应当留在室内，避免体力消耗，一般人群应避免户外活动"
            });
            AQIAboutDic = AQIAbouts.ToDictionary(o => o.Type);
            this.SO2ConcentrationLimits = SO2ConcentrationLimits;
            this.NO2ConcentrationLimits = NO2ConcentrationLimits;
            PM10ConcentrationLimits = new double[] { 0, 50, 150, 250, 350, 420, 500, 600 };
            this.COConcentrationLimits = COConcentrationLimits;
            this.O3ConcentrationLimits = O3ConcentrationLimits;
            PM25ConcentrationLimits = new double[] { 0, 35, 75, 115, 150, 250, 350, 500 };
            ConcentrationLimitsDic = new Dictionary<string, double[]>()
            {
                {"SO2", SO2ConcentrationLimits },
                {"NO2", NO2ConcentrationLimits },
                {"PM10", PM10ConcentrationLimits },
                {"CO", COConcentrationLimits },
                {"O3", O3ConcentrationLimits },
                {"PM25", PM25ConcentrationLimits }
            };
        }

        #region 私有方法
        /// <summary>
        /// 计算空气质量分指数
        /// </summary>
        /// <param name="value">浓度值</param>
        /// <param name="concentrationLimits">浓度限值数组</param>
        /// <returns>空气质量分指数</returns>
        protected virtual double GetIAQI(double value, double[] concentrationLimits)
        {
            for (int i = 1; i < concentrationLimits.Length; i++)
            {
                if (value <= concentrationLimits[i])
                {
                    return Math.Ceiling((IAQILimits[i] - IAQILimits[i - 1]) * (value - concentrationLimits[i - 1]) / (concentrationLimits[i] - concentrationLimits[i - 1])) + IAQILimits[i - 1];
                }
            }
            return 500;
        }

        /// <summary>
        /// 计算AQI和PrimaryPollutant，并赋值IAQIResult
        /// </summary>
        /// <param name="result">空气质量指数结果接口</param>
        /// <param name="IAQIDic">空气质量分指数字典</param>
        protected virtual void CalculateAQI(IAQIResult result, Dictionary<string, double> IAQIDic)
        {
            if (IAQIDic.Count > 0)
            {
                result.AQI = IAQIDic.Max(o => o.Value);
                if (result.AQI > PrimaryPollutantLimit)
                {
                    result.PrimaryPollutant = string.Join(",", IAQIDic.Where(o => o.Value == result.AQI).Select(o => o.Key));
                }
            }
        }

        /// <summary>
        /// 计算AQIAbout，并赋值IAQIResult（此方法调用前，需先计算AQI）
        /// </summary>
        /// <param name="result">空气质量指数结果接口</param>
        protected virtual void CalculateAQIAbout(IAQIResult result)
        {
            AQIAbout about = GetAQIAbout(result.AQI);
            if (about != null)
            {
                result.Type = about.Type;
            }
        }

        /// <summary>
        /// 计算AQIAbout，并赋值IAQIReport（此方法调用前，需先计算AQI）
        /// </summary>
        /// <param name="result">空气质量指数报表接口</param>
        protected virtual void CalculateAQIAbout(IAQIReport result)
        {
            AQIAbout about = GetAQIAbout(result.AQI);
            if (about != null)
            {
                result.Type = about.Type;
                result.Level = about.Level;
                result.Color = about.Color;
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 计算空气质量分指数
        /// </summary>
        /// <param name="pollutant">污染物监测项，命名同IAirQuality</param>
        /// <param name="value">浓度值</param>
        /// <returns>空气质量分指数</returns>
        public virtual double? GetIAQI(string pollutant, double? value)
        {
            if (value.HasValue && value >= 0)
            {
                double[] concentrationLimits = ConcentrationLimitsDic[pollutant];
                if (concentrationLimits.Length == 8 || value <= concentrationLimits.Last())
                {
                    return GetIAQI(value.Value, concentrationLimits);
                }
            }
            return null;
        }

        /// <summary>
        /// 计算空气质量分指数字典
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>空气质量分指数字典</returns>
        public virtual Dictionary<string, double> GetIAQIDic(IAirQuality data)
        {
            Dictionary<string, double> IAQIDic = new Dictionary<string, double>();
            foreach (var item in IAirQualityPropertyAccessorDic)
            {
                double? value = item.Value.GetValue(data) as double?;
                if (value.HasValue && value >= 0)
                {
                    double[] concentrationLimits = ConcentrationLimitsDic[item.Key];
                    if (concentrationLimits.Length == 8 || value <= concentrationLimits.Last())
                    {
                        IAQIDic.Add(item.Key, GetIAQI(value.Value, concentrationLimits));
                    }
                }
            }
            return IAQIDic;
        }

        /// <summary>
        /// 计算空气质量指数相关信息
        /// </summary>
        /// <param name="aqi">空气质量指数</param>
        /// <returns>空气质量指数相关信息</returns>
        public virtual AQIAbout GetAQIAbout(double? aqi)
        {
            if (aqi.HasValue && aqi >= 0)
            {
                foreach (AQIAbout about in AQIAbouts)
                {
                    if (aqi <= about.AQIUpperLimit)
                    {
                        return about;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 计算空气质量指数相关信息
        /// </summary>
        /// <param name="type">空气质量指数类别</param>
        /// <returns>空气质量指数相关信息</returns>
        public virtual AQIAbout GetAQIAbout(string type)
        {
            if (AQIAboutDic.ContainsKey(type))
            {
                return AQIAboutDic[type];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 计算空气质量指数结果
        /// </summary>
        /// <param name="IAQIDic">空气质量分指数字典</param>
        /// <returns>空气质量指数结果</returns>
        public virtual AQIResult GetAQIResultByIAQIDic(Dictionary<string, double> IAQIDic)
        {
            AQIResult result = new AQIResult();
            CalculateAQI(result, IAQIDic);
            CalculateAQIAbout(result);
            return result;
        }

        /// <summary>
        /// 计算空气质量指数结果
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>空气质量指数结果</returns>
        public virtual AQIResult GetAQIResult(IAirQuality data)
        {
            Dictionary<string, double> IAQIDic = GetIAQIDic(data);
            return GetAQIResultByIAQIDic(IAQIDic);
        }

        /// <summary>
        /// 计算空气质量指数
        /// </summary>
        /// <param name="calculate">空气质量指数计算接口</param>
        public virtual void CalculateAQI(IAQICalculate calculate)
        {
            Dictionary<string, double> IAQIDic = GetIAQIDic(calculate);
            CalculateAQI(calculate, IAQIDic);
            CalculateAQIAbout(calculate);
        }

        /// <summary>
        /// 计算空气质量指数实时报/日报
        /// </summary>
        /// <param name="report">空气质量指数实时报/日报接口</param>
        public virtual void CalculateAQI(IAQIReport report)
        {
            Dictionary<string, double> IAQIDic = GetIAQIDic(report);
            CalculateIAQI(report, IAQIDic);
            CalculateAQI(report, IAQIDic);
            CalculateAQIAbout(report);
        }

        /// <summary>
        /// 计算超标污染物
        /// </summary>
        /// <param name="IAQIDic">空气质量分指数字典</param>
        /// <returns>超标污染物</returns>
        public virtual string GetNonAttainmentPollutantByIAQIDic(Dictionary<string, double> IAQIDic)
        {
            if (IAQIDic.Count > 0)
            {
                double AQI = IAQIDic.Max(o => o.Value);
                if (AQI > NonAttainmentPollutantLimit)
                {
                    return string.Join(",", IAQIDic.Where(o => o.Value > NonAttainmentPollutantLimit).Select(o => o.Key));
                }
            }
            return null;
        }

        /// <summary>
        /// 计算超标污染物
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>超标污染物</returns>
        public virtual string GetNonAttainmentPollutant(IAirQuality data)
        {
            Dictionary<string, double> IAQIDic = GetIAQIDic(data);
            return GetNonAttainmentPollutantByIAQIDic(IAQIDic);
        }
        #endregion
    }
}

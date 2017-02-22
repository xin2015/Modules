using Modules.FastReflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE.AQI
{
    /// <summary>
    /// 空气质量指数计算器
    /// </summary>
    public class AQICalculator
    {
        #region 字段参数
        #region 浓度限值
        /// <summary>
        /// 空气质量分指数限值数组
        /// </summary>
        private static int[] IAQILimits = { 0, 50, 100, 150, 200, 300, 400, 500 };
        /// <summary>
        /// 二氧化硫（SO2）24小时平均浓度限值
        /// </summary>
        private static int[] SO2DayConcentrationLimits = { 0, 50, 150, 475, 800, 1600, 2100, 2620 };
        /// <summary>
        /// 二氧化硫（SO2）1小时平均浓度限值
        /// </summary>
        private static int[] SO2HourConcentrationLimits = { 0, 150, 500, 650, 800 };
        /// <summary>
        /// 二氧化氮（NO2）24小时平均浓度限值
        /// </summary>
        private static int[] NO2DayConcentrationLimits = { 0, 40, 80, 180, 280, 565, 750, 940 };
        /// <summary>
        /// 二氧化氮（NO2）1小时平均浓度限值
        /// </summary>
        private static int[] NO2HourConcentrationLimits = { 0, 100, 200, 700, 1200, 2340, 3090, 3840 };
        /// <summary>
        /// 可吸入颗粒物（PM10）24小时平均浓度限值
        /// </summary>
        private static int[] PM10DayConcentrationLimits = { 0, 50, 150, 250, 350, 420, 500, 600 };
        /// <summary>
        /// 可吸入颗粒物（PM10）24小时平均浓度限值
        /// </summary>
        private static int[] PM10HourConcentrationLimits = { 0, 50, 150, 250, 350, 420, 500, 600 };
        /// <summary>
        /// 一氧化碳（CO）24小时平均浓度限值
        /// </summary>
        private static int[] CODayConcentrationLimits = { 0, 2, 4, 14, 24, 36, 48, 60 };
        /// <summary>
        /// 一氧化碳（CO）1小时平均浓度限值
        /// </summary>
        private static int[] COHourConcentrationLimits = { 0, 5, 10, 35, 60, 90, 120, 150 };
        /// <summary>
        /// 臭氧（O3）8小时滑动平均浓度限值
        /// </summary>
        private static int[] O3DayConcentrationLimits = { 0, 100, 160, 215, 265, 800 };
        /// <summary>
        /// 臭氧（O3）1小时平均浓度限值
        /// </summary>
        private static int[] O3HourConcentrationLimits = { 0, 160, 200, 300, 400, 800, 1000, 1200 };
        /// <summary>
        /// 细颗粒物（PM2.5）24小时平均浓度限值
        /// </summary>
        private static int[] PM25DayConcentrationLimits = { 0, 35, 75, 115, 150, 250, 350, 500 };
        /// <summary>
        /// 细颗粒物（PM2.5）24小时平均浓度限值
        /// </summary>
        private static int[] PM25HourConcentrationLimits = { 0, 35, 75, 115, 150, 250, 350, 500 };
        #endregion
        #region 其他
        /// <summary>
        /// 首要污染物限值
        /// </summary>
        private static int primaryPollutantLimit = 50;
        /// <summary>
        /// 超标污染物限值
        /// </summary>
        private static int nonAttainmentPollutantLimit = 100;
        /// <summary>
        /// IAQMData属性
        /// </summary>
        private static IPropertyAccessor[] IAQMDataProperties;
        /// <summary>
        /// IIAQIData属性字典
        /// </summary>
        private static Dictionary<string, IPropertyAccessor> IIAQIDataPropertiesDic;
        /// <summary>
        /// 日报浓度限值字典
        /// </summary>
        private static Dictionary<string, int[]> dayConcentrationLimitsDic;
        /// <summary>
        /// 实时报浓度限值字典
        /// </summary>
        private static Dictionary<string, int[]> hourConcentrationLimitsDic;
        #endregion
        #endregion
        #region 属性参数
        /// <summary>
        /// AQI相关信息集合
        /// </summary>
        public static List<AQIAbout> AQIAbouts { get; set; }
        /// <summary>
        /// AQI相关信息字典
        /// </summary>
        public static Dictionary<string, AQIAbout> AQIAboutDic { get; set; }
        #endregion

        static AQICalculator()
        {
            dayConcentrationLimitsDic = new Dictionary<string, int[]>(){
                {"SO2",SO2DayConcentrationLimits},
                {"NO2",NO2DayConcentrationLimits},
                {"PM10",PM10DayConcentrationLimits},
                {"CO",CODayConcentrationLimits},
                {"O3",O3DayConcentrationLimits},
                {"PM25",PM25DayConcentrationLimits}
            };
            hourConcentrationLimitsDic = new Dictionary<string, int[]>(){
                {"SO2",SO2HourConcentrationLimits},
                {"NO2",NO2HourConcentrationLimits},
                {"PM10",PM10HourConcentrationLimits},
                {"CO",COHourConcentrationLimits},
                {"O3",O3HourConcentrationLimits},
                {"PM25",PM25HourConcentrationLimits}
            };
            PropertyAccessorFactory factory = new PropertyAccessorFactory();
            IAQMDataProperties = typeof(IAQMData).GetProperties().Select(o => factory.Get(o)).ToArray();
            PropertyInfo[] IIAQIDataProperties = typeof(IIAQIData).GetProperties();
            IIAQIDataPropertiesDic = new Dictionary<string, IPropertyAccessor>();
            foreach (string pollutant in dayConcentrationLimitsDic.Keys)
            {
                IIAQIDataPropertiesDic.Add(pollutant, factory.Get(IIAQIDataProperties.First(o => o.Name == string.Format("I{0}", pollutant))));
            }
            AQIAbouts = new List<AQIAbout>();
            AQIAbouts.Add(new AQIAbout()
            {
                AQILowLimit = 0,
                Level = "一级",
                Type = "优",
                Color = "绿色",
                Effect = "空气质量令人满意，基本无空气污染",
                Measure = "各类人群可正常活动"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQILowLimit = 51,
                Level = "二级",
                Type = "良",
                Color = "黄色",
                Effect = "空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响",
                Measure = "极少数异常敏感人群应减少户外活动"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQILowLimit = 101,
                Level = "三级",
                Type = "轻度污染",
                Color = "橙色",
                Effect = "易感人群症状有轻度加剧，健康人群出现刺激症状",
                Measure = "儿童、老年人及心脏病、呼吸系统疾病患者应减少长时间、高强度的户外锻炼"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQILowLimit = 151,
                Level = "四级",
                Type = "中度污染",
                Color = "红色",
                Effect = "进一步加剧易感人群症状，可能对健康人群心脏、呼吸系统有影响",
                Measure = "儿童、老年人及心脏病、呼吸系统疾病患者避免长时间、高强度的户外锻炼，一般人群适量减少户外运动"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQILowLimit = 201,
                Level = "五级",
                Type = "重度污染",
                Color = "紫色",
                Effect = "心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出现症状",
                Measure = "儿童、老年人和心脏病、肺病患者应停留在室内，停止户外运动，一般人群减少户外运动"
            });
            AQIAbouts.Add(new AQIAbout()
            {
                AQILowLimit = 301,
                Level = "六级",
                Type = "严重污染",
                Color = "褐红色",
                Effect = "健康人群运动耐受力降低，有明显强烈症状，提前出现某些疾病",
                Measure = "儿童、老年人和病人应当留在室内，避免体力消耗，一般人群应避免户外活动"
            });
            AQIAboutDic = new Dictionary<string, AQIAbout>();
            AQIAbouts.ForEach(o => AQIAboutDic.Add(o.Type, o));
            AQIAbouts.Reverse();
        }

        #region 私有方法
        /// <summary>
        /// 计算空气质量分指数
        /// </summary>
        /// <param name="value">浓度值</param>
        /// <param name="concentrationLimits">浓度限值数组</param>
        /// <returns>空气质量分指数</returns>
        private static int? GetIAQI(decimal? value, int[] concentrationLimits)
        {
            if (value.HasValue && value >= 0)
            {
                for (int i = 1; i < concentrationLimits.Length; i++)
                {
                    if (value <= concentrationLimits[i])
                    {
                        return (int)Math.Ceiling((IAQILimits[i] - IAQILimits[i - 1]) * (value.Value - concentrationLimits[i - 1]) / (concentrationLimits[i] - concentrationLimits[i - 1])) + IAQILimits[i - 1];
                    }
                }
                return 500;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 计算空气质量分指数字典
        /// </summary>
        /// <param name="concentrationsDic">空气质量基本评价项目浓度值字典</param>
        /// <param name="concentrationLimitsDic">浓度限值数组字典</param>
        /// <returns>空气质量分指数字典</returns>
        private static Dictionary<string, int?> GetIAQIDic(Dictionary<string, decimal?> concentrationsDic, Dictionary<string, int[]> concentrationLimitsDic)
        {
            Dictionary<string, int?> IAQIDic = new Dictionary<string, int?>();
            foreach (var item in concentrationsDic)
            {
                IAQIDic.Add(item.Key, GetIAQI(item.Value, concentrationLimitsDic[item.Key]));
            }
            return IAQIDic;
        }

        /// <summary>
        /// 计算空气质量分指数字典
        /// </summary>
        /// <param name="data">空气质量基本评价项目浓度值数据接口</param>
        /// <param name="concentrationLimitsDic">浓度限值数组字典</param>
        /// <returns>空气质量分指数字典</returns>
        private static Dictionary<string, int?> GetIAQIDic(IAQMData data, Dictionary<string, int[]> concentrationLimitsDic)
        {
            Dictionary<string, int?> IAQIDic = new Dictionary<string, int?>();
            foreach (IPropertyAccessor property in IAQMDataProperties)
            {
                decimal? value = property.GetValue(data) as decimal?;
                IAQIDic.Add(property.Property.Name, GetIAQI(value, concentrationLimitsDic[property.Property.Name]));
            }
            return IAQIDic;
        }

        /// <summary>
        /// 赋值IIAQIData
        /// </summary>
        /// <param name="result">空气质量分指数数据接口</param>
        /// <param name="IAQIDic">空气质量分指数字典</param>
        private static void CalculateIAQI(IIAQIData result, Dictionary<string, int?> IAQIDic)
        {
            foreach (var item in IAQIDic)
            {
                IIAQIDataPropertiesDic[item.Key].SetValue(result, item.Value);
            }
        }

        /// <summary>
        /// 计算AQI和PrimaryPollutant，并赋值IAQIResult
        /// </summary>
        /// <param name="result">空气质量指数结果接口</param>
        /// <param name="IAQIDic">空气质量分指数字典</param>
        private static void CalculateAQI(IAQIResult result, Dictionary<string, int?> IAQIDic)
        {
            if (IAQIDic.Any(o => o.Value.HasValue))
            {
                result.AQI = IAQIDic.Max(o => o.Value);
                if (result.AQI > primaryPollutantLimit)
                {
                    result.PrimaryPollutant = string.Join(",", IAQIDic.Where(o => o.Value == result.AQI).Select(o => o.Key));
                }
            }
        }

        /// <summary>
        /// 计算AQI，PrimaryPollutant和NonAttainmentPollutant，并赋值IAQIResultDetail
        /// </summary>
        /// <param name="result">空气质量指数详细结果接口</param>
        /// <param name="IAQIDic">空气质量分指数字典</param>
        private static void CalculateAQI(IAQIResultDetail result, Dictionary<string, int?> IAQIDic)
        {
            if (IAQIDic.Any(o => o.Value.HasValue))
            {
                result.AQI = IAQIDic.Max(o => o.Value);
                if (result.AQI > primaryPollutantLimit)
                {
                    result.PrimaryPollutant = string.Join(",", IAQIDic.Where(o => o.Value == result.AQI).Select(o => o.Key));
                    if (result.AQI > nonAttainmentPollutantLimit)
                    {
                        result.NonAttainmentPollutant = string.Join(",", IAQIDic.Where(o => o.Value > nonAttainmentPollutantLimit).Select(o => o.Key));
                    }
                }
            }
        }

        /// <summary>
        /// 计算AQIAbout，并赋值IAQIResult（此方法调用前，需先计算AQI）
        /// </summary>
        /// <param name="result">空气质量指数结果接口</param>
        private static void CalculateAQIAbout(IAQIResult result)
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
        private static void CalculateAQIAbout(IAQIReport result)
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
        /// 计算小时空气质量分指数字典
        /// </summary>
        /// <param name="dic">空气质量基本评价项目浓度值字典</param>
        /// <returns>小时空气质量分指数字典</returns>
        public static Dictionary<string, int?> GetHourIAQIDic(Dictionary<string, decimal?> dic)
        {
            return GetIAQIDic(dic, hourConcentrationLimitsDic);
        }

        /// <summary>
        /// 计算日均空气质量分指数字典
        /// </summary>
        /// <param name="dic">空气质量基本评价项目浓度值字典</param>
        /// <returns>日均空气质量分指数字典</returns>
        public static Dictionary<string, int?> GetDayIAQIDic(Dictionary<string, decimal?> dic)
        {
            return GetIAQIDic(dic, dayConcentrationLimitsDic);
        }

        /// <summary>
        /// 计算小时空气质量分指数字典
        /// </summary>
        /// <param name="dic">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>小时空气质量分指数字典</returns>
        public static Dictionary<string, int?> GetHourIAQIDic(IAQMData data)
        {
            return GetIAQIDic(data, hourConcentrationLimitsDic);
        }

        /// <summary>
        /// 计算日均空气质量分指数字典
        /// </summary>
        /// <param name="dic">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>日均空气质量分指数字典</returns>
        public static Dictionary<string, int?> GetDayIAQIDic(IAQMData data)
        {
            return GetIAQIDic(data, dayConcentrationLimitsDic);
        }

        /// <summary>
        /// 计算空气质量指数相关信息
        /// </summary>
        /// <param name="aqi">空气质量指数</param>
        /// <returns>空气质量指数相关信息</returns>
        public static AQIAbout GetAQIAbout(int? aqi)
        {
            if (aqi.HasValue && aqi >= 0)
            {
                foreach (AQIAbout about in AQIAbouts)
                {
                    if (aqi >= about.AQILowLimit)
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
        public static AQIAbout GetAQIAbout(string type)
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
        public static AQIResult GetAQIResult(Dictionary<string, int?> IAQIDic)
        {
            AQIResult result = new AQIResult();
            CalculateAQI(result, IAQIDic);
            CalculateAQIAbout(result);
            return result;
        }

        /// <summary>
        /// 计算小时空气质量指数结果
        /// </summary>
        /// <param name="IAQIDic">空气质量基本评价项目小时浓度值字典</param>
        /// <returns>小时空气质量指数结果</returns>
        public static AQIResult GetHourAQIResult(Dictionary<string, decimal?> dic)
        {
            Dictionary<string, int?> IAQIDic = GetHourIAQIDic(dic);
            return GetAQIResult(IAQIDic);
        }

        /// <summary>
        /// 计算日均空气质量指数结果
        /// </summary>
        /// <param name="IAQIDic">空气质量基本评价项目日均浓度值字典</param>
        /// <returns>日均空气质量指数结果</returns>
        public static AQIResult GetDayAQIResult(Dictionary<string, decimal?> dic)
        {
            Dictionary<string, int?> IAQIDic = GetDayIAQIDic(dic);
            return GetAQIResult(IAQIDic);
        }

        /// <summary>
        /// 计算小时空气质量指数结果
        /// </summary>
        /// <param name="IAQIDic">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>小时空气质量指数结果</returns>
        public static AQIResult GetHourAQIResult(IAQMData data)
        {
            Dictionary<string, int?> IAQIDic = GetHourIAQIDic(data);
            return GetAQIResult(IAQIDic);
        }

        /// <summary>
        /// 计算日均空气质量指数结果
        /// </summary>
        /// <param name="IAQIDic">空气质量基本评价项目浓度值数据接口</param>
        /// <returns>日均空气质量指数结果</returns>
        public static AQIResult GetDayAQIResult(IAQMData data)
        {
            Dictionary<string, int?> IAQIDic = GetDayIAQIDic(data);
            return GetAQIResult(IAQIDic);
        }

        /// <summary>
        /// 计算空气质量指数详细结果
        /// </summary>
        /// <param name="IAQIDic">空气质量分指数字典</param>
        /// <returns>空气质量指数详细结果</returns>
        public static AQIResultDetail GetAQIResultDetail(Dictionary<string, int?> IAQIDic)
        {
            AQIResultDetail result = new AQIResultDetail();
            CalculateAQI(result, IAQIDic);
            CalculateAQIAbout(result);
            return result;
        }

        /// <summary>
        /// 计算小时空气质量指数
        /// </summary>
        /// <param name="calculate">空气质量指数计算接口</param>
        public static void CalculateHourAQI(IAQICalculate calculate)
        {
            Dictionary<string, int?> IAQIDic = GetHourIAQIDic(calculate);
            CalculateAQI(calculate, IAQIDic);
            CalculateAQIAbout(calculate);
        }

        /// <summary>
        /// 计算日均空气质量指数
        /// </summary>
        /// <param name="calculate">空气质量指数计算接口</param>
        public static void CalculateDayAQI(IAQICalculate calculate)
        {
            Dictionary<string, int?> IAQIDic = GetDayIAQIDic(calculate);
            CalculateAQI(calculate, IAQIDic);
            CalculateAQIAbout(calculate);
        }

        /// <summary>
        /// 计算空气质量指数实时报
        /// </summary>
        /// <param name="calculate">空气质量指数报表接口</param>
        public static void CalculateHourAQI(IAQIReport report)
        {
            Dictionary<string, int?> IAQIDic = GetHourIAQIDic(report);
            CalculateIAQI(report, IAQIDic);
            CalculateAQI(report, IAQIDic);
            CalculateAQIAbout(report);
        }

        /// <summary>
        /// 计算空气质量指数日报
        /// </summary>
        /// <param name="calculate">空气质量指数报表接口</param>
        public static void CalculateDayAQI(IAQIReport report)
        {
            Dictionary<string, int?> IAQIDic = GetDayIAQIDic(report);
            CalculateIAQI(report, IAQIDic);
            CalculateAQI(report, IAQIDic);
            CalculateAQIAbout(report);
        }

        /// <summary>
        /// 计算小时空气质量分指数
        /// </summary>
        /// <param name="pollutant">污染物监测项，命名同IAQMData</param>
        /// <param name="value">浓度值</param>
        /// <returns>小时空气质量分指数</returns>
        public static int? GetHourIAQI(string pollutant, decimal? value)
        {
            return GetIAQI(value, hourConcentrationLimitsDic[pollutant]);
        }

        /// <summary>
        /// 计算日均空气质量分指数
        /// </summary>
        /// <param name="pollutant">污染物监测项，命名同IAQMData</param>
        /// <param name="value">浓度值</param>
        /// <returns>日均空气质量分指数</returns>
        public static int? GetDayIAQI(string pollutant, decimal? value)
        {
            return GetIAQI(value, dayConcentrationLimitsDic[pollutant]);
        }
        #endregion
    }
}

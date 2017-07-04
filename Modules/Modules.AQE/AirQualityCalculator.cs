using Modules.FastReflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE
{
    /// <summary>
    /// 空气质量计算器基类
    /// </summary>
    public class AirQualityCalculator
    {
        /// <summary>
        /// 空气质量监测基本项目浓度值/百分位数数据接口属性字典
        /// </summary>
        public Dictionary<string, IPropertyAccessor> IAirQualityPropertyAccessorDic { get; }
        /// <summary>
        /// 空气质量分指数/单项指数数据接口属性字典
        /// </summary>
        public Dictionary<string, IPropertyAccessor> IIAQIPropertyAccessorDic { get; }

        /// <summary>
        /// 空气质量计算器基类构造函数
        /// </summary>
        public AirQualityCalculator()
        {
            PropertyAccessorFactory factory = new PropertyAccessorFactory();
            PropertyInfo[] IAirQualityProperties = typeof(IAirQuality).GetProperties();
            IAirQualityPropertyAccessorDic = new Dictionary<string, IPropertyAccessor>();
            foreach (PropertyInfo property in IAirQualityProperties)
            {
                IAirQualityPropertyAccessorDic.Add(property.Name, factory.Get(property));
            }
            PropertyInfo[] IIAQIProperties = typeof(IIAQI).GetProperties();
            IIAQIPropertyAccessorDic = new Dictionary<string, IPropertyAccessor>();
            foreach (PropertyInfo property in IIAQIProperties)
            {
                IIAQIPropertyAccessorDic.Add(property.Name.Remove(0, 1), factory.Get(property));
            }
        }

        /// <summary>
        /// 赋值空气质量分指数/单项指数
        /// </summary>
        /// <param name="IAQIData">空气质量分指数/单项指数数据接口</param>
        /// <param name="IAQIDic">空气质量分指数/单项指数字典</param>
        protected virtual void CalculateIAQI(IIAQI IAQIData, Dictionary<string, double?> IAQIDic)
        {
            foreach (var item in IAQIDic)
            {
                if (item.Value.HasValue)
                {
                    IIAQIPropertyAccessorDic[item.Key].SetValue(IAQIData, item.Value);
                }
            }
        }
    }
}

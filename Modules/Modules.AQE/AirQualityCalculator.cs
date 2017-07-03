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
    /// 空气质量计算器
    /// </summary>
    public class AirQualityCalculator
    {
        /// <summary>
        /// 空气质量监测基本项目浓度值/百分位数数据接口属性字典
        /// </summary>
        public Dictionary<string, IPropertyAccessor> IAirQualityPropertyAccessors { get; }

        public AirQualityCalculator()
        {
            PropertyAccessorFactory factory = new PropertyAccessorFactory();
            PropertyInfo[] IAirQualityProperties = typeof(IAirQuality).GetProperties();
            IAirQualityPropertyAccessors = new Dictionary<string, IPropertyAccessor>();
            foreach (PropertyInfo property in IAirQualityProperties)
            {
                IAirQualityPropertyAccessors.Add(property.Name, factory.Get(property));
            }
        }
    }
}

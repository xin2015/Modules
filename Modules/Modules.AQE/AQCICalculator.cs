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
    /// 空气质量综合指数计算器
    /// </summary>
    public class AQCICalculator
    {
        #region 字段参数
        /// <summary>
        /// IAQMData属性
        /// </summary>
        private static IPropertyAccessor[] IAQMDataProperties;
        /// <summary>
        /// ISAQIData属性字典
        /// </summary>
        private static Dictionary<string, IPropertyAccessor> ISAQIDataPropertiesDic;
        /// <summary>
        /// 污染物监测项浓度二级标准字典
        /// </summary>
        private static Dictionary<string, int> limitDic;
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
            IAQMDataProperties = typeof(IAQMData).GetProperties().Select(o => factory.Get(o)).ToArray();
            PropertyInfo[] ISAQIDataProperties = typeof(ISAQIData).GetProperties();
            ISAQIDataPropertiesDic = new Dictionary<string, IPropertyAccessor>();
            foreach (string pollutant in limitDic.Keys)
            {
                ISAQIDataPropertiesDic.Add(pollutant, factory.Get(ISAQIDataProperties.First(o => o.Name == string.Format("I{0}", pollutant))));
            }
        }

        #region 私有方法

        #endregion
        #region 公共方法
        /// <summary>
        /// 计算空气质量单项指数
        /// </summary>
        /// <param name="pollutant">污染物监测项，命名同IAQMData</param>
        /// <param name="value">浓度值</param>
        /// <returns></returns>
        public static decimal? GetIAQI(string pollutant, decimal? value)
        {
            if (value.HasValue && value >= 0)
            {
                return Math.Round(value.Value / limitDic[pollutant], 2);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}

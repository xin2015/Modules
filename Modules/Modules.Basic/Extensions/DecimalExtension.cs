using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Basic.Extensions
{
    /// <summary>
    /// decimal扩展类
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// 可空decimal的保留小数位
        /// </summary>
        /// <param name="d">可空decimal</param>
        /// <returns></returns>
        public static decimal? Round(this decimal? d)
        {
            if (d.HasValue) return decimal.Round(d.Value);
            else return null;
        }

        /// <summary>
        /// 可空decimal的保留小数位
        /// </summary>
        /// <param name="d">可空decimal</param>
        /// <param name="decimals">保留小数位数</param>
        /// <returns></returns>
        public static decimal? Round(this decimal? d, int decimals)
        {
            if (d.HasValue) return decimal.Round(d.Value, decimals);
            else return null;
        }

        /// <summary>
        /// 可空decimal的保留小数位
        /// </summary>
        /// <param name="d">可空decimal</param>
        /// <param name="decimals">保留小数位数</param>
        /// <param name="mode">一个值，指定当 d 正好处于另两个数字中间时如何舍入。</param>
        /// <returns></returns>
        public static decimal? Round(this decimal? d, int decimals, MidpointRounding mode)
        {
            if (d.HasValue) return decimal.Round(d.Value, decimals, mode);
            else return null;
        }
    }
}

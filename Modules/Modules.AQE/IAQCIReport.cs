using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.AQE
{
    /// <summary>
    /// 空气质量综合指数报表接口
    /// </summary>
    public interface IAQCIReport : IAQCICalculate, ISAQIData
    {
    }
}

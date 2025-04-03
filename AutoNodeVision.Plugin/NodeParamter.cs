using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNodeVision.Plugin
{
    public class NodeParamter
    {
        /// <summary>
        /// 图像或区域参数
        /// </summary>
        public HObject HobjectParamter { get; set; }


        public HTuple[] HTupleListParamter { get; set; }

        /// <summary>
        /// ROI区域参数
        /// </summary>

        public HObject ROIRegionParamter { get; set; }
    }
}

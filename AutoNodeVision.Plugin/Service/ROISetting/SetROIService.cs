using AutoNodeVision.Plugin.Dialog.ROI工具箱;
using AutoNodeVision.Plugin.PluginAttribute;
using AutoNodeVision.Plugin.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoNodeVision.Plugin.Service.ROISetting
{
    [Category("ROI工具箱")]
    [DisplayName("ROI区域划定")]
    [Icon("ArrangeSendBackward")]
    public class SetROIService : BasePluginService
    {
        public override void ShowWindow(FlowNodeView nodeView)
        {
            Window window=new SetROIView(nodeView);
            window.ShowDialog();
        }
    }
}

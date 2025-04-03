using AutoNodeVision.Plugin.Dialog.形态学处理;
using AutoNodeVision.Plugin.PluginAttribute;
using AutoNodeVision.Plugin.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoNodeVision.Plugin.Service.Morphology
{
    [Category("形态学处理")]
    [DisplayName("转为灰度图")]
    [Icon("Shape")]
    public class ToGrayImage:BasePluginService
    {
        public override void ShowWindow(FlowNodeView nodeView)
        {
            Window window = new ToGrayView(nodeView);
            window.ShowDialog();
        }
    }
}

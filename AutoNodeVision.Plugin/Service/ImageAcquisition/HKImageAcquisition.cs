using AutoNodeVision.Plugin.Dialog.图像采集;
using AutoNodeVision.Plugin.PluginAttribute;
using AutoNodeVision.Plugin.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoNodeVision.Plugin.Service.ImageAcquisition
{

    [Category("相机采集")]
    [DisplayName("海康相机采集")]
    [Icon("CameraMarker")]
    public class HKImageAcquisition:BasePluginService
    {
       
        public HKImageAcquisition()
        {
            
            
        }
      
        public override void ShowWindow(FlowNodeView node)
        {
            Window window = new HKImageCollectView(node);
            window.ShowDialog();
        }
    }
}

using AutoNodeVision.Plugin.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoNodeVision.Models
{
    /// <summary>
    /// 视觉工具模型
    /// </summary>
    public class VisionToolModel : BindableBase
    {
        private string name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private string icon;

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                RaisePropertyChanged();
            }
        }

       /// <summary>
       /// 插件服务
       /// </summary>
       public BasePluginService PluginService { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public ObservableCollection<VisionToolModel> Children { get; set; }
    }
}

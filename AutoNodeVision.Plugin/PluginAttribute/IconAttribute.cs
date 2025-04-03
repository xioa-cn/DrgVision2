using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNodeVision.Plugin.PluginAttribute
{
    /// <summary>
    /// 插件图标属性
    /// </summary>
    public class IconAttribute:Attribute
    {
        public string Icon { get; set; }
        public IconAttribute(string icon)
        {
            Icon = icon;
        }
    }
}

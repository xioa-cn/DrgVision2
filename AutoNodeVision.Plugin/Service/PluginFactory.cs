using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoNodeVision.Plugin.Dialog.图像采集;
using AutoNodeVision.Plugin.Service.ImageAcquisition;
using System.ComponentModel;

namespace AutoNodeVision.Plugin.Service
{
    /// <summary>
    /// 插件工厂
    /// </summary>
    public class PluginFactory
    {
        public static BasePluginService GetPlugin(string pluginName)
        {
            //通过反射拿到对应的插件服务
            var assembly = Assembly.GetExecutingAssembly();

            //找到所有BasePluginService和IPlugin的子类且这个Type不是接口
            var typeList = assembly
                .GetTypes()
                .Where(t =>
                    t.IsSubclassOf(typeof(BasePluginService)) && typeof(IPlugin).IsAssignableFrom(t)
                    &&!t.IsInterface
                );

            //找到对应插件名(通过DisplayNameAttribute)的插件
            var type = typeList.FirstOrDefault(t => t.GetCustomAttribute<DisplayNameAttribute>().DisplayName == pluginName);
           
            //通过反射创建插件实例
            return (BasePluginService)Activator.CreateInstance(type);
          
        }
    }
}

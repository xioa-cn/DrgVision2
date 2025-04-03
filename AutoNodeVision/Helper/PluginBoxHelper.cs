using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoNodeVision.Models;
using AutoNodeVision.Plugin.PluginAttribute;
using AutoNodeVision.Plugin.Service;

namespace AutoNodeVision.Helper
{
    /// <summary>
    /// 工具箱帮助类
    /// </summary>
    public class PluginBoxHelper
    {
        /// <summary>
        /// 设置工具箱
        /// </summary>
        /// <param name="VisoToolList"></param>
        public void SetVisonToolBox(ObservableCollection<VisionToolModel> VisoToolList)
        {
            //反射加载所有插件类
            Assembly assembly = Assembly.Load("AutoNodeVision.Plugin");
            Type[] types = assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BasePluginService)))
                .ToArray();

            //拿到所有工具箱的名称 Distinct的作用是去除重复的字符串
            List<string> ItemTotalName = types
                .Select(t => t.GetCustomAttribute<CategoryAttribute>().Category).Distinct()
                .ToList();
             foreach(var itemTotal in ItemTotalName)
                {
                    VisionToolModel model= new VisionToolModel() { Name= itemTotal ,Icon= "ToolboxOutline" };
                    model.Children = new ObservableCollection<VisionToolModel>();
                    foreach(var item in types)
                    {
                       string ItemBlongName= item.GetCustomAttribute<CategoryAttribute>().Category;
                       if(ItemBlongName==itemTotal)
                        {
                            //拿到工具箱的名称和图标
                            string name = item.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            string icon = item.GetCustomAttribute<IconAttribute>().Icon;
                            
                            //添加到工具箱子项
                            model.Children.Add(new VisionToolModel() { Name = name, Icon = icon,PluginService= (BasePluginService)Activator.CreateInstance(item) });
                        }
                       
                    }
                    VisoToolList.Add(model);//添加工具箱子项
                }
        }
    }
}

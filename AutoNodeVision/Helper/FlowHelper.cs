using AutoNodeVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using AutoNodeVision.Plugin.Views;
using AutoNodeVision.ViewModels;
using System.Collections.ObjectModel;
using AutoNodeVision.Views;
using HalconDotNet;

namespace AutoNodeVision.Helper
{
    /// <summary>
    /// 流程帮助类
    /// </summary>
    public class FlowHelper:BindableBase
    {

        public FlowHelper()
        {
            LogList= new ObservableCollection<LogModel>();
        }

        private ObservableCollection<LogModel> logList;

        /// <summary>
        /// 日志列表
        /// </summary>
        public ObservableCollection<LogModel> LogList
        {
            get { return logList; }
            set { logList = value; RaisePropertyChanged(); }
        }


        public void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisionToolModel model = (sender as StackPanel).DataContext as VisionToolModel;
            if (model.Children == null)
            {
                DragDrop.DoDragDrop((sender as StackPanel), model, DragDropEffects.Move);
            }
        }
        public void Canvas_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(VisionToolModel)) as VisionToolModel;
           FlowNodeView flowNodeView = new FlowNodeView() 
                   { NodeName = data.Name,PluginService=data.PluginService };
            var canvas = sender as Canvas;
            Point p = e.GetPosition(canvas);
            Canvas.SetLeft(flowNodeView, p.X);
            Canvas.SetTop(flowNodeView, p.Y);
            canvas.Children.Add(flowNodeView);

            //添加日志     
            flowNodeView.AddLogAction += (message) =>
                {
                    string time= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    LogList.Add(new LogModel
                    {
                        Time = time,
                        Message = message
                    });
                };



            //显示视图
            flowNodeView.ShowParamterAction += (paramter) =>
            {
                var mainView = GetParentWindow(sender as DependencyObject);
                HOperatorSet.SetColor(mainView.hsmart.HalconWindow, "red");
                if (paramter.HobjectParamter!=null)
                {
                    
                    mainView.hsmart.HalconWindow.DispObj(paramter.HobjectParamter);
                }
                if(paramter.ROIRegionParamter!=null)
                {
                    HOperatorSet.SetDraw(mainView.hsmart.HalconWindow, "margin");
                    mainView.hsmart.HalconWindow.DispObj(paramter.ROIRegionParamter);
                }
                
            };
           

        }

       
        private MainView GetParentWindow(DependencyObject obj)
        {
            obj= VisualTreeHelper.GetParent(obj);
            if(obj is Window)
            {
                return obj as MainView;
            }
            else
            {
               return  GetParentWindow(obj);
            }
        }
    }
}

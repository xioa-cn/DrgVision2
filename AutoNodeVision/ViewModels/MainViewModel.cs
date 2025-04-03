using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutoNodeVision.Helper;
using AutoNodeVision.Models;

namespace AutoNodeVision.ViewModels
{
    /// <summary>
    /// 主视图模型
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region 字段和属性
        private IDialogService dialogService;

        public FlowHelper FlowHelper { get; set; } = new FlowHelper();

        private ObservableCollection<VisionToolModel> visionToolList;

        /// <summary>
        /// 视觉工具列表
        /// </summary>
        public ObservableCollection<VisionToolModel> VisionToolList
        {
            get { return visionToolList; }
            set
            {
                visionToolList = value;
                RaisePropertyChanged();
            }
        }

        private string systemTime;

        /// <summary>
        /// 系统时间
        /// </summary>
        public string SystemTime
        {
            get { return systemTime; }
            set
            {
                systemTime = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<TabItem> viewTabItemList;

        /// <summary>
        /// 视图选项卡列表
        /// </summary>
        public ObservableCollection<TabItem> ViewTabItemList
        {
            get { return viewTabItemList; }
            set
            {
                viewTabItemList = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<TabItem> nodeTabItemList;

        /// <summary>
        /// 节点选项卡列表
        /// </summary>
        public ObservableCollection<TabItem> NodeTabItemList
        {
            get { return nodeTabItemList; }
            set
            {
                nodeTabItemList = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<TabItem> logItemList;

        /// <summary>
        /// 日志选项卡列表
        /// </summary>
        public ObservableCollection<TabItem> LogItemList
        {
            get { return logItemList; }
            set
            {
                logItemList = value;
                RaisePropertyChanged();
            }
        }

        


        #endregion

        #region 命令

        /// <summary>
        /// 添加节点命令
        /// </summary>
        public DelegateCommand AaddFlowCommand { get; set; }

        public DelegateCommand<TabItem> RemoveItemCommand { get; set; }

       

        #endregion


        #region 构造方法
        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            //设置系统时间
            SetSystemTime();

            //初始化视觉工具箱菜单
            InitVisionToolList();

            //添加视图选项卡
            ViewTabItemList = new ObservableCollection<TabItem>();
            NodeTabItemList = new ObservableCollection<TabItem>();
            LogItemList = new ObservableCollection<TabItem>();
           
            AaddFlowCommand = new DelegateCommand(AddFlow);
            RemoveItemCommand = new DelegateCommand<TabItem>(RemoveItem);
          
            
        }

       
        #endregion




        #region 方法
        /// <summary>
        /// 设置系统时间
        /// </summary>
        private void SetSystemTime()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    SystemTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            });
        }

        /// <summary>
        /// 初始化视觉工具箱菜单
        /// </summary>
        private void InitVisionToolList()
        {
            VisionToolList = new ObservableCollection<VisionToolModel>();

            new PluginBoxHelper().SetVisonToolBox(visionToolList);
        }

        private void AddFlow()
        {
            dialogService.ShowDialog(
                "AddFlowDialogView",
                callback =>
                {
                    if (callback.Result == ButtonResult.OK)
                    {
                        FlowModel model = callback.Parameters.GetValue<FlowModel>("flowParam");
                        ViewTabItemList.Add(new TabItem() { Header = model.Name, Content = null, });

                        var headerTemplate = (DataTemplate)
                            new TabControl().FindResource("FlowTabItemHeader");
                        NodeTabItemList.Add(
                            new TabItem()
                            {
                                Header = model.Name,
                                HeaderTemplate = headerTemplate,
                                Content = new ContentControl()
                                {
                                    Template = (ControlTemplate)
                                        new ContentControl().FindResource(
                                            "FlowTabItemContentTemplate"
                                        )
                                }
                            }
                        );

                        var headerTemplate2 = (DataTemplate)
                            new TabControl().FindResource("FlowTabItemHeaderNoRemoveButton");
                        LogItemList.Add(
                            new TabItem()
                            {
                                Header = model.Name,
                                HeaderTemplate = headerTemplate2,
                                
                            }
                        );

                        
                    }
                }
            );
        }

        private void RemoveItem(TabItem tabItem)
        {
            if (tabItem == null)
                return;
            var tabControl = ItemsControl.ItemsControlFromItemContainer(tabItem) as TabControl; //通过子项找到父容器
            var source = tabControl.ItemsSource as ObservableCollection<TabItem>; //找到父容器的数据源
            if (tabControl.SelectedItem == tabItem)
                source?.Remove(tabItem); //若删除项为选中项则从数据源中移除
        }


       
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoNodeVision.Plugin.Dialog.图像采集;
using AutoNodeVision.Plugin.Service;

namespace AutoNodeVision.Plugin.Views
{
    /// <summary>
    /// FlowNodeView.xaml 的交互逻辑
    /// </summary>
    public partial class FlowNodeView : UserControl
    {
        public FlowNodeView()
        {
            InitializeComponent();

            //结点的双击事件
            this.MouseDoubleClick += FlowNodeView_MouseDoubleClick;

            
        }

        /// <summary>
        /// 结点的输入参数
        /// </summary>
        public NodeParamter InputParamter { get; set; } = new NodeParamter();

        /// <summary>
        /// 结点的输出参数
        /// </summary>
        public NodeParamter OutputParamter { get; set; } = new NodeParamter();

        public Action<NodeParamter> ShowParamterAction { get; set; }

        public  Action<string> AddLogAction { get; set; }
        #region 依赖属性

        #region 结点名称
        public string NodeName
        {
            get { return (string)GetValue(NodeNameProperty); }
            set { SetValue(NodeNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NodeName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeNameProperty = DependencyProperty.Register(
            "NodeName",
            typeof(string),
            typeof(FlowNodeView),
            new PropertyMetadata("", OnNodeNameChanged)
        );

        private static void OnNodeNameChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            FlowNodeView flowNodeView = d as FlowNodeView;
            if (flowNodeView != null)
            {
                flowNodeView.SetNodeName(e.NewValue.ToString());
            }
        }

        private void SetNodeName(string name)
        {
            textBlock.Text = name;
        }

        #endregion




        #region 连接点是否可见

        public bool IsDrawingConnectLine
        {
            get { return (bool)GetValue(IsDrawingConnectLineProperty); }
            set { SetValue(IsDrawingConnectLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDrawingConnectLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDrawingConnectLineProperty =
            DependencyProperty.Register(
                "IsDrawingConnectLine",
                typeof(bool),
                typeof(FlowNodeView),
                new PropertyMetadata(false, OnIsDrawingConnectLineChanged)
            );

        private static void OnIsDrawingConnectLineChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            FlowNodeView flowNodeView = d as FlowNodeView;
            if (flowNodeView != null)
            {
                flowNodeView.SetConnectorShowing((bool)e.NewValue);
            }
        }

        private void SetConnectorShowing(bool isDrawing)
        {
            if (isDrawing)
            {
                ConnectorControls.Visibility = Visibility.Visible;
            }
            else
            {
                ConnectorControls.Visibility = Visibility.Collapsed;
            }
        }
        #endregion


        #region  添加日志的命令


       



        #endregion

        #endregion


        #region 结点拖动相关

        double xGap = 0;
        double yGap = 0;

        private void thumb_DragStarted(object sender, DragStartedEventArgs e) { }

        /// <summary>
        /// 节点拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void thumb_DragDelta(
            object sender,
            System.Windows.Controls.Primitives.DragDeltaEventArgs e
        )
        {
            var thumb = sender as Thumb;
            var canvas = GetParentCanvas(thumb);
            //var flowNode= GetParentFlowNode(thumb);

            //移动当前流程节点
            double x = Canvas.GetLeft(this) + e.HorizontalChange;
            double y = Canvas.GetTop(this) + e.VerticalChange;
            double xmax = canvas.ActualWidth - this.ActualWidth;
            double ymax = canvas.ActualHeight - this.ActualHeight;
            if (x >= 0 && x <= xmax)
                Canvas.SetLeft(this, x);
            if (y >= 0 && y <= ymax)
                Canvas.SetTop(this, y);

            //改变所有连接点中连接路径的位置
            ChangeAllConnectPath();
        }

        /// <summary>
        /// 改变所有连接点的连接路径位置
        /// </summary>
        private void ChangeAllConnectPath()
        {
            ChangeConnectPathByDirection(LeftConnector);
            ChangeConnectPathByDirection(RightConnector);
            ChangeConnectPathByDirection(TopConnector);
            ChangeConnectPathByDirection(BottomConnector);
        }

        //根据当前连接点的布局方向改变连接的路径位置
        private void ChangeConnectPathByDirection(Connector connector)
        {
            switch (connector.Direction)
            {
                case ConnectDirection.Left:
                    
                    {
                        double x = Canvas.GetLeft(this);
                        double y = Canvas.GetTop(this) + this.ActualHeight / 2;
                        RelinkPath(connector, x, y);
                    }
                    break;
                case ConnectDirection.Right:
                    {
                        double x = Canvas.GetLeft(this) + this.ActualWidth;
                        double y = Canvas.GetTop(this) + this.ActualHeight / 2;
                        RelinkPath(connector, x, y);
                    }
                    break;
                case ConnectDirection.Top:
                   { 
                        double x = Canvas.GetLeft(this) + this.ActualWidth / 2;
                        double y = Canvas.GetTop(this);
                        RelinkPath(connector,x, y);
                   }
                    break;
                case ConnectDirection.Bottom:
                    {
                        double x = Canvas.GetLeft(this) + this.ActualWidth / 2;
                        double y = Canvas.GetTop(this) + this.ActualHeight;
                        RelinkPath(connector, x, y);
                    }
                    break;
            }
        }

      /// <summary>
      /// 重置连接点生成新的路径
      /// </summary>
      /// <param name="connector">当前连接点</param>
      /// <param name="x">当前连接点的x坐标</param>
      /// <param name="y">当前连接点的y坐标</param>
        private void RelinkPath(Connector connector,double x,double y)
        {
            if (connector.InputPathInfo != null)
            {
                connector.InputPathInfo.EndPoint = new Point(x, y);
                connector.InputPathInfo.SetNewPath();
            }
            if (connector.OutputPathList.Count > 0)
            {
                foreach (var pathInfo in connector.OutputPathList)
                {
                    var nextNode = pathInfo.NextNode;
                    if (nextNode != null)
                    {

                        pathInfo.StartPoint = new Point(x, y);
                        pathInfo.SetNewPath();
                    }
                }

            }
        }



        /// <summary>
        /// 找到父级Canvas
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Canvas GetParentCanvas(DependencyObject obj)
        {
            var parent = VisualTreeHelper.GetParent(obj);
            if (parent == null)
                return null;
            else if (parent is Canvas)
                return parent as Canvas;
            else
                return GetParentCanvas(parent);
        }

        /// <summary>
        /// 找到父级FlowNodeView
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private FlowNodeView GetParentFlowNode(DependencyObject obj)
        {
            var parent = VisualTreeHelper.GetParent(obj);
            if (parent == null)
                return null;
            else if (parent is FlowNodeView)
                return parent as FlowNodeView;
            else
                return GetParentFlowNode(parent);
        }

        #endregion


        #region 对应的插件服务属性
        /// <summary>
        /// 对应的插件服务
        /// </summary>
        public BasePluginService PluginService { get; set; }
        #endregion


        #region 节点关联属性
        /// <summary>
        /// 左侧连接点
        /// </summary>
        public Connector LeftConnector { get; set; }

        /// <summary>
        /// 右侧连接点
        /// </summary>
        public Connector RightConnector { get; set; }

        /// <summary>
        /// 顶部连接点
        /// </summary>
        public Connector TopConnector { get; set; }

        /// <summary>
        /// 底部连接点
        /// </summary>
        public Connector BottomConnector { get; set; }

        #endregion



        #region  连接点的显示与隐藏

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            double x = Canvas.GetLeft(this);
            ConnectorControls.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsDrawingConnectLine)
            {
                ConnectorControls.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region 删除结点
        /// <summary>
        /// 删除结点,将当前的节点从父级Canvas中移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            //获取父级Canvas
            var canvas = GetParentCanvas(this);

            //删除当前节点上所有连接点的连接线
            RemovaAllPath(canvas, this.LeftConnector);
            RemovaAllPath(canvas, this.RightConnector);
            RemovaAllPath(canvas, this.TopConnector);
            RemovaAllPath(canvas, this.BottomConnector);

            //删除当前节点
            canvas.Children.Remove(this);
        }

        private void RemovaAllPath(Canvas canvas, Connector connector)
        {
            if (connector.InputPathInfo != null)
            {
                canvas.Children.Remove(connector.InputPathInfo.Path);
                connector.InputPathInfo = null;
            }
            if (connector.OutputPathList.Count > 0)
            {
                foreach (var item in connector.OutputPathList)
                {
                    canvas.Children.Remove(item.Path);
                    connector.OutputPathList.Clear();
                }
            }
        }

        #endregion

        #region 结点的双击事件

        /// <summary>
        /// 获取连接点的输入参数
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        private NodeParamter GetInputParamterByConnector(Connector connector )
        {
            if (connector.InputPathInfo.PreNode != null)
            {
                return connector.InputPathInfo.PreNode.OutputParamter;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前节点的输入参数
        /// </summary>
        private void GetInputParamter()
        {
            this.InputParamter =(InputParamter==null)?GetInputParamterByConnector(this.LeftConnector):InputParamter;
            this.InputParamter = (InputParamter == null) ? GetInputParamterByConnector(this.RightConnector) : InputParamter;
            this.InputParamter = (InputParamter == null) ? GetInputParamterByConnector(this.TopConnector) : InputParamter;
            this.InputParamter = (InputParamter == null) ? GetInputParamterByConnector(this.BottomConnector) : InputParamter;
        } 

        /// <summary>
        /// 设置后续连接节点的输出参数
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="paramter"></param>
        private void SetOuputConnectorNodeParamter(Connector connector,NodeParamter paramter)
        {
           
            if(connector.OutputPathList.Count>0)
            {
                foreach (var item in connector.OutputPathList)
                {
                    if(item.NextNode!=null)
                    {
                        item.NextNode.InputParamter = paramter;
                    }
                }
            }
        }
        
        /// <summary>
        /// 输出当前节点的输出参数
        /// </summary>
        private void SetOutputParamterToNextNode()
        {
            SetOuputConnectorNodeParamter(LeftConnector, this.OutputParamter);
            SetOuputConnectorNodeParamter(RightConnector, this.OutputParamter);
            SetOuputConnectorNodeParamter(TopConnector, this.OutputParamter);
            SetOuputConnectorNodeParamter(BottomConnector, this.OutputParamter);
        }

        /// <summary>
        /// 执行节点编辑操作
        /// </summary>
        private void ExcuteNode()
        {
            GetInputParamter();//获取当前节点的输入参数

            var pluginService = PluginFactory.GetPlugin(NodeName);//获取视觉对应插件服务
            pluginService.ShowWindow(this);//弹出对应插件窗口


            SetOutputParamterToNextNode();//设置后续节点的输入参数

            //设置节点颜色,若输出参数若输出参数不为空则设置为绿色,否则为蓝色
            if (this.OutputParamter.HobjectParamter != null || this.OutputParamter.HTupleListParamter != null)
            {

                Color color = (Color)ColorConverter.ConvertFromString("#228B22");
                nodeBorder.Background = new SolidColorBrush(color);

                //触发添加日志的命令
              string logMessage= $"{NodeName}执行成功";
              AddLogAction?.Invoke(logMessage);
              ShowParamterAction?.Invoke(this.OutputParamter);
              
                
            }
            else
            {
                Color color = (Color)ColorConverter.ConvertFromString("#8ED0F3");
                nodeBorder.Background = new SolidColorBrush(color);
            }
        }


        /// <summary>
        /// 双击节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlowNodeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExcuteNode();//执行节点编辑操作
        }

        #endregion

        #region 结点编辑事件
        private void EditNode_Click(object sender, RoutedEventArgs e)
        {
            ExcuteNode();//执行节点编辑操作
        }
        #endregion
    }
}

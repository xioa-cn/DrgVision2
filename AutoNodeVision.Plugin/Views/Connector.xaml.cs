using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoNodeVision.Plugin.Views
{
    public enum ConnectDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }

    /// <summary>
    /// Connector.xaml 的交互逻辑
    /// </summary>
    public partial class Connector : UserControl
    {
        public Connector()
        {
            
            InitializeComponent();
            this.MouseMove += Connector_MouseMove;
            this.MouseUp += Connector_Up;
            this.Loaded += Connector_Loaded;
 
        }

       




        #region 依赖属性



        public ConnectDirection Direction
        {
            get { return (ConnectDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(ConnectDirection), typeof(Connector), new PropertyMetadata(ConnectDirection.Top));



        #endregion




       public FlowNodeView ParentNode { get; set; }//父节点

       /// <summary>
       /// 节点输入连接线信息
       /// </summary>
       public ConnectPathInfo InputPathInfo { get; set; }= new ConnectPathInfo();

      
      // public ConnectPathInfo OutputPathInfo { get; set; }= new ConnectPathInfo();


       /// <summary>
       /// 节点输出连接线信息
       /// </summary>
       public List<ConnectPathInfo> OutputPathList { get; set; } = new List<ConnectPathInfo>();

        #region 事件方法




        private void Connector_Loaded(object sender, RoutedEventArgs e)
        {
            var flowNode = GetParentFlowNodeView(this);
            ParentNode = flowNode;
            switch (Direction)
            {
                case ConnectDirection.Left:
                    flowNode.LeftConnector = this;
                    break;
                case ConnectDirection.Right:
                    flowNode.RightConnector = this;
                    break;
                case ConnectDirection.Top:
                    flowNode.TopConnector = this;
                    break;
                case ConnectDirection.Bottom:
                    flowNode.BottomConnector = this;
                    break;
                default:
                    break;
            }
        }

       

        // 连接点控件的鼠标移动事件处理器（用于实现拖拽创建连接线）
        private void Connector_MouseMove(object sender, MouseEventArgs e)
        {
            // 当检测到鼠标左键处于按下状态时（拖拽开始）
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // 获取当前连接点所属的流程节点视图（通过可视化树向上查找）
                var flowNode = GetParentFlowNodeView(this);
                ParentNode = flowNode; // 缓存父节点引用

                // 查找承载节点的画布容器（通过可视化树向上遍历）
                var canvas = GetParentCanvas(this);
                if (canvas != null)
                {
                    // 获取画布上的装饰层（AdornerLayer），用于显示临时图形元素[6,7](@ref)
                    var layer = AdornerLayer.GetAdornerLayer(canvas);

                    // 创建连接线装饰器实例，参数为画布和当前连接点控件
                    var adorner = new ConnectAdorner(canvas, this);
                    adorner.StartNode = flowNode; // 设置连接起始节点

                    // 将装饰器添加到装饰层，此时会触发装饰器的渲染逻辑[8](@ref)
                    layer.Add(adorner);

                    // 标记事件已处理，阻止事件继续冒泡
                    e.Handled = true;
                }
            }
        }

        private void Connector_Up(object sender, MouseButtonEventArgs e)
        {
            var flowNode = GetParentFlowNodeView(this);
            flowNode.IsDrawingConnectLine = false;
           
        }


        /// <summary>
        /// 获取当前连接器的父节点容器Canvas类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        private Canvas GetParentCanvas(DependencyObject obj)
        {
            var parent= VisualTreeHelper.GetParent(obj);
            if(parent ==null) return null;
            else if (parent is Canvas) return parent as Canvas;
            else return GetParentCanvas(parent);
        }

        /// <summary>
        /// 获取当前连接器的父节点类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private FlowNodeView GetParentFlowNodeView(DependencyObject obj)
        {
            var parent = VisualTreeHelper.GetParent(obj);
            if (parent == null) return null;
            else if (parent is FlowNodeView) return parent as FlowNodeView;
            else return GetParentFlowNodeView(parent);
        }

       


        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AutoNodeVision.Plugin.Views;

namespace AutoNodeVision.Plugin
{
    /// <summary>
    /// 节点连接装饰器
    /// </summary>
    public class ConnectAdorner : Adorner
    {
        private Connector startConnector; //起始连接点
        private Connector hitConnector; //碰撞连接点
        private Canvas canvas; //画布
        private Path path; //连接路径
        private Pen strokePen; //连接路径画笔
        public FlowNodeView StartNode { get; set; } //起始节点
        private FlowNodeView hitNode { get; set; } //碰撞节点

        public ConnectAdorner(Canvas canvas, Connector connector)
            : base(canvas)
        {
            this.canvas = canvas;
            this.startConnector = connector;
            path = new Path();
            strokePen = new Pen(Brushes.YellowGreen, 2);
            path.StrokeThickness = strokePen.Thickness;
            path.Stroke = strokePen.Brush;
            path.Fill = strokePen.Brush;
            this.Cursor = Cursors.Cross;
        }

        // 重写 OnRender 方法，用于自定义元素的绘制逻辑
        // 此方法会在元素需要进行绘制时被触发，例如元素初次显示、布局改变或视觉状态改变时
        protected override void OnRender(DrawingContext dc)
        {
            // 调用基类的 OnRender 方法，确保基类的绘制逻辑得以执行
            base.OnRender(dc);

            // 使用 DrawingContext 对象绘制几何图形
            // 第一个参数为填充画刷，这里传入 null 表示不填充
            // 第二个参数为描边画笔，用于绘制几何图形的轮廓
            // 第三个参数为要绘制的几何图形的数据
            dc.DrawGeometry(null, strokePen, path.Data);

            // 绘制一个透明的矩形，覆盖整个元素的渲染区域
            // 第一个参数为填充画刷，使用 Brushes.Transparent 表示透明填充
            // 第二个参数为描边画笔，传入 null 表示不绘制轮廓
            // 第三个参数为要绘制矩形的区域，使用 RenderSize 确定矩形的大小
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize)); //绘制一个透明的矩形
        }

        private Point startPoint;
        private Point currentPoint;

        /// <summary>
        /// 获取起始点
        /// </summary>
        /// <returns></returns>
        private Point GetPoint(FlowNodeView node, Connector connector)
        {
            double x = 0,
                y = 0;
            switch (connector.Direction)
            {
                case ConnectDirection.Left:
                    x = Canvas.GetLeft(node);
                    y = Canvas.GetTop(node) + node.ActualHeight / 2;
                    break;
                case ConnectDirection.Right:
                    x = Canvas.GetLeft(node) + node.ActualWidth;
                    y = Canvas.GetTop(node) + node.ActualHeight / 2;
                    break;
                case ConnectDirection.Top:
                    x = Canvas.GetLeft(node) + node.ActualWidth / 2;
                    y = Canvas.GetTop(node);
                    break;
                case ConnectDirection.Bottom:
                    x = Canvas.GetLeft(node) + node.ActualWidth / 2;
                    y = Canvas.GetTop(node) + node.ActualHeight;
                    break;
                default:
                    break;
            }
            return new Point(x, y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            startPoint = GetPoint(StartNode, startConnector); //获取起始点
            if (hitConnector == null)
                currentPoint = e.GetPosition(canvas); //获取当前鼠标位置
            StartNode.IsDrawingConnectLine = true;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured)
                {
                    this.CaptureMouse();
                }

                HitTesting(currentPoint);//鼠标移动过程中执行碰撞检测
                //更新路径绘制
                UpdaPath(startPoint, currentPoint, path);
                this.InvalidateVisual(); //重绘
            }
            else
            {
                if (this.IsMouseCaptured)
                {
                    this.ReleaseMouseCapture();
                }
            }
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <param name="hitPoint"></param>
        ///
        // 定义一个名为 HitTesting 的私有方法，用于执行命中测试
        // 参数 hitPoint 表示要进行命中测试的点，通常是鼠标指针所在的位置
        private void HitTesting(Point hitPoint)
        {
            // 声明一个布尔类型的变量 hitConnectorFlag，用于标记是否命中了连接器
            // 初始值设为 false，表示尚未命中连接器
            bool hitConnectorFlag = false;
            // 声明一个 Connector 类型的变量 hitConnector，用于存储命中的连接器对象
            // 初始值设为 null，表示尚未命中任何连接器
            hitConnector = null;
            // 声明一个 FlowNodeView 类型的变量 hitNode，用于存储命中的节点视图对象
            // 初始值设为 null，表示尚未命中任何节点视图
            hitNode = null;

            // 调用 canvas 的 InputHitTest 方法进行命中测试，获取鼠标碰撞到的元素
            // 将返回的对象转换为 DependencyObject 类型，并赋值给 hitObject 变量
            DependencyObject hitObject = canvas.InputHitTest(hitPoint) as DependencyObject;

            // 开始一个 while 循环，只要 hitObject 不为 null，且不是 StartNode，并且不是 Canvas 类型，就继续循环
            while (
                hitObject != null && hitObject != StartNode && hitObject.GetType() != typeof(Canvas)
            )
            {
                // 检查 hitObject 是否为 Connector 类型，并且该连接器的父节点不是 StartNode
                if (hitObject is Connector && (hitObject as Connector)?.ParentNode != StartNode)
                {
                    // 如果满足条件，将 hitObject 转换为 Connector 类型，并赋值给 hitConnector 变量
                    hitConnector = hitObject as Connector;
                    // 获取该连接器的父节点
                    var node = hitConnector.ParentNode;
                    // 调用 GetPoint 方法，根据节点和连接器获取当前点的位置，并赋值给 currentPoint 变量
                    currentPoint = GetPoint(node, hitConnector);
                    // 将 hitConnectorFlag 设为 true，表示已经命中了连接器
                    hitConnectorFlag = true;
                }

                // 检查 hitObject 是否为 FlowNodeView 类型
                if (hitObject is FlowNodeView)
                {
                    // 如果满足条件，将 hitObject 转换为 FlowNodeView 类型，并赋值给 hitNode 变量
                    hitNode = hitObject as FlowNodeView;
                    // 将命中的节点视图的 IsDrawingConnectLine 属性设为 true，使得4个连接结点可见
                    hitNode.IsDrawingConnectLine = true;
                    // 如果尚未命中连接器
                    if (!hitConnectorFlag)
                    {
                        // 将 hitConnector 设为 null，表示没有命中连接器
                        hitConnector = null;
                    }
                    // 命中节点视图后，方法返回，结束命中测试
                    return;
                }
                // 如果当前的 hitObject 既不是连接器也不是节点视图，通过 VisualTreeHelper 获取其父对象
                // 继续在父对象上进行检查，直到找到符合条件的对象或者遍历到 Canvas 或 StartNode
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }
        }

        


        /// <summary>
        /// 绘制连接路径
        /// </summary>
        /// <param name="startPoint1">起始点坐标</param>
        /// <param name="currentPoint1">当前的鼠标坐标</param>
        /// <param name="updatePath">要更新的路径</param>
        private void UpdaPath(Point startPoint1, Point currentPoint1, Path updatePath)
        {
            double radius = 2;
            string data =
                $"M{startPoint1.X},{startPoint1.Y} "
                + $"L{currentPoint1.X - radius},{currentPoint1.Y - 0} "
                + $"A{radius},{radius} 0 1 0 {currentPoint1.X + radius},{currentPoint1.Y}"
                + $"A{radius}, {radius} 0 1 0 {currentPoint1.X - radius}, {currentPoint1.Y}";
            updatePath.Data = Geometry.Parse(data);
        }

        /// <summary>
        /// 鼠标松开时，若若鼠标位置在某个连接器上，则将连接器与起始节点连接起来，并保存路径信息
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (hitConnector != null)
            {
                //保存路径信息
                ConnectPathInfo info = new ConnectPathInfo();
                info.StartPoint = startPoint;
                info.EndPoint = currentPoint;
                info.Path = path;
                info.PreNode = StartNode;
                //info.NextNode= hitConnector.ParentNode;
                info.NextNode = hitConnector.ParentNode;
                info.Path.Cursor = Cursors.Pen;

                info.NextNode.InputParamter = info.PreNode.OutputParamter;//将输入节点的输出参数作为输出节点的输入参数
                //更新起始连接结点的路径信息，并将路径绘制到画布中
                startConnector.OutputPathList.Add(info);

                hitConnector.InputPathInfo = info;

                canvas.Children.Add(info.Path);
                path.MouseRightButtonDown += Path_MouseRightButtonDown;
            }
            var layer = AdornerLayer.GetAdornerLayer(canvas);
            if (layer != null)
            {
                layer.Remove(this);
            }
            StartNode.IsDrawingConnectLine = false;
        }

        /// <summary>
        /// 鼠标右键点击时，删除路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Path_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //删除路径
            var path = sender as Path;
            canvas.Children.Remove(path);

            //删除路径连接的两个结点之间的关联信息
            startConnector.InputPathInfo.PreNode = null;
            startConnector.OutputPathList.Remove(hitConnector.InputPathInfo);
        }
    }
}

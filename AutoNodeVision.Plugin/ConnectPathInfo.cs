using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using AutoNodeVision.Plugin.Views;


namespace AutoNodeVision.Plugin
{
    /// <summary>
    /// 节点连接路径信息
    /// </summary>
    public class ConnectPathInfo
    {

        public ConnectPathInfo()
        {
           
        }

        /// <summary>
        /// 上一个节点
        /// </summary>
        public FlowNodeView PreNode { get; set; }

        /// <summary>
        /// 下一个节点
        /// </summary>
        public FlowNodeView NextNode { get; set; }

        
        //后续连接结点的集合
        //public List<FlowNodeView> NextNodeList { get; set; } = new List<FlowNodeView>();

        /// <summary>
        /// 起始点
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// 结束点
        /// </summary>
        public Point EndPoint { get; set; }


        /// <summary>
        /// 连接路径
        /// </summary>
       public Path Path { get; set; }



      /// <summary>
      /// 设置新的路径
      /// </summary>
       public void SetNewPath()
        {
            /*  string data = $"M{StartPoint.X},{StartPoint.Y} L{EndPoint.X}," +
                  $"{EndPoint.Y} L{EndPoint.X - 5},{EndPoint.Y - 5}  " + $"L{EndPoint.X},{EndPoint.Y}" +
                  $"L{EndPoint.X + 10},{EndPoint.Y - 10} ";
            Path.Data = Geometry.Parse(data);*/
            if (Path == null)
                return;

            double radius = 3;
            string data = $"M{StartPoint.X},{StartPoint.Y} " +
                         $"L{EndPoint.X - radius},{EndPoint.Y - 0} " +
                         $"A{radius},{radius} 0 1 0 {EndPoint.X + radius},{EndPoint.Y}" +
                         $"A{radius}, {radius} 0 1 0 {EndPoint.X - radius}, {EndPoint.Y}";
            Path.Fill = Brushes.LightBlue;
            Path.Data = Geometry.Parse(data);
        }



       

       
    }
}

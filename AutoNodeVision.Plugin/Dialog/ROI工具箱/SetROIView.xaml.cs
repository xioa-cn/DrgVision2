using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using AutoNodeVision.Plugin.Views;
using HalconDotNet;

namespace AutoNodeVision.Plugin.Dialog.ROI工具箱
{
    public enum ROIEnum
    {
        Circle,
        Rectangle1,
        Ellipse,
    }

    /// <summary>
    /// SetROIView.xaml 的交互逻辑
    /// </summary>
    public partial class SetROIView : Window
    {
        public SetROIView()
        {
            InitializeComponent();
        }

        public SetROIView(FlowNodeView node)
            : this()
        {
            InitializeComponent();
            this.node = node;
        }

        private FlowNodeView node;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (node.InputParamter.HobjectParamter != null)
            {
                hsmart.HalconWindow.DispObj(node.InputParamter.HobjectParamter);
                imageParamter = node.InputParamter.HobjectParamter;
            }
        }

        private HTuple[] roiParamter = null;
        private HObject regionParamter = null;
        private HObject imageParamter= null;

        /// <summary>
        /// 根据不同形状绘制ROI
        /// </summary>
        /// <param name="roiType"></param>
        /// <returns></returns>
        private async Task DrawROI(ROIEnum roiType)
        {

            HOperatorSet.SetColor(hsmart.HalconWindow, "red");
            await Task.Run(() =>
            {
               
                switch (roiType)
                {
                    case ROIEnum.Circle:
                        roiParamter = new HTuple[3];
                        HOperatorSet.DrawCircle(
                            hsmart.HalconWindow,
                            out roiParamter[0],
                            out roiParamter[1],
                            out roiParamter[2]
                        );
                        HOperatorSet.GenCircle(
                            out regionParamter,
                            roiParamter[0],
                            roiParamter[1],
                            roiParamter[2]
                        );
                        break;
                    case ROIEnum.Rectangle1:
                        roiParamter = new HTuple[4];
                        HOperatorSet.DrawRectangle1(
                            hsmart.HalconWindow,
                            out roiParamter[0],
                            out roiParamter[1],
                            out roiParamter[2],
                            out roiParamter[3]
                        );
                        HOperatorSet.GenRectangle1(
                            out regionParamter,
                            roiParamter[0],
                            roiParamter[1],
                            roiParamter[2],
                            roiParamter[3]
                        );
                        break;
                    case ROIEnum.Ellipse:
                        roiParamter = new HTuple[5];
                        HOperatorSet.DrawEllipse(
                            hsmart.HalconWindow,
                            out roiParamter[0],
                            out roiParamter[1],
                            out roiParamter[2],
                            out roiParamter[3],
                            out roiParamter[4]
                        );
                        HOperatorSet.GenEllipse(
                            out regionParamter,
                            roiParamter[0],
                            roiParamter[1],
                            roiParamter[2],
                            roiParamter[3],
                            roiParamter[4]
                        );
                        break;
                }
            });
            
            hsmart.HalconWindow.DispObj(regionParamter);
        }

        private async void DrawROI_Click(object sender, RoutedEventArgs e) 
        {
            if(rbCircle.IsChecked== true) 
            {
                await DrawROI(ROIEnum.Circle);
            }
            else if (rbRectangle.IsChecked == true)
            {
                await DrawROI(ROIEnum.Rectangle1);
            }
            else if (rbEllipse.IsChecked == true)
            {
                await DrawROI(ROIEnum.Ellipse);
            }
        }

        private void Complete_Click(object sender, RoutedEventArgs e) 
        {
            node.OutputParamter.HobjectParamter = imageParamter;
            node.OutputParamter.HTupleListParamter = roiParamter;
            node.OutputParamter.ROIRegionParamter= regionParamter;
        }

        private void Clear_Click(object sender, RoutedEventArgs e) 
        {
            hsmart.HalconWindow.ClearWindow();
            if(imageParamter != null)
            {
                hsmart.HalconWindow.DispObj(imageParamter);
            }
            roiParamter = null;
            regionParamter = null;

        }
    }
}

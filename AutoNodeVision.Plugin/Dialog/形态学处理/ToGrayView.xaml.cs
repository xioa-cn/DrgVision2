using AutoNodeVision.Plugin.Views;
using HalconDotNet;
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

namespace AutoNodeVision.Plugin.Dialog.形态学处理
{
    /// <summary>
    /// ToGrayView.xaml 的交互逻辑
    /// </summary>
    public partial class ToGrayView : Window
    {
        public ToGrayView()
        {
            InitializeComponent();
        }

        public ToGrayView(FlowNodeView node):this()
        {
            this.node = node;
            

        }

        private FlowNodeView node;
        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            if(node.InputParamter.HobjectParamter==null)
            {
                MessageBox.Show("请先输入图像");
                return;
            }

            HOperatorSet.Rgb1ToGray(node.InputParamter.HobjectParamter, out HObject grayImage);
            hsmart.HalconWindow.DispObj(grayImage);

            MessageBox.Show("执行转为灰度图成功!");

            node.OutputParamter.HobjectParamter = grayImage;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (node != null && node.InputParamter.HobjectParamter != null)
            {
                hsmart.HalconWindow.DispObj(node.InputParamter.HobjectParamter);
            }
        }
    }
}

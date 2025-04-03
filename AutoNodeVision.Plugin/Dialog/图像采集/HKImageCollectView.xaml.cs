using AutoNodeVision.Plugin.Views;
using HalconDotNet;
using Microsoft.Win32;
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

namespace AutoNodeVision.Plugin.Dialog.图像采集
{
    /// <summary>
    /// HKImageCollectView.xaml 的交互逻辑
    /// </summary>
    public partial class HKImageCollectView : Window
    {
        public HKImageCollectView()
        {
            InitializeComponent();
        }

        public HKImageCollectView(FlowNodeView node):this()
        {
           this.node= node;
        }
        

        private FlowNodeView node;
        private static HImage image = null;
        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog= new OpenFileDialog();
            fileDialog.Filter = "图片文件|*.jpg;*.png;*.bmp";
            if (fileDialog.ShowDialog() == true)
            {         
                image= new HImage();
                image.ReadImage(fileDialog.FileName);
                hsmart.HalconWindow.DispObj(image);
            }
        }

        private void ExecuteComplete_Click(object sender, RoutedEventArgs e)
        {
            node.OutputParamter.HobjectParamter= image;
            MessageBox.Show("加载图像完成");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(image != null)
            {
                hsmart.HalconWindow.DispObj(image);
            }
        }
    }
}

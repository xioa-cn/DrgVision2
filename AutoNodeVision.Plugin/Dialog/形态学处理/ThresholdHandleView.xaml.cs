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
    /// ThresholdHandleView.xaml 的交互逻辑
    /// </summary>
    public partial class ThresholdHandleView : Window
    {
        public ThresholdHandleView()
        {
            InitializeComponent();
        }

        public ThresholdHandleView(FlowNodeView node):this()
        {
            this.node= node;
            
        }

        private FlowNodeView node;
        private HObject thresholdHobject=null;

       private void doThreshold(int minValue,int maxValue)
        {
           HOperatorSet.SetColor(hsmart.HalconWindow, "red");
           HOperatorSet.Threshold(node.InputParamter.HobjectParamter, out thresholdHobject, minValue, maxValue);
           hsmart.HalconWindow.DispObj(thresholdHobject);
        }

        private void minSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(minSlider.Value<maxSlider.Value)
            {
                doThreshold((int)minSlider.Value, (int)maxSlider.Value);
            }
        }

        private void maxSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (minSlider.Value < maxSlider.Value)
            {
                doThreshold((int)minSlider.Value, (int)maxSlider.Value);
            }
        }


        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            if (minSlider.Value < maxSlider.Value)
            {
                doThreshold((int)minSlider.Value, (int)maxSlider.Value);
            }
            node.OutputParamter.HobjectParamter = thresholdHobject;
            MessageBox.Show("二值化处理完成!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (node != null && node.InputParamter != null)
            {
                hsmart.HalconWindow.DispObj(node.InputParamter.HobjectParamter);
            }
        }
    }
}

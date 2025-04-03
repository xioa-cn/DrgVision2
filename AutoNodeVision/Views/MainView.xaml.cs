using AutoNodeVision.Models;
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

namespace AutoNodeVision.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 最小化窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mini_WindowClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化或还原窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxOrNomalWindow_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState==WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else if(this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
        }

            /// <summary>
            /// 关闭窗体
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoNodeVision.Models
{
    public class FlowModel:BindableBase
    {
        /// <summary>
        /// 流程名称
        /// </summary>
        public string Name { get; set; }


        private ObservableCollection<UserControl> flowControlList;

        /// <summary>
        /// 流程控件集合
        /// </summary>
        public ObservableCollection<UserControl> FlowControlList
        {
            get { return flowControlList; }
            set { flowControlList = value;RaisePropertyChanged(); }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNodeVision.Models
{
    public class LogModel : BindableBase
    {
        private string time;

        /// <summary>
        /// 日志时间
        /// </summary>
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChanged();
            }
        }

        private string message;

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

    }
}

using AutoNodeVision.Plugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNodeVision.Plugin.Service
{
    public interface IPlugin
    {
        public  void RunProcess();

        public  void ShowWindow(FlowNodeView nodeView);
    }
}

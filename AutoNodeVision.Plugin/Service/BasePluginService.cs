using AutoNodeVision.Plugin.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoNodeVision.Plugin.Service
{
    public abstract class BasePluginService :IPlugin
    {
        public virtual void ShowWindow(FlowNodeView nodeView)
        {
            throw new NotImplementedException();
        }

        public virtual void RunProcess()
        {
            throw new NotImplementedException();
        }

        
    }
}

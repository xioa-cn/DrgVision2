using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNodeVision.ViewModels.Dialog
{
    public class BaseDialogViewModel : BindableBase,IDialogAware
    {
        public DialogCloseListener RequestClose { get; set; } 

       

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
            
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}

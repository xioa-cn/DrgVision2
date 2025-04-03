using AutoNodeVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoNodeVision.ViewModels.Dialog
{
    public class AddFlowDialogViewModel:BaseDialogViewModel
    {
        #region 字段和属性
        private FlowModel flowModel;

		public FlowModel FlowModel
        {
			get { return flowModel; }
			set { flowModel = value; RaisePropertyChanged(); }
		}
        #endregion

        #region 命令

        //确定命令
        public DelegateCommand SureCommand { get; set; }

        /// <summary>
        /// 取消命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        #endregion


        #region 构造方法
        public AddFlowDialogViewModel()
        {
            FlowModel = new FlowModel();
            SureCommand = new DelegateCommand(Sure);
            CancelCommand = new DelegateCommand(Cancel);
        }

     
        #endregion


        #region 方法
        private void Sure()
        {
            if(!string.IsNullOrWhiteSpace(FlowModel.Name))
            {
                DialogParameters para= new DialogParameters();
                para.Add("flowParam", FlowModel);
                this.RequestClose.Invoke(para,ButtonResult.OK);
            }
            else
            {
                MessageBox.Show("流程名称不能为空");
            }
        }



        private void Cancel()
        {
            this.RequestClose.Invoke(ButtonResult.No);
        }

        #endregion
    }
}

using AutoNodeVision.ViewModels;
using AutoNodeVision.ViewModels.Dialog;
using AutoNodeVision.Views;
using AutoNodeVision.Views.Dialog;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AutoNodeVision
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();

            containerRegistry.RegisterDialog<AddFlowDialogView, AddFlowDialogViewModel>();
        }
    }

}

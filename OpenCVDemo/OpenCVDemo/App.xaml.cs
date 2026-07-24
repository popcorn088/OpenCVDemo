using System.Windows;
using OpenCVDemo.Services;
using OpenCVDemo.ViewModels;
using OpenCVDemo.Views;
using Prism.Ioc;
using UserControlsWPF.FileDialog;

namespace OpenCVDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IOpenFileService, OpenFileService>();
            containerRegistry.RegisterSingleton<IImageService, ImageService>();
            containerRegistry.RegisterForNavigation<Image>();
        }
    }
}

using OpenCVDemo.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using UserControlsWPF.FileDialog;
using OpenCvSharp;
using Prism.Regions;
using OpenCVDemo.Views;

namespace OpenCVDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IOpenFileService _openFileService;
        private readonly IImageService _imageService;
        public static string Title => "OpenCV Demo";
        public DelegateCommand FileOpenCommand { get; private set; }
        public MainWindowViewModel(IRegionManager regionManager, IOpenFileService openFileService, IImageService imageService)
        {
            _regionManager = regionManager;
            _openFileService = openFileService;
            _imageService = imageService;
            FileOpenCommand = new DelegateCommand(FileOpenCommandExecute);

            _regionManager.RegisterViewWithRegion("ContentRegion", nameof(Image));
        }

        private void FileOpenCommandExecute()
        {
            if (_openFileService.ShowDialog() == false)
            {
                return;
            }

            _imageService.Mat = new Mat(_openFileService.FileName);
        }
    }
}

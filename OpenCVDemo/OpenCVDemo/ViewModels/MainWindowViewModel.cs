using OpenCVDemo.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using UserControlsWPF.FileDialog;
using OpenCvSharp;
using Prism.Regions;
using OpenCVDemo.Views;
using System.ComponentModel;

namespace OpenCVDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IOpenFileService _openFileService;
        private readonly IImageService _imageService;
        public static string Title => "OpenCV Demo";
        public bool IsEnabled
        {
            get => _imageService.Mat is not null;
        }
        public bool IsLastMatEnabled
        {
            get => _imageService.LastMat is not null;
        }
        public DelegateCommand FileOpenCommand { get; }
        public DelegateCommand UndoCommand { get; }
        public DelegateCommand NegativeCommand { get; }
        public DelegateCommand GrayscaleCommand { get; }
        public DelegateCommand EqualizedCommand { get; }
        public DelegateCommand ThresholdCommand { get; }
        public MainWindowViewModel(IRegionManager regionManager, IOpenFileService openFileService, IImageService imageService)
        {
            _regionManager = regionManager;
            _openFileService = openFileService;
            _imageService = imageService;
            _imageService.ImageChanged += ImageChangedHandler;
            FileOpenCommand = new DelegateCommand(FileOpenCommandExecute);
            UndoCommand = new DelegateCommand(UndoCommandExecute);
            NegativeCommand = new DelegateCommand(NegativeCommandExecute);
            GrayscaleCommand = new DelegateCommand(GrayscaleCommandExecute);
            EqualizedCommand = new DelegateCommand(EqualizedCommandExecute);
            ThresholdCommand = new DelegateCommand(ThresholdCommandExecute);

            _regionManager.RegisterViewWithRegion("ContentRegion", nameof(Image));
        }

        private void ThresholdCommandExecute()
        {
            var oMat = new Mat();
            if (_imageService.Mat.Channels() >= 2)
            {
                Cv2.CvtColor(_imageService.Mat.Clone(), oMat, ColorConversionCodes.BGR2GRAY);
                Cv2.Threshold(oMat, oMat, 80.0, 210.0, ThresholdTypes.Binary);
                _imageService.Mat = oMat;
            }
        }

        private void EqualizedCommandExecute()
        {
            var oMat = new Mat();
            if (_imageService.Mat.Channels() >= 2)
            {
                Cv2.CvtColor(_imageService.Mat.Clone(), oMat, ColorConversionCodes.BGR2GRAY);
                Cv2.EqualizeHist(oMat, oMat);
                _imageService.Mat = oMat;
            }
        }

        private void GrayscaleCommandExecute()
        {
            Mat oMat = new Mat();
            Cv2.CvtColor(_imageService.Mat.Clone(), oMat, ColorConversionCodes.BGR2GRAY);
            _imageService.Mat = oMat;
        }

        private void NegativeCommandExecute()
        {
            Mat oMat = new Mat();
            Cv2.BitwiseNot(_imageService.Mat.Clone(), oMat);
            _imageService.Mat = oMat;
        }

        private void ImageChangedHandler(object sender, ImageServiceArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsEnabled)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLastMatEnabled)));
        }

        private void UndoCommandExecute()
        {
            _imageService.Mat = _imageService.LastMat.Clone();
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

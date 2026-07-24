using OpenCVDemo.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using OpenCvSharp.WpfExtensions;
using System.Windows.Media;

namespace OpenCVDemo.ViewModels
{
    public class ImageViewModel : BindableBase, INavigationAware
    {
        private readonly IImageService _imageService;
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public ImageViewModel(IImageService imageService)
        {
            _imageService = imageService;
            _imageService.ImageChanged += ImageChanged;
        }

        private void ImageChanged(object sender, ImageServiceArgs e)
        {
            ImageSource = BitmapSourceConverter.ToBitmapSource(_imageService.Mat);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
    }
}

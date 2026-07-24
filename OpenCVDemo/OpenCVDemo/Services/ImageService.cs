using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;


namespace OpenCVDemo.Services
{
    public interface IImageService
    {
        Mat Mat { get; set; }
        event EventHandler<ImageServiceArgs> ImageChanged;
    }

    public sealed class ImageServiceArgs : EventArgs
    {
        public Mat Mat { get; }
        public ImageServiceArgs(Mat mat)
        {
            Mat = mat;
        }
    }

    public class ImageService : IImageService
    {
        private Mat _mat;
        public Mat Mat
        {
            get => _mat;
            set
            {
                _mat = value;
                ImageChanged?.Invoke(null, new ImageServiceArgs(_mat));
            }
        }

        public event EventHandler<ImageServiceArgs> ImageChanged;
    }
}

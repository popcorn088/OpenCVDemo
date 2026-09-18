using OpenCVDemo.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using UserControlsWPF.FileDialog;
using OpenCvSharp;
using Prism.Regions;
using OpenCVDemo.Views;
using System.ComponentModel;
using System.Collections.Generic;

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
        public DelegateCommand LinesCommand { get; }
        public DelegateCommand CirclesCommand { get; }
        public DelegateCommand RectangleCommand { get; }
        public DelegateCommand EllipseCommand { get; }
        public DelegateCommand StringCommand { get; }
        public DelegateCommand PolylinesCommand { get; }
        public DelegateCommand Polylines2Command { get; }
        public DelegateCommand FillPolyCommand { get; }
        public DelegateCommand FillPoly2Command { get; }
        public DelegateCommand BlurCommand { get; }
        public DelegateCommand GaussianBlurCommand { get; }
        public DelegateCommand LaplacianCommand { get; }
        public DelegateCommand SobelCommand { get; }
        public DelegateCommand CannyCommand { get; }
        public DelegateCommand DilateCommand { get; }
        public DelegateCommand ErodeCommand { get; }
        public DelegateCommand GammaCommand { get; }
        public DelegateCommand HistgramCommand { get; }
        public MainWindowViewModel(IRegionManager regionManager, IOpenFileService openFileService, IImageService imageService)
        {
            _regionManager = regionManager;
            _openFileService = openFileService;
            _imageService = imageService;
            _imageService.ImageChanged += ImageChangedHandler;
            FileOpenCommand = new DelegateCommand(FileOpenCommandExecute);
            UndoCommand = new DelegateCommand(UndoCommandExecute);
            
            // 2章
            NegativeCommand = new DelegateCommand(NegativeCommandExecute);
            GrayscaleCommand = new DelegateCommand(GrayscaleCommandExecute);
            EqualizedCommand = new DelegateCommand(EqualizedCommandExecute);
            ThresholdCommand = new DelegateCommand(ThresholdCommandExecute);

            // 3章
            LinesCommand = new DelegateCommand(LinesCommandExecute);
            CirclesCommand = new DelegateCommand(CirclesCommandExecute);
            RectangleCommand = new DelegateCommand(RectangleCommandExecute);
            EllipseCommand = new DelegateCommand(EllipseCommandExecute);
            StringCommand = new DelegateCommand(StringCommandExecute);
            PolylinesCommand = new DelegateCommand(PolylinesCommandExecute);
            Polylines2Command = new DelegateCommand(Polylines2CommandExecute);
            FillPolyCommand = new DelegateCommand(FillPolyCommandExecute);
            FillPoly2Command = new DelegateCommand(FillPoly2CommandExecute);

            // 4章
            BlurCommand = new DelegateCommand(BlurCommandExecute);
            GaussianBlurCommand = new DelegateCommand(GaussianBlurCommandExecute);
            LaplacianCommand = new DelegateCommand(LaplacianCommandExecute);
            SobelCommand = new DelegateCommand(SobelCommandExecute);
            CannyCommand = new DelegateCommand(CannyCommandExecute);
            DilateCommand = new DelegateCommand(DilateCommandExecute);
            ErodeCommand = new DelegateCommand(ErodeCommandExecute);
            GammaCommand = new DelegateCommand(GammaCommandExecute);
            HistgramCommand = new DelegateCommand(HistgramCommandExecute);

            _regionManager.RegisterViewWithRegion("ContentRegion", nameof(Image));
        }

        private void HistgramCommandExecute()
        {
            var oMat = new Mat(400, 256 * 2, MatType.CV_8UC3, Scalar.White);
            var histgrams = new Mat[3];
            var colors = new Scalar[]
            {
                Scalar.Blue,
                Scalar.Green,
                Scalar.Red
            };
            int[] hdims = { 256 };
            Rangef[] ranges = { new Rangef(0, 256), };
            for (int ch = 0; ch < histgrams.Length; ch++)
            {
                histgrams[ch] = new Mat();
                Cv2.CalcHist(new Mat[] { _imageService.Mat }, new int[] { ch }, null, histgrams[ch], 1, hdims, ranges);
                Cv2.Normalize(histgrams[ch], histgrams[ch], 0, _imageService.Mat.Height, NormTypes.MinMax);

                DrawHistgram(oMat, histgrams[ch], colors[ch]);
            }
            Cv2.ImShow("ヒストグラム", oMat);
        }

        private void DrawHistgram(Mat histMat, Mat hist, Scalar color)
        {
            List<List<Point>> lLPoint = new List<List<Point>>();
            List<Point> lPoint = new List<Point>();
            for (int i = 0; i < 256; i++)
            {
                float v = hist.At<float>(i, 0);
                var bin = histMat.Width / 256;
                lPoint.Add(new Point(i * bin, histMat.Height - v - 1));
            }
            lLPoint.Add(lPoint);
            histMat.Polylines(lLPoint, false, color);
        }

        private void GammaCommandExecute()
        {
            var gamma = 2.0;
            var lutMat = new Mat(1, 256, MatType.CV_8UC1);
            var lut = new sbyte[256];
            for (int i = 0; i < lut.Length; i++)
            {
                lut[i] = (sbyte)(Math.Pow(i / 255.0, 1.0 / gamma) * 255.0);
            }
            for (int i = 0; i < lut.Length; i++)
            {
                lutMat.Set(0, i, unchecked((sbyte)lut[i]));
            }
            var oMat = new Mat();
            Cv2.LUT(_imageService.Mat, lutMat, oMat);
            _imageService.Mat = oMat;
        }

        private void ErodeCommandExecute()
        {
            var oMat = new Mat();
            Cv2.Erode(_imageService.Mat, oMat, new Mat());
            _imageService.Mat = oMat;
        }

        private void DilateCommandExecute()
        {
            var oMat = new Mat();
            Cv2.Dilate(_imageService.Mat, oMat, new Mat());
            _imageService.Mat = oMat;
        }

        private void CannyCommandExecute()
        {
            var oMat = new Mat();
            Cv2.CvtColor(_imageService.Mat, oMat, ColorConversionCodes.BGR2GRAY);
            Cv2.Canny(oMat, oMat, 40.0, 150.0);
            _imageService.Mat = oMat;
        }

        private void SobelCommandExecute()
        {
            var oMat = new Mat();
            Cv2.CvtColor(_imageService.Mat, oMat, ColorConversionCodes.BGR2GRAY);
            Cv2.Sobel(oMat, oMat, -1, 0, 1);
            _imageService.Mat = oMat;
        }

        private void LaplacianCommandExecute()
        {
            var oMat = new Mat();
            Cv2.CvtColor(_imageService.Mat, oMat, ColorConversionCodes.BGR2GRAY);
            Cv2.Laplacian(oMat, oMat, 0);
            _imageService.Mat = oMat;
        }

        private void GaussianBlurCommandExecute()
        {
            var oMat = new Mat();
            Cv2.GaussianBlur(_imageService.Mat, oMat, new Size(5, 5), 10.0);
            _imageService.Mat = oMat;
        }

        private void BlurCommandExecute()
        {
            var oMat = new Mat();
            Cv2.Blur(_imageService.Mat, oMat, new Size(5, 5));
            _imageService.Mat = oMat;
        }

        private void FillPoly2CommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var xUnit = oMat.Width / 8;
            var yUnit = oMat.Height / 8;
            List<List<Point>> lLPoint2D = new()
            {
                new ()
                {
                    new (4 * xUnit, 1 * yUnit),
                    new (7 * xUnit, 6 * yUnit),
                    new (1 * xUnit, 6 * yUnit),
                },
                new ()
                {
                    new (1 * xUnit, 2 * yUnit),
                    new (7 * xUnit, 2 * yUnit),
                    new (4 * xUnit, 7 * yUnit),
                },
            };
            Cv2.FillPoly(oMat, lLPoint2D, Scalar.HotPink);
            _imageService.Mat = oMat;
        }

        private void FillPolyCommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var xUnit = oMat.Width / 8;
            var yUnit = oMat.Height / 8;
            List<List<Point>> lLPoint2D = new()
            {
                new ()
                {
                    new (4 * xUnit, 1 * yUnit),
                    new (7 * xUnit, 6 * yUnit),
                    new (1 * xUnit, 6 * yUnit),
                },
                new ()
                {
                    new (1 * xUnit, 2 * yUnit),
                    new (7 * xUnit, 2 * yUnit),
                    new (4 * xUnit, 7 * yUnit),
                },
            };
            lLPoint2D.RemoveAt(lLPoint2D.Count - 1);
            Cv2.FillPoly(oMat, lLPoint2D, Scalar.HotPink);
            _imageService.Mat = oMat;
        }

        private void Polylines2CommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var xUnit = oMat.Width / 8;
            var yUnit = oMat.Height / 8;
            List<List<Point>> lLPoint2D = new()
            {
                new ()
                {
                    new (4 * xUnit, 1 * yUnit),
                    new (7 * xUnit, 6 * yUnit),
                    new (1 * xUnit, 6 * yUnit),
                },
                new ()
                {
                    new (1 * xUnit, 2 * yUnit),
                    new (7 * xUnit, 2 * yUnit),
                    new (4 * xUnit, 7 * yUnit),
                },
            };
            Cv2.Polylines(oMat, lLPoint2D, true, Scalar.LimeGreen, 3);
            _imageService.Mat = oMat;
        }

        private void PolylinesCommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var xUnit = oMat.Width / 8;
            var yUnit = oMat.Height / 8;
            List<List<Point>> lLPoint2D = new()
            {
                new ()
                {
                    new (4 * xUnit, 1 * yUnit),
                    new (7 * xUnit, 6 * yUnit),
                    new (1 * xUnit, 6 * yUnit),
                },
                new ()
                {
                    new (1 * xUnit, 2 * yUnit),
                    new (7 * xUnit, 2 * yUnit),
                    new (4 * xUnit, 7 * yUnit),
                },
            };
            lLPoint2D.RemoveAt(lLPoint2D.Count - 1);
            Cv2.Polylines(oMat, lLPoint2D, true, Scalar.LimeGreen, 2);
            _imageService.Mat = oMat;
        }

        private void StringCommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var p = new Point(oMat.Width / 10, oMat.Height / 2);
            Cv2.PutText(oMat, "Hello OpenCV", p, HersheyFonts.HersheyTriplex, 0.8, new Scalar(250, 200, 200), 2, LineTypes.AntiAlias);
            _imageService.Mat = oMat;
        }

        private void EllipseCommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var eCenter = new Point(oMat.Width / 2, oMat.Height / 2);
            var size = new Size(oMat.Width / 2, oMat.Height / 2);
            Cv2.Ellipse(oMat, eCenter, size, 0, 0, 360, new Scalar(255, 255, 0), 2, LineTypes.Link4);
            size.Width -= size.Width / 4;
            size.Height -= size.Height / 4;
            Cv2.Ellipse(oMat, eCenter, size, 15, 10, 300, new Scalar(255, 255, 0), 2, LineTypes.Link4);
            _imageService.Mat = oMat;
        }

        private void RectangleCommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var p0 = new Point(oMat.Width / 8, oMat.Height / 8);
            var p1 = new Point(oMat.Width * 7 / 8, oMat.Height * 7 / 8);
            Cv2.Rectangle(oMat, p0, p1, new Scalar(0, 255, 0), 5, LineTypes.Link8);
            var p2 = new Point(oMat.Width * 2 / 8, oMat.Height * 2 / 8);
            var p3 = new Point(oMat.Width * 6 / 8, oMat.Height * 6 / 8);
            Cv2.Rectangle(oMat, p2, p3, new Scalar(0, 255, 255), 4, LineTypes.AntiAlias);
            _imageService.Mat = oMat;
        }

        private void CirclesCommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var center = new Point(oMat.Width / 2, oMat.Height / 2);
            Cv2.Circle(oMat, center, oMat.Height / 3, new Scalar(0, 255, 0), 3);
            Cv2.Circle(oMat, center, oMat.Height / 6, new Scalar(255, 255, 0), -1);
            _imageService.Mat = oMat;
        }

        private void LinesCommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var x0 = oMat.Width / 4;
            var x1 = oMat.Width * 3 / 4;
            var y0 = oMat.Height / 4;
            var y1 = oMat.Height * 3 / 4;
            Cv2.Line(oMat, x0, y0, x1, y1, new Scalar(0, 0, 255), 3, LineTypes.Link4);
            Cv2.Line(oMat, x1, y0, x0, y1, new Scalar(255, 0, 0), 3, LineTypes.Link4);
            _imageService.Mat = oMat;
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

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
using System.Runtime.InteropServices;
using Prism.Services.Dialogs;
using OpenCvSharp.XImgProc;
using I = System.Windows.Input;
using W = System.Windows;
using System.Windows.Media.Media3D;
using OpenCVDemo.Managers;

namespace OpenCVDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public enum MouseDownModes
        {
            PerspectiveRectangle,
            RemoveObject,
            None,
        }
        private readonly IRegionManager _regionManager;
        private readonly IOpenFileService _openFileService;
        private readonly IImageService _imageService;
        private readonly IDialogService _dialogService;
        public static string Title => "OpenCV Demo";
        public bool IsEnabled
        {
            get => _imageService.Mat is not null;
        }
        public bool IsLastMatEnabled
        {
            get => _imageService.LastMat is not null;
        }
        private double _contentWidth;
        public double ContentWidth
        {
            get => _contentWidth;
            set => SetProperty(ref _contentWidth, value);
        }
        private double _contentHeight;
        public double ContentHeight
        {
            get => _contentHeight;
            set => SetProperty(ref _contentHeight, value);
        }
        private MouseDownModes mode { get; set; } = MouseDownModes.None;
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
        public DelegateCommand VerticalFlipCommand { get; }
        public DelegateCommand HorizontalFlipCommand { get; }
        public DelegateCommand VerticalAndHorizontalFlipCommand { get; }
        public DelegateCommand Resize5Command { get; }
        public DelegateCommand Resize8Command { get; }
        public DelegateCommand Resize12Command { get; }
        public DelegateCommand Rotation333Command { get; }
        public DelegateCommand Rotation1235Command { get; }
        public DelegateCommand Rotation2901Command { get; }
        public DelegateCommand Perspective1Command { get; }
        public DelegateCommand Perspective2Command { get; }
        public DelegateCommand Perspective3Command { get; }
        public DelegateCommand DetectConers1Command { get; }
        public DelegateCommand DetectConers2Command { get; }
        public DelegateCommand FindRectsCommand { get; }
        public DelegateCommand RepairCommand { get; }
        public DelegateCommand ThinCommand { get; }
        public DelegateCommand PerspectiveRectangleCommand { get; }
        public DelegateCommand<I.MouseButtonEventArgs> MouseLeftButtonDownCommand { get; }
        public DelegateCommand RemoveObjectCommand { get; }
        PersObjManeger persObjManager;
        RemoveObjManeger removeObjMaanager;

        public MainWindowViewModel(IRegionManager regionManager, IOpenFileService openFileService, IImageService imageService, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _openFileService = openFileService;
            _imageService = imageService;
            _dialogService = dialogService;

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

            // 5章
            VerticalFlipCommand = new DelegateCommand(VerticalFlipCommandExecute);
            HorizontalFlipCommand = new DelegateCommand(HorizontalFlipCommandExecute);
            VerticalAndHorizontalFlipCommand = new DelegateCommand(VerticalAndHorizontalFlipCommandExecute);
            Resize5Command = new DelegateCommand(Resize5CommandExecute);
            Resize8Command = new DelegateCommand(Resize8CommandExecute);
            Resize12Command = new DelegateCommand(Resize12CommandExecute);
            Rotation333Command = new DelegateCommand(Rotation333CommandExecute);
            Rotation1235Command = new DelegateCommand(Rotation1235CommandExecute);
            Rotation2901Command = new DelegateCommand(Rotation2901CommandExecute);
            Perspective1Command = new DelegateCommand(Perspective1CommandExecute);
            Perspective2Command = new DelegateCommand(Perspective2CommandExecute);
            Perspective3Command = new DelegateCommand(Perspective3CommandExecute);

            // 6章
            DetectConers1Command = new DelegateCommand(DetectConers1CommandExecute);
            DetectConers2Command = new DelegateCommand(DetectConers2CommandExecute);
            FindRectsCommand = new DelegateCommand(FindRectsCommandExecute);
            RepairCommand = new DelegateCommand(RepairCommandExecute);
            ThinCommand = new DelegateCommand(ThinCommandExecute);

            // 7章
            persObjManager = new PersObjManeger();
            removeObjMaanager = new RemoveObjManeger();
            PerspectiveRectangleCommand = new DelegateCommand(PerspectiveRectangleCommandExecute);
            MouseLeftButtonDownCommand = new DelegateCommand<I.MouseButtonEventArgs>(MouseLeftButtonDownCommandExecute);
            RemoveObjectCommand = new DelegateCommand(RemoveObjectCommandExecute);

            _regionManager.RegisterViewWithRegion("ContentRegion", nameof(Image));
        }

        private void RemoveObjectCommandExecute()
        {
            mode = MouseDownModes.RemoveObject;
        }

        private void PerspectiveRectangleCommandExecute()
        {
            mode = MouseDownModes.PerspectiveRectangle;
        }

        private void MouseLeftButtonDownCommandExecute(I.MouseButtonEventArgs args)
        {
            if (mode == MouseDownModes.PerspectiveRectangle)
            {
                var oMat = persObjManager.MouseLeftButtonDownCommandExecute(args);
                _imageService.Mat = oMat;
                if (persObjManager.MousePointsCount == 0)
                {
                    persObjManager.SourceMat = _imageService.Mat.Clone();
                    mode = MouseDownModes.None;
                }
            }
            else if (mode == MouseDownModes.RemoveObject)
            {
                var oMat = removeObjMaanager.MouseLeftButtonDownCommandExecute(args);
                _imageService.Mat = oMat;
            }
            else
            {
                mode = MouseDownModes.None;
            }
        }

        private void ThinCommandExecute()
        {
            var oMat = new Mat();
            using var gray = new Mat();
            Cv2.CvtColor(_imageService.Mat, gray, ColorConversionCodes.RGB2GRAY);
            CvXImgProc.Thinning(gray, oMat, ThinningTypes.ZHANGSUEN);
            _imageService.Mat = oMat;
        }

        private void RepairCommandExecute()
        {
            var oMat = new Mat();
            using var gray = new Mat();
            using var mask = new Mat();
            Cv2.CvtColor(_imageService.Mat, gray, ColorConversionCodes.RGB2GRAY);
            Cv2.EqualizeHist(gray, mask);
            Cv2.Threshold(mask, mask, 253, 1, ThresholdTypes.Binary);
            Cv2.Inpaint(_imageService.Mat, mask, oMat, 3, InpaintTypes.Telea);
            _imageService.Mat = oMat;
        }

        private void FindRectsCommandExecute()
        {
            _dialogService.ShowDialog(nameof(TextBox), (result) =>
            {
                if (result.Result == ButtonResult.Cancel)
                {
                    return;
                }

                var inputText = result.Parameters.GetValue<string>("InputText");
                char[] delimitter = { 'X', 'x' };
                try
                {
                    var resolutions = inputText.Split(delimitter);
                    var width = int.Parse(resolutions[0]);
                    var height = int.Parse(resolutions[1]);
                    FindRects(width, height, _imageService.Mat.Rows);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    return;
                }
            });
        }

        private void FindRects(int width, int height, int dispHeight)
        {
            int detects = 0;
            var oMat = _imageService.Mat.Clone();

            var gray = new Mat();
            Cv2.CvtColor(_imageService.Mat, gray, ColorConversionCodes.RGB2GRAY);
            Cv2.Threshold(gray, gray, 128, 255, ThresholdTypes.Binary);
            Cv2.FindContours(gray, out Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxTC89L1);
            for (int i = 0; i < contours.Length; i++)
            {
                Cv2.DrawContours(oMat, contours, i, Scalar.Green, 2);
            }
            for (int i = 0; i < contours.Length; i++)
            {
                double a = Cv2.ContourArea(contours[i], false);
                if (a > width * height)
                {
                    Point[] approx;
                    approx = Cv2.ApproxPolyDP(contours[i], 0.01 * Cv2.ArcLength(contours[i], true), true);
                    if (approx.Length == 4)
                    {
                        detects++;
                        Point[][] tmpoContours = new Point[][] { approx };
                        int maxLevel = 0;
                        Cv2.DrawContours(oMat, tmpoContours, 0, Scalar.Red, 2, LineTypes.AntiAlias, hierarchy, maxLevel);
                    }
                }
            }
            float scale = (float)dispHeight / (float)oMat.Height;
            var dipDst = new Mat();
            Cv2.Resize(oMat, dipDst, new Size(), scale, scale);
            _imageService.Mat = dipDst;
        }

        private void DetectConers2CommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var grayMat = new Mat();

            Cv2.CvtColor(_imageService.Mat, grayMat, ColorConversionCodes.RGB2GRAY);
            Cv2.Threshold(grayMat, grayMat, 128, 255, ThresholdTypes.Binary);

            Cv2.FindContours(grayMat, out Point[][] contours, out HierarchyIndex[] hierarchies, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            for (int i = 0; i < contours.Length; i++)
            {
                Cv2.DrawContours(oMat, contours, i, Scalar.Green, 2, LineTypes.Link8, hierarchies, 0);
            }
            _imageService.Mat = oMat;
        }

        private void DetectConers1CommandExecute()
        {
            var oMat = _imageService.Mat.Clone();
            var grayMat = new Mat();

            Cv2.CvtColor(_imageService.Mat, grayMat, ColorConversionCodes.BGR2GRAY);

            const int maxCorners = 50, blockSize = 3;
            const double qualityLevel = 0.01, minDistance = 20.0, k = 0.04;
            const bool useHarrisDetector = false;
            Point2f[] corners = Cv2.GoodFeaturesToTrack(grayMat, maxCorners, qualityLevel, minDistance, new Mat(), blockSize, useHarrisDetector, k);
            foreach (Point2f it in corners)
            {
                Cv2.Circle(oMat, (Point)it, 4, Scalar.Blue, 2);
            }
            _imageService.Mat = oMat;
        }

        private void Perspective3CommandExecute()
        {
            var oMat = new Mat();

            var common = PerspectiveCommon();
            var x0 = common.Item1;
            var x1 = common.Item2;
            var y0 = common.Item3;
            var y1 = common.Item4;

            var xMergin = common.Item5;
            var yMergin = common.Item6;

            var srcPoints = common.Item7;
            Point2f[] dstPoints = new Point2f[4];

            dstPoints[0] = srcPoints[0];
            dstPoints[1] = new Point2f(x0 + xMergin, y1 - yMergin);
            dstPoints[2] = srcPoints[2];
            dstPoints[3] = new Point2f(x1 - xMergin, y0 + yMergin); 

            Mat perspectiveMmat = Cv2.GetPerspectiveTransform(srcPoints, dstPoints);
            Cv2.WarpPerspective(_imageService.Mat, oMat, perspectiveMmat, _imageService.Mat.Size(), InterpolationFlags.Cubic);
            _imageService.Mat = oMat;
        }

        private void Perspective2CommandExecute()
        {
            var oMat = new Mat();

            var common = PerspectiveCommon();
            var x0 = common.Item1;
            var x1 = common.Item2;
            var y0 = common.Item3;
            var y1 = common.Item4;

            var xMergin = common.Item5;
            var yMergin = common.Item6;

            var srcPoints = common.Item7;
            Point2f[] dstPoints = new Point2f[4];

            dstPoints[0] = srcPoints[0];
            dstPoints[1] = new Point2f(x0 + xMergin, y1 - yMergin);
            dstPoints[2] = new Point2f(x1 - xMergin, y1 - yMergin);
            dstPoints[3] = srcPoints[3];

            Mat perspectiveMmat = Cv2.GetPerspectiveTransform(srcPoints, dstPoints);
            Cv2.WarpPerspective(_imageService.Mat, oMat, perspectiveMmat, _imageService.Mat.Size(), InterpolationFlags.Cubic);
            _imageService.Mat = oMat;
        }

        private void Perspective1CommandExecute()
        {
            var oMat = new Mat();

            var common = PerspectiveCommon();
            var x0 = common.Item1;
            var x1 = common.Item2;
            var y0 = common.Item3;
            var y1 = common.Item4;

            var xMergin = common.Item5;
            var yMergin = common.Item6;

            var srcPoints = common.Item7;
            Point2f[] dstPoints = new Point2f[4];

            dstPoints[0] = new Point2f(x0 + xMergin, y0 + yMergin);
            dstPoints[1] = srcPoints[1];
            dstPoints[2] = srcPoints[2];
            dstPoints[3] = new Point2f(x1 - xMergin, y0 + yMergin);

            Mat perspectiveMmat = Cv2.GetPerspectiveTransform(srcPoints, dstPoints);
            Cv2.WarpPerspective(_imageService.Mat, oMat, perspectiveMmat, _imageService.Mat.Size(), InterpolationFlags.Cubic);
            _imageService.Mat = oMat;
        }

        Tuple<float, float, float, float, int, int, Point2f[]> PerspectiveCommon()
        {
            var x0 = (float)(_imageService.Mat.Cols / 4);
            var x1 = (float)((_imageService.Mat.Cols / 4) * 3);
            var y0 = (float)(_imageService.Mat.Rows / 4);
            var y1 = (float)((_imageService.Mat.Rows / 4) * 3);

            var xMergin = _imageService.Mat.Cols / 10;
            var yMergin = _imageService.Mat.Rows / 10;

            Point2f[] srcPoints = new Point2f[]
            {
                new Point2f(x0, y0),
                new Point2f(x0, y1),
                new Point2f(x1, y1),
                new Point2f(x1, y0),
            };

            return new Tuple<float, float, float, float, int, int, Point2f[]>(x0, x1, y0, y1, xMergin, yMergin, srcPoints);
        }

        private void Rotation2901CommandExecute()
        {
            var oMat = new Mat();
            var center = new Point2f(_imageService.Mat.Cols / 2, _imageService.Mat.Rows / 2);
            var affinieTrans = Cv2.GetRotationMatrix2D(center, 290.1, 1.0);
            Cv2.WarpAffine(_imageService.Mat, oMat, affinieTrans, _imageService.Mat.Size(), InterpolationFlags.Cubic);
            _imageService.Mat = oMat;
        }

        private void Rotation1235CommandExecute()
        {
            var oMat = new Mat();
            var center = new Point2f(_imageService.Mat.Cols / 2, _imageService.Mat.Rows / 2);
            var affinieTrans = Cv2.GetRotationMatrix2D(center, 123.5, 1.0);
            Cv2.WarpAffine(_imageService.Mat, oMat, affinieTrans, _imageService.Mat.Size(), InterpolationFlags.Cubic);
            _imageService.Mat = oMat;
        }

        private void Rotation333CommandExecute()
        {
            var oMat = new Mat();
            var center = new Point2f(_imageService.Mat.Cols / 2, _imageService.Mat.Rows / 2);
            var affinieTrans = Cv2.GetRotationMatrix2D(center, 33.3, 1.0);
            Cv2.WarpAffine(_imageService.Mat, oMat, affinieTrans, _imageService.Mat.Size(), InterpolationFlags.Cubic);
            _imageService.Mat = oMat;
        }

        private void Resize12CommandExecute()
        {
            var oMat = new Mat();
            Cv2.Resize(_imageService.Mat, oMat, new Size(), 1.2, 1.2);
            _imageService.Mat = oMat;
            ContentWidth = _imageService.Mat.Width;
            ContentHeight = _imageService.Mat.Height;
        }

        private void Resize8CommandExecute()
        {
            var oMat = new Mat();
            Cv2.Resize(_imageService.Mat, oMat, new Size(), 0.8, 0.8);
            _imageService.Mat = oMat;
            ContentWidth = _imageService.Mat.Width;
            ContentHeight = _imageService.Mat.Height;
        }

        private void Resize5CommandExecute()
        {
            var oMat = new Mat();
            Cv2.Resize(_imageService.Mat, oMat, new Size(), 0.5, 0.5);
            _imageService.Mat = oMat;
            ContentWidth = _imageService.Mat.Width;
            ContentHeight = _imageService.Mat.Height;
        }

        private void VerticalAndHorizontalFlipCommandExecute()
        {
            var oMat = new Mat();
            Cv2.Flip(_imageService.Mat, oMat, FlipMode.XY);
            _imageService.Mat = oMat;
        }

        private void HorizontalFlipCommandExecute()
        {
            var oMat = new Mat();
            Cv2.Flip(_imageService.Mat, oMat, FlipMode.Y);
            _imageService.Mat = oMat;
        }

        private void VerticalFlipCommandExecute()
        {
            var oMat = new Mat();
            Cv2.Flip(_imageService.Mat, oMat, FlipMode.X);
            _imageService.Mat = oMat;
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
            persObjManager.SourceMat = _imageService.Mat.Clone();
        }

        private void FileOpenCommandExecute()
        {
            if (_openFileService.ShowDialog() == false)
            {
                return;
            }

            _imageService.Mat = new Mat(_openFileService.FileName);
            ContentWidth = _imageService.Mat.Width;
            ContentHeight = _imageService.Mat.Height;
            persObjManager.SourceMat = _imageService.Mat.Clone();
            removeObjMaanager.SourceMat = _imageService.Mat.Clone();
        }
    }
}

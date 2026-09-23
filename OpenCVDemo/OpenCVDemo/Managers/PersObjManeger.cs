using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCVDemo.Services;
using OpenCvSharp;

namespace OpenCVDemo.Managers
{
    public class PersObjManeger
    {
        List<List<Point>> mouseDownPoints = new List<List<Point>>() { new List<Point>() };
        private Mat souceMat = new Mat();
        public Mat SourceMat
        {
            get => souceMat;
            set
            {
                souceMat = value;
                pointMat = souceMat.Clone();
            }
        }
        public int MousePointsCount
        {
            get => mouseDownPoints[0].Count;
        }
        Mat pointMat;
        public PersObjManeger()
        {

        }

        public Mat MouseLeftButtonDownCommandExecute(System.Windows.Input.MouseButtonEventArgs args)
        {
            if (args.Source is not System.Windows.IInputElement)
            {
                return SourceMat.Clone();
            }

            var element = args.Source as System.Windows.IInputElement;
            var point = args.GetPosition(element);
            // クリックした座標に赤丸を付ける
            var cv2Point = new Point(point.X, point.Y);
            Cv2.Circle(pointMat, cv2Point, 3, Scalar.Red, -1);
            // クリックした座標が4つになったらソート。左上が0、左下が1、右下が2、右上が3にする
            if (mouseDownPoints[0].Count != 4)
            {
                mouseDownPoints[0].Add(cv2Point);
                return pointMat.Clone();
            }
            else
            {
                mouseDownPoints[0].Sort((a, b) => a.X - b.X);
                if (mouseDownPoints[0][0].Y > mouseDownPoints[0][1].Y)
                {
                    (mouseDownPoints[0][0], mouseDownPoints[0][1]) = (mouseDownPoints[0][1], mouseDownPoints[0][0]);
                }
                if (mouseDownPoints[0][2].Y < mouseDownPoints[0][3].Y)
                {
                    (mouseDownPoints[0][2], mouseDownPoints[0][3]) = (mouseDownPoints[0][3], mouseDownPoints[0][2]);
                }

                // 投射投影処理
                var oMat = new Mat(SourceMat.Height, SourceMat.Width, SourceMat.Type());
                var sourcePoints = new Point2f[4];
                for (int i = 0; i < mouseDownPoints[0].Count; i++)
                {
                    sourcePoints[i] = (Point2f)mouseDownPoints[0][i];
                }
                var dstPoints = new Point2f[]
                {
                        new Point2f(0.0f, 0.0f),
                        new Point2f(0.0f, (float)(oMat.Height - 1)),
                        new Point2f((float)(oMat.Width - 1), (float)(oMat.Height - 1)),
                        new Point2f((float)(oMat.Width - 1), 0.0f),
                };
                var persMat = Cv2.GetPerspectiveTransform(sourcePoints, dstPoints);
                Cv2.WarpPerspective(SourceMat, oMat, persMat, oMat.Size(), InterpolationFlags.Cubic);

                // 終わったので解放
                mouseDownPoints[0].Clear();

                return oMat.Clone();
            }
        }
    }
}

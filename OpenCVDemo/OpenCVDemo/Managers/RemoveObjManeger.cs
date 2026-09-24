using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVDemo.Managers
{
    public class RemoveObjManeger
    {
        List<Point> mouseDownPoints = new();
        private Mat sourceMat = new Mat();
        public Mat SourceMat
        {
            get => sourceMat;
            set
            {
                sourceMat = value;
                pointMat = sourceMat.Clone();
            }
        }
        Mat pointMat;
        public Mat MouseLeftButtonDownCommandExecute(System.Windows.Input.MouseButtonEventArgs args)
        {
            if (args.Source is not System.Windows.IInputElement)
            {
                return SourceMat.Clone();
            }

            var element = args.Source as System.Windows.IInputElement;
            var point = args.GetPosition(element);
            var cv2Point = new Point(point.X, point.Y);
            Cv2.Circle(pointMat, cv2Point, 3, Scalar.Red, -1);
            if (mouseDownPoints.Count != 2)
            {
                mouseDownPoints.Add(cv2Point);
                return pointMat.Clone();
            }
            else
            {
                mouseDownPoints.Sort((a, b) => a.X - b.X);

                // オブジェクト除去処理
                var mask = new Mat(SourceMat.Size(), MatType.CV_8UC1, new Scalar(0));
                Cv2.Rectangle(mask, mouseDownPoints[0], mouseDownPoints[1], new Scalar(255), -1, LineTypes.AntiAlias);
                var oMat = new Mat();
                Cv2.Inpaint(SourceMat, mask, oMat, 1, InpaintTypes.Telea);

                mouseDownPoints.Clear();
                return oMat.Clone();
            }
        }
    }
}

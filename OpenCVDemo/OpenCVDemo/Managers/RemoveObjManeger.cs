using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVDemo.Managers
{
    public class RemoveObjManeger : MouseDownObjectManger
    {
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
            pointMat = base.MouseDownCommandExecute(pointMat, args);
            var mouseDownPoints = base.GetPoints();
            if (mouseDownPoints.Count != 2)
            {
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

                base.Clear();
                return oMat.Clone();
            }
        }
    }
}

using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVDemo.Managers
{
    public class MouseDownObjectManger : MouseDownPointsManager
    {
        public Mat MouseDownCommandExecute(Mat pointMat, System.Windows.Input.MouseButtonEventArgs args)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(pointMat));

            base.MouseDownCommand(args);
            if (base.MouseDownPoints.Count > 0)
            {
                foreach (var point in MouseDownPoints)
                {
                    Cv2.Circle(pointMat, point, 3, Scalar.Red, -1);
                }
            }

            return pointMat;
        }
    }
}

using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVDemo.Managers
{
    public static class MouseDownPoint
    {
        public static Point? MouseDownCommand(System.Windows.Input.MouseButtonEventArgs args)
        {
            if (args.Source is not System.Windows.IInputElement)
            {
                return null;
            }

            var element = args.Source as System.Windows.IInputElement;
            var point = args.GetPosition(element);
            return new Point(point.X, point.Y);
        }
    }

    public class MouseDownPointsManager
    {
        protected List<Point> MouseDownPoints = new();
        public void MouseDownCommand(System.Windows.Input.MouseButtonEventArgs args)
        {
            var point = MouseDownPoint.MouseDownCommand(args);
            if (!point.HasValue)
            {
                return;
            }

            MouseDownPoints.Add(point.Value);
        }

        public List<Point> GetPoints()
        {
            return MouseDownPoints;
        }

        public void Clear()
        {
            MouseDownPoints.Clear();
        }
    }
}

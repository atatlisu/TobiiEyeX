using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TobiiEyeX.helper
{
    class Helper
    {
        // calculates the median of the given double values
        public static double calcMedianDouble(List<double> f_values)
        {
            double median = 0;
            if (f_values.Count != 0)
            {
                f_values.Sort();

                if (f_values.Count % 2 == 0)
                {
                    int idx_left_to_mid = (f_values.Count - 1) / 2;
                    median = (f_values[idx_left_to_mid] + f_values[idx_left_to_mid + 1]) / 2.0f;
                }
                else
                {
                    int idx_mid = (f_values.Count - 1) / 2;
                    median = f_values[idx_mid];
                }
            }
            return median;
        }

        // calculates the median of all x and y values of the given points and returns the resulting point
        public static Point calcMedianPoint(List<Point> points)
        {
            List<double> x_values = new List<double>();
            List<double> y_values = new List<double>();
            foreach (Point p in points)
            {
                x_values.Add(p.X);
                y_values.Add(p.Y);
            }

            return new Point(calcMedianDouble(x_values), calcMedianDouble(y_values));
        }
    }
}

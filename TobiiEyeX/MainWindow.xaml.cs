using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EyeXFramework.Wpf;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace TobiiEyeX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private enum Corners { TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT };

        // time for which the data points of one corner are recorded
        private int recTimeMs = 3000;

        // the latest point of the gaze position (set when a new gaze data event occurs)
        private Point latestGazePoint = new Point(0.0f, 0.0f);

        // if set to true, the new arriving gaze data points are stored in the vector
        private volatile bool streamRecActive = false;
        private List<Point> recGazePoints = new List<Point>();

        // background process that processes the gaze data stream
        private readonly BackgroundWorker procStreamWorker = new BackgroundWorker();

        // background process that stores the gaze data points
        private readonly BackgroundWorker recDataWorker = new BackgroundWorker();

        // measure time used to skip some points (only in the textBlock displaying) to improve readability in the window
        private Stopwatch stopWatch = new Stopwatch();

        // when the 'recDataWorker' has finished, the calibration points are stored in this array
        private Point[] calibrPoints = new Point[Enum.GetNames(typeof(Corners)).Length];


        public MainWindow()
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();

            stopWatch.Start();

            procStreamWorker.DoWork += procStreamWorker_DoWork;
            procStreamWorker.RunWorkerAsync();

            recDataWorker.DoWork += recDataWorker_DoWork;
            recDataWorker.RunWorkerCompleted += recDataWorker_Completed;
            recDataWorker.RunWorkerAsync();
        }


        private void procStreamWorker_DoWork(object sender, DoWorkEventArgs eArgs)
        {
            /*using (var eyeXHost = new EyeXHost())
            {
                // Create a data stream: lightly filtered gaze point data.
                // Other choices of data streams include EyePositionDataStream and FixationDataStream.
                using (var lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered))
                {
                    eyeXHost.Start();
                    // eyeXHost.LaunchCalibrationTesting();

                    lightlyFilteredGazeDataStream.Next += (s, e) =>
                    {
                        Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) @{2:0}", e.X, e.Y, e.Timestamp);
                        setDataPointTextBlock("Raw gaze data point (x,y): ( " + e.X.ToString("0") + " , " + e.Y.ToString("0") + " )" +
                            Environment.NewLine + "(data points skipped only in this textBlock for better readability)");

                        latestGazePoint = new Point(e.X, e.Y);

                        lock (recGazePoints)
                        {
                            if (streamRecActive)
                            {
                                recGazePoints.Add(latestGazePoint);
                            }
                        }
                    };

                    System.Threading.Thread.Sleep(Timeout.Infinite);
                }
            }*/
        }

        private void recDataWorker_DoWork(object sender, DoWorkEventArgs eArgs)
        {
            Array cornersEnumValues = Enum.GetValues(typeof(Corners));
            foreach (Corners c in cornersEnumValues)
            {
                setCornerTextBlock("Currently recording data for this corner:  " + Enum.GetName(typeof(Corners), c));

                // short delay to ensure that the user looks at the designated corner
                System.Threading.Thread.Sleep(2000);

                recordAndStorePoints(c);
            }
        }

        /*
        Records the gaze data points of the given corner for a specified time
         and writes them into a file.
        This overwrites previously added data
        */
        private void recordAndStorePoints(Corners corner)
        {
            // remove all previously stored points to make sure that the list only contains the points of the currently recorded corner
            /*recGazePoints.Clear();

            streamRecActive = true;

            // store the data points occuring in the specified period
            System.Threading.Thread.Sleep(recTimeMs);

            streamRecActive = false;

            string gazeRecStr = "";
            gazeRecStr += " [ " + corner + " ] " + Environment.NewLine;

            foreach (Point p in recGazePoints)
            {
                gazeRecStr += " ( " + p.X + " , " + p.Y + " ) " + Environment.NewLine;
            }
            gazeRecStr += Environment.NewLine;

            System.IO.File.WriteAllText(@"gaze_rec_file_" + ((int)corner) + "_" + corner + ".txt", gazeRecStr);


            // calculate and set the calibration point for the specific corner 
            //  by using the median of the x and y values of the recorded points
            calibrPoints[(int)corner] = Helper.calcMedianPoint(recGazePoints);*/
        }

        // set the text that is displayed in the textBlock for the corner description
        private void setCornerTextBlock(string text)
        {
            this.Dispatcher.Invoke(() =>
            {
                textBlockCorner.Text = text;
            });
        }



        // set the text that is displayed in the textBlock for the new arriving data points
        private void setDataPointTextBlock(string text)
        {
            if (stopWatch.ElapsedMilliseconds > 300)
            {
                this.Dispatcher.Invoke(() =>
                {
                    textBlockDataPoint.Text = text;
                });
                stopWatch.Restart();
            }

        }

        // when the worker for the data recording is done, the application exits
        private void recDataWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Threading.Thread.Sleep(400);
            Application.Current.Shutdown();
        }


        
    }
}


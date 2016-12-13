using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TobiiEyeX.Utils {
    class Timer {

        public int time;
        public int interval;
        public int elapsed;
        public bool isRunning;
        public bool isRepeating;
        private DispatcherTimer coreTimer;

        public delegate void tickAction(double progress);
        public delegate void elapsedAction();
        public event tickAction OnTick;
        public event elapsedAction OnElapsed;

        public Timer(int time, int interval, bool isRepeating = false) {
            this.time = time;
            this.interval = interval;
            this.isRepeating = isRepeating;
            elapsed = 0;
            isRunning = false;
            coreTimer = new DispatcherTimer(DispatcherPriority.Normal);
            coreTimer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            coreTimer.Tick += onCoreTimerTick;
        }

        private void onCoreTimerTick(object sender, EventArgs e) {
            if (elapsed < time) {
                elapsed += interval;
                double ratio = (double) elapsed / time;
                OnTick?.Invoke(ratio);
            }
            else {
                OnElapsed?.Invoke();
                if (isRepeating) {
                    elapsed = 0;
                }
                else {
                    coreTimer.Stop();
                    isRunning = false;
                }
            }
        }

        public void Start() {
            isRunning = true;
            coreTimer.Start();
        }

        public void Stop() {
            coreTimer.Stop();
            elapsed = 0;
            isRunning = false;
        }

        ~Timer() {
            if (coreTimer.IsEnabled) coreTimer.Stop();
        }
    }
}

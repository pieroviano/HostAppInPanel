using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HostAppInPanelLib.Utility
{
    public class TimedAction
    {
        private DispatcherTimer _dispatcherTimer;

        public void ExecuteWithDelay(Action action, TimeSpan delay)
        {
            _dispatcherTimer?.Stop();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = delay;
            _dispatcherTimer.Tag = action;
            _dispatcherTimer.Tick += new EventHandler(timer_Tick);
            _dispatcherTimer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            Action action = (Action)timer.Tag;

            action.Invoke();
            timer.Stop();
            _dispatcherTimer = null;
        }
    }
}

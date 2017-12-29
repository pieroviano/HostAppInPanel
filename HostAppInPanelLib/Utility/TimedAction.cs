using System;
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
            _dispatcherTimer.Tick += timer_Tick;
            _dispatcherTimer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var timer = (DispatcherTimer) sender;
            var action = (Action) timer.Tag;

            action.Invoke();
            timer.Stop();
            _dispatcherTimer = null;
        }
    }
}
using System;
using System.Diagnostics;
using System.Threading;

namespace HostAppInPanelLib.Utility
{
    internal class ProcessExitWaiter
    {
        private readonly Process _process;
        public EventHandler ProcessExited;

        public ProcessExitWaiter(Process process)
        {
            _process = process;
        }

        public void WaitProcessExit()
        {
            void Start()
            {
                try
                {
                    _process.WaitForExit();
                    ProcessExited?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            var thread = new Thread(Start);
            thread.Start();
        }
    }
}
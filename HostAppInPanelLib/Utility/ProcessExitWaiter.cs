using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HostAppInPanelLib.Utility
{
    class ProcessExitWaiter
    {
        private Process _process;
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

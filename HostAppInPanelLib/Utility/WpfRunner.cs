using System;
using System.Windows;

namespace HostAppInPanelLib.Utility
{
    public class WpfRunner
    {
        public readonly RoutedEventHandler WindowOnLoaded;
        private Type _type;

        public WpfRunner(Type type, RoutedEventHandler windowOnOnLoaded = null)
        {
            WindowOnLoaded = windowOnOnLoaded;
            WrapperWindow = (Window) type.GetConstructor(new Type[0])?.Invoke(new object[0]);
        }

        public Window WrapperWindow { get; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var wpfRunner = this;
            if (wpfRunner.WrapperWindow != null)
            {
                if (wpfRunner.WindowOnLoaded != null)
                {
                    wpfRunner.WrapperWindow.Loaded += wpfRunner.WindowOnLoaded;
                }
                wpfRunner.WrapperWindow.Show();
            }
        }

        public void RunWpfFromMain()
        {
            var application = new Application();
            application.Startup += Application_Startup;
            application.Run();
        }
    }
}
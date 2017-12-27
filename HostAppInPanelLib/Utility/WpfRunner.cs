using System;
using System.Windows;

namespace HostAppInPanelLib.Utility
{
    public class WpfRunner
    {
        private readonly RoutedEventHandler _windowLoaded;
        private Type _type;

        public WpfRunner(Type type, RoutedEventHandler windowOnLoaded = null)
        {
            _windowLoaded = windowOnLoaded;
            WrapperWindow = (Window) type.GetConstructor(new Type[0])?.Invoke(new object[0]);
        }

        public Window WrapperWindow { get; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var wpfRunner = this;
            if (wpfRunner.WrapperWindow != null)
            {
                if (wpfRunner._windowLoaded != null)
                {
                    wpfRunner.WrapperWindow.Loaded += wpfRunner._windowLoaded;
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
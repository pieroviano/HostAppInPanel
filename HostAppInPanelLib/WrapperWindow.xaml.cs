using System.Windows;
using HostAppInPanelLib.Controls;

namespace HostAppInPanelLib
{
    /// <summary>
    ///     Interaction logic for WrapperWindow.xaml
    /// </summary>
    public partial class WrapperWindow
    {
        public WrapperWindow(string fileName)
        {
            InitializeComponent();
            if (WrapperControl == null)
            {
                WrapperControl = new WrapperControl
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    ProcessPath = fileName
                };
            }
            else
            {
                WrapperControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                WrapperControl.VerticalAlignment = VerticalAlignment.Stretch;
                WrapperControl.ProcessPath = fileName;
            }

            Grid.Children.Add(WrapperControl);
        }

        public WrapperWindow()
        {
            InitializeComponent();
            if (WrapperControl == null)
            {
                WrapperControl = new WrapperControl
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
            }
            else
            {
                WrapperControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                WrapperControl.VerticalAlignment = VerticalAlignment.Stretch;
            }

            Grid.Children.Add(WrapperControl);
        }

        public string ProcessPath
        {
            get => WrapperControl.ProcessPath;
            set => WrapperControl.ProcessPath = value;
        }


        public WrapperControl WrapperControl { get; }
    }
}
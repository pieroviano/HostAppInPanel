using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using HostAppInPanelLib.Utility;
using HostAppInPanelLib.Utility.Win32;
using OpenQA.Selenium;
using WpfAdornedControl.WpfControls.Extensions;
using Panel = System.Windows.Forms.Panel;

namespace HostAppInPanelLib
{
    /// <summary>
    ///     Interaction logic for WrapperWindow.xaml
    /// </summary>
    public partial class WrapperWindow : Window
    {
        public WrapperWindow()
        {
            InitializeComponent();
            WrapperControl = new WrapperControl();
            WrapperControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            WrapperControl.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.Children.Add(WrapperControl);
        }

        public WrapperControl WrapperControl { get; }

    }
}
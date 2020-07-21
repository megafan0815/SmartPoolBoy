using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.I2c;
using Windows.Devices.Enumeration;
using SmartPoolBoy.Konfiguration;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartPoolBoy.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btModuleScannen_Click(object sender, RoutedEventArgs e)
        {
            tbScannedDevices.Text = "";
            var moduleScanner = new ModuleScanner();
            foreach (var di in moduleScanner.ScanForI2CDevices())
            {
                tbScannedDevices.Text += $"\nDevice-Addresse='{di.DeviceSlaveAddress}', Device-Description='{di.DeviceDescription}'";
            }
        }
    }
}

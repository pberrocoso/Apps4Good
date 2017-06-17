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

namespace Sidercar.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Xamarin.FormsMaps.Init("22wqUiekHrMcGVD5gnOG~rGfo7oHId1_QqE0tgSeqEA~ApkKrArqOBsH1RO0zJH8-4lUY9GPyb8kukrVnuoNq6qlJIdZ2h43_YepRypx4WBJ");
            LoadApplication(new Sidercar.App());
        }
    }
}

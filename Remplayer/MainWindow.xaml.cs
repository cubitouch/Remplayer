using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;

using Nancy.Hosting.Self;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Remplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Service.ViewModel _viewModel = new Service.ViewModel();
        private NancyHost nancyHost;

        public MainWindow()
        {
            InitializeComponent();
            base.DataContext = _viewModel;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (nancyHost == null)
            {
                // create a new self-host server
                nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:8080"));
            }
            else
            {
                // stop
                nancyHost.Stop();
            }
            // start
            nancyHost.Start();

            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            _viewModel.Server = localIP + ":8080";

            //System.Windows.Forms.Panel thisPanel2 = new System.Windows.Forms.Panel();
            //windowsFormsHost1.Child = thisPanel2;

            //ProcessStartInfo info = new ProcessStartInfo("C:\\Users\\Hugo\\Downloads\\ffmpeg\\ffmpeg-20131018-git-f6b56b1-win32-static\\bin\\ffplay.exe", "-v quiet -i \"D:/Misfits S02/Misfits - 2x04 - Episode 4.HDTV.bia.fr_PC_PC.avi\"");
            //Process PR = Process.Start(info);
            ////PR.WaitForInputIdle(4000);
            ////SetParent(PR.MainWindowHandle, thisPanel2.Handle);
            ////PR.WaitForExit();
            //System.Threading.Thread.Sleep(1000);
            //ShowWindow((int)PR.MainWindowHandle, SW_MAXIMIZE);

            ////Process[] test = Process.GetProcessesByName("ffplay");

            ////test[0].WaitForInputIdle();
            ////test[0].MainModule.
            ////SetParent(test[0].MainWindowHandle, thisPanel2.Handle);
            ////ShowWindow((int)PR.MainWindowHandle, SW_HIDE);
            ////test[0].StandardInput.Write("p");
            ////test[0].Container.Components.

        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr SetParent(IntPtr HWNDChild, IntPtr HWNDNewParent);

        const int SW_HIDE = 0;
        const int SW_RESTORE = 1;
        const int SW_MINIMIZE = 2;
        const int SW_MAXIMIZE = 3;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int ShowWindow(int hwnd, int nCmdShow);

    }

    // netsh http add urlacl url=http://localhost:8080/ User="Tout le monde"
    public class Bootstrapper : Nancy.DefaultNancyBootstrapper
    {
        protected virtual Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return Nancy.Bootstrapper.NancyInternalConfiguration.Default;
            }
        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(
            Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("foundation", @"foundation")
        );
        }
    }

}

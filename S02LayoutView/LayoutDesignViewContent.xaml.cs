using System.Diagnostics.CodeAnalysis;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Diagnostics;

namespace S02LayoutView
{
    /// <summary>
    /// Interaction logic for MyToolWindowControl.
    /// </summary>
    public partial class LayoutDesignViewContent : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="S02FirstWindow.LayoutDesignViewContent"/> class.
        /// </summary>
        public LayoutDesignViewContent()
        {
            this.InitializeComponent();
        }

        private Process process = null;

        private void LaunchLayoutDesignViewer(object sender, EventArgs e)
        {
            System.Console.WriteLine("LaunchLayoutDesignViewer:  called.");

            Window layoutWindow = Window.GetWindow(borderLayoutView);
            //IntPtr hWnd = new WindowInteropHelper(layoutWindow).EnsureHandle();
            IntPtr hWnd = new WindowInteropHelper(layoutWindow).Handle;

            string viewExecutablePath = "E:\\Work\\VSSDK.Study\\S02LayoutView\\SampleHelloWorldBuild\\SampleHelloWorld.exe";
            //string commandArguments = String.Format("-parentHWND {0:d} delayed", hWnd);
            string commandArguments = String.Format("-parentHWND {0}", hWnd);

            process = new Process();
            process.StartInfo.FileName = viewExecutablePath;
            process.StartInfo.Arguments = commandArguments;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForInputIdle();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //LaunchLayoutDesignViewer(this, e);

            string viewExecutablePath = "E:\\Work\\VSSDK.Study\\S02LayoutView\\SampleHelloWorldBuild\\SampleHelloWorld.exe";
            borderLayoutView.Child = new UnityHWndHost(viewExecutablePath, "");
        }
    }
}

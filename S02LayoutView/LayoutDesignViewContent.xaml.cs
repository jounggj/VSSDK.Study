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

            rectLayoutDesign.Initialized += LaunchLayoutDesignViewer;

            //LaunchLayoutDesignViewer();
        }

        private void LaunchLayoutDesignViewer(object sender, EventArgs e)
        {
            System.Console.WriteLine("LaunchLayoutDesignViewer:  called.");

            Window layoutWindow = Window.GetWindow(rectLayoutDesign);
            IntPtr hWnd = new WindowInteropHelper(layoutWindow).EnsureHandle();

            string viewExecutablePath = "E:\\Work\\VSSDK.Study\\S02LayoutView\\SampleHelloWorldBuild\\SampleHelloWorld.exe";
            string commandArguments = String.Format("-parentHWND %d delayed", hWnd);
            //Process.Start(viewExecutablePath + " " + commandArguments);
            Process.Start(viewExecutablePath);
        }
    }
}
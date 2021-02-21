using System.Diagnostics.CodeAnalysis;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Diagnostics;
using Palantiri.IO;
using Palantiri.Debug;

namespace S02LayoutView
{
    /// <summary>
    /// Interaction logic for MyToolWindowControl.
    /// </summary>
    public partial class LayoutDesignViewContent : UserControl
    {
        private const string TAG = "LayoutViewer";
        private UnityHWndHost unityWindowHost = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="S02FirstWindow.LayoutDesignViewContent"/> class.
        /// </summary>
        public LayoutDesignViewContent()
        {
            this.InitializeComponent();

            Log.Open(Log.LogTarget.File, "E:\\vsix-view.log");
            Log.D(TAG, "LayoutDesignViewContent:  called.");

            string viewExecutablePath = "E:\\Work\\VSSDK.Study\\S02LayoutView\\LayoutViewer\\LayoutViewer.exe";
            //unityWindowHost = new UnityHWndHost(viewExecutablePath, "");
            //borderLayoutView.Child = unityWindowHost;
            //Log.D(TAG, "--  after  adding new UnityHWndHost()");
        }

        public void TerminateLayoutViewer()
        {
            unityWindowHost.TerminateLayoutViewer();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
        }

        private void borderLayoutView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Log.I(TAG, "borderLayoutView_IsVisibleChanged:  called.");
        }

        private void borderLayoutView_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void borderLayoutView_Initialized(object sender, EventArgs e)
        {

        }

        private void borderLayoutView_LayoutUpdated(object sender, EventArgs e)
        {

        }
    }
}

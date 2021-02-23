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
            unityWindowHost = new UnityHWndHost(viewExecutablePath, "");
            borderLayoutView.Child = unityWindowHost;
            Log.D(TAG, "--  after  adding new UnityHWndHost()");
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
            Border border = borderLayoutView;
            unityWindowHost?.WriteLine(String.Format("IsVisible {0}",
                border.IsVisible.ToString()));
            //Log.I(TAG, "--  border.IsVisible = " + border.IsVisible.ToString());
        }

        private void borderLayoutView_GotFocus(object sender, RoutedEventArgs e)
        {
            Log.I(TAG, "borderLayoutView_GotFocus:  called.");
            Border border = borderLayoutView;
            unityWindowHost?.WriteLine(String.Format("IsFocused {0}",
                border.IsFocused.ToString()));
            //Log.I(TAG, "--  border.IsFocused = " + border.IsFocused.ToString());
        }

        private void borderLayoutView_Initialized(object sender, EventArgs e)
        {
            Log.I(TAG, "borderLayoutView_Initialized:  called.");
            Border border = borderLayoutView;
            unityWindowHost?.WriteLine(String.Format("IsInitialized {0}",
                border.IsInitialized.ToString()));
            //Log.I(TAG, "--  border.IsInitialized = " + border.IsInitialized.ToString());
        }

        private void borderLayoutView_LayoutUpdated(object sender, EventArgs e)
        {
            Log.I(TAG, "borderLayoutView_LayoutUpdated:  called.");
            Border border = borderLayoutView;
            unityWindowHost?.WriteLine(String.Format("ActualSize {0} {1}",
                border.ActualWidth, border.ActualHeight));
            //Log.I(TAG, "--  border.ActualWidth  = " + border.ActualWidth);
            //Log.I(TAG, "--  border.ActualHeight = " + border.ActualHeight);
        }

        private void borderLayoutView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Log.I(TAG, "borderLayoutView_SizeChanged:  called.");
            Border border = borderLayoutView;
            unityWindowHost?.WriteLine(String.Format("SizeChanged {0} {1}",
                e.NewSize.Width, e.NewSize.Height));
            //Log.I(TAG, "--  e.PreviousSize.Width    = " + e.PreviousSize.Width);
            //Log.I(TAG, "--  e.PreviousSize.Height   = " + e.PreviousSize.Height);
            //Log.I(TAG, "--  e.NewSize.Width         = " + e.NewSize.Width);
            //Log.I(TAG, "--  e.NewSize.Height        = " + e.NewSize.Height);
        }
    }
}

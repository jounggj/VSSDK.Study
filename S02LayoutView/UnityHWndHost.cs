using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Interop;
using Palantiri.IO;
using Palantiri.Debug;

namespace S02LayoutView
{
    class UnityHWndHost : HwndHost
    {
        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint processId);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")] // TODO: 32/64?
        internal static extern IntPtr GetWindowLongPtr(IntPtr hWnd, Int32 nIndex);
        internal const Int32 GWLP_USERDATA = -21;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        internal const UInt32 WM_CLOSE = 0x0010;

        private const string TAG = "UnityHWndHost";
        private string programName;
        private string arguments;

        private AnonPipeServer pipeServer = null;
        private Process clientProcess = null;
        private IntPtr unityHWND = IntPtr.Zero;

        private Timer aTimer = null;
        private int sendCount = 0;

        public UnityHWndHost(string programName, string arguments = "")
        {
            this.programName = programName;
            this.arguments = arguments;
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            Debug.WriteLine("Going to launch Unity at: " + this.programName + " " + this.arguments);
            Log.D(TAG, "BuildWindowCore:  called.");

            pipeServer = new AnonPipeServer();
            bool ret = pipeServer.Create();
            if (!ret)
            {
                Log.E(TAG, "new AnonPipeserver() failed!!");
                return new HandleRef(this, unityHWND);
            }
            Log.D(TAG, "--  after  new AnonPipeServer():  clientHandle = " + pipeServer.GetClientHandleAsString());

            Process client = new Process();
            try
            {
                string clientHandle = pipeServer.GetClientHandleAsString();
                //pipeServer.DisposeLocalCopyOfClientHandle();

                client.StartInfo.FileName = this.programName;
                client.StartInfo.Arguments = String.Format("{0} -parentHWND {1} -pipe {2}",
                    arguments, hwndParent.Handle, pipeServer.GetClientHandleAsString());
                client.StartInfo.UseShellExecute = false;
                client.StartInfo.CreateNoWindow = true;
                client.Start();
                client.WaitForInputIdle();
            }
            catch (Exception exception)
            {
                Log.E(TAG, "creating andlaunching client failed:  " + exception.Message);
                return new HandleRef(this, unityHWND);
            }
            clientProcess = client;
            Log.D(TAG, "Client launched.");

            int repeat = 50;
            while (unityHWND == IntPtr.Zero && repeat-- > 0)
            {
                Thread.Sleep(100);
                EnumChildWindows(hwndParent.Handle, WindowEnum, IntPtr.Zero);
            }
            if (unityHWND == IntPtr.Zero)
                throw new Exception("Unable to find Unity window");
            Debug.WriteLine("Found Unity window: " + unityHWND);

            repeat += 150;
            while ((GetWindowLongPtr(unityHWND, GWLP_USERDATA).ToInt32() & 1) == 0 && --repeat > 0)
            {
                Thread.Sleep(100);
                Debug.WriteLine("Waiting for Unity to initialize... " + repeat);
            }
            if (repeat == 0)
            {
                Debug.WriteLine("Timed out while waiting for Unity to initialize");
            }
            else
            {
                Debug.WriteLine("Unity initialized!");
            }

            try
            {
                aTimer = new Timer(
                    (object sender) =>
                    {
                        string message = String.Format("Message-{0}", sendCount++);
                        pipeServer?.WriteLine(message);
                        Log.D(TAG, "Message sent:  " + message);
                    },
                    this, 10000, 2000);
            }
            catch (Exception e)
            {
                Log.E(TAG, "creating Timer failed:  " + e.Message);
            }

            return new HandleRef(this, unityHWND);
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            if (unityHWND != IntPtr.Zero)
                throw new Exception("Found multiple Unity windows");
            unityHWND = hwnd;
            return 0;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Debug.WriteLine("Asking Unity to exit...");
            PostMessage(unityHWND, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            int i = 30;
            while (!clientProcess.HasExited)
            {
                if (--i < 0)
                {
                    Debug.WriteLine("Process not dead yet, killing...");
                    clientProcess.Kill();
                }
                Thread.Sleep(100);
            }
        }

        public void TerminateLayoutViewer()
        {
            pipeServer?.WriteLine("Terminate");
            clientProcess.WaitForExit();
            pipeServer?.Destroy();
            pipeServer = null;
            clientProcess.Close();
            clientProcess = null;
            Log.D(TAG, "Terminated client.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Idea.Helpers
{
    public static class WindowHelper
    {
       
        public static void BringProcessToFront(Process[] process)
        {
            IntPtr handle = (IntPtr)null;
            for (int i = 0; i < process.Length; i++)
                handle = process[i].MainWindowHandle;

            if (IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }

            SetForegroundWindow(handle);
        }

        const int SW_RESTORE = 9;

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
    }
}

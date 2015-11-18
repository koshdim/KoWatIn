using mshtml;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WatiN.Core.Native.Windows;

namespace WatiN.Core.Native.EmbeddedInternetExplorer
{
    internal class EmbeddedIEHelper
    {
        private static bool EnumWindows(IntPtr hwnd, IntPtr lparam)
        {
            int iRet = 1;
            StringBuilder classname = new StringBuilder(128);
            NativeMethods.GetClassName(hwnd, classname, classname.Capacity);
            if (string.Compare(classname.ToString(), "Internet Explorer_Server") == 0)
            {
                lparam = hwnd;
                iRet = 0;
            }
            return iRet == 0;
        }

        public static IHTMLDocument2 GetIEDocumentFromWindowHandle(IntPtr hWnd)
        {
            IHTMLDocument2 htmlDocument = null;
            if (hWnd != IntPtr.Zero)
            {
                int lMsg = NativeMethods.RegisterWindowMessage("WM_HTML_GETOBJECT");
                int lResult = 0;
                NativeMethods.SendMessageTimeout(hWnd, lMsg, 0, 0, NativeMethods.SMTO_ABORTIFHUNG, 1000, ref lResult);
                if (lResult != 0)
                {
                    htmlDocument = NativeMethods.ObjectFromLresult(lResult, typeof(IHTMLDocument).GUID, IntPtr.Zero) as IHTMLDocument2;
                    if (htmlDocument == null)
                    {
                        throw new COMException("Unable to cast to an object of type IHTMLDocument");
                    }
                }
            }
            return htmlDocument;
        }

        public static IntPtr GetHWnd(string applicationProcessName)
        {
            var processes = Process.GetProcessesByName(applicationProcessName);
            IntPtr hWnd = processes.First().MainWindowHandle;

            NativeMethods.EnumChildWindows(hWnd, new NativeMethods.EnumChildWindowProc(EnumWindows), hWnd);
            return hWnd;
        }

        public static IHTMLDocument2 GetIEDoc(IntPtr hwnd)
        {
            return GetIEDocumentFromWindowHandle(hwnd);
        }
    }
}

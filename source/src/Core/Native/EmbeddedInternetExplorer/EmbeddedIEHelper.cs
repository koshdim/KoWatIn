using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WatiN.Core.Native.EmbeddedInternetExplorer
{
    class EmbeddedIEHelper
    {
        public delegate int Win32Callback(IntPtr hwnd, ref IntPtr lParam);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, ref IntPtr lParam);

        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr h, StringBuilder s, int nMaxCount);

        [DllImport("User32.Dll")]
        public static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out UIntPtr result);

        [DllImport("oleacc.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        static extern object ObjectFromLresult(UIntPtr lResult, [MarshalAs(UnmanagedType.LPStruct)] Guid refiid, IntPtr wParam);

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct UUID
        {
            public long Data1;
            public int Data2;
            public int Data3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] Data4;
        }

        private static int EnumWindows(IntPtr hwnd, ref IntPtr lparam)
        {
            int iRet = 1;
            StringBuilder classname = new StringBuilder(128);
            GetClassName(hwnd, classname, classname.Capacity);
            if (string.Compare(classname.ToString(), "Internet Explorer_Server") == 0)
            {
                lparam = hwnd;
                iRet = 0;
            }
            return iRet;
        }

        public static IHTMLDocument2 GetIEDocumentFromWindowHandle(IntPtr hWnd)
        {
            IHTMLDocument2 htmlDocument = null;
            if (hWnd != IntPtr.Zero)
            {
                uint lMsg = RegisterWindowMessage("WM_HTML_GETOBJECT");
                UIntPtr lResult;
                SendMessageTimeout(hWnd, lMsg, IntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out lResult);
                if (lResult != UIntPtr.Zero)
                {
                    htmlDocument = ObjectFromLresult(lResult, typeof(IHTMLDocument).GUID, IntPtr.Zero) as IHTMLDocument2;
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

            var proc = new Win32Callback(EnumWindows);
            EnumChildWindows(hWnd, proc, ref hWnd);
            return hWnd;
        }

        public static IHTMLDocument2 GetIEDoc(IntPtr hwnd)
        {
            return GetIEDocumentFromWindowHandle(hwnd);
        }
    }
}

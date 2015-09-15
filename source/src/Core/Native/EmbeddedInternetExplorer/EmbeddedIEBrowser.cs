using System;
using WatiN.Core.Native.InternetExplorer;

namespace WatiN.Core.Native.EmbeddedInternetExplorer
{
    internal class EmbeddedIEBrowser : INativeBrowser
    {
        private IEDocument nativeDocument;

        public void NavigateTo(Uri url)
        {
        }

        public void NavigateToNoWait(Uri url)
        {
        }

        public bool GoBack()
        {
            return true;
        }

        public bool GoForward()
        {
            return true;
        }

        public void Reopen()
        {
        }

        public void Refresh()
        {
        }

        public IntPtr hWnd
        {
            get { return EmbeddedIEHelper.GetHWndOutlook(); }
        }

        public INativeDocument NativeDocument
        {
            get
            {
                return nativeDocument ?? (nativeDocument = new IEDocument(EmbeddedIEHelper.GetIEDoc()));
            }
        }
    }
}

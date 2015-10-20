using System;
using WatiN.Core.Native.InternetExplorer;

namespace WatiN.Core.Native.EmbeddedInternetExplorer
{
    internal class EmbeddedIEBrowser : INativeBrowser
    {
        private IEDocument nativeDocument;
        private string _applicationProcessName;

        public EmbeddedIEBrowser(string applicationProcessName)
        {
            if (string.IsNullOrEmpty(applicationProcessName))
                throw new ArgumentNullException("applicationProcessName");
            this._applicationProcessName = applicationProcessName;
        }

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
            get { return EmbeddedIEHelper.GetHWnd(_applicationProcessName); }
        }

        public INativeDocument NativeDocument
        {
            get
            {
                return nativeDocument ?? (nativeDocument = new IEDocument(EmbeddedIEHelper.GetIEDoc(hWnd)));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core.Native.EmbeddedInternetExplorer;

namespace WatiN.Core
{
    public class EmbededIE : Browser
    {
        private string _applicationProcessName;
        private Native.INativeBrowser _nativeBrowser;

        public EmbededIE(string applicationProcessName)
            : base()
        {
            if (string.IsNullOrEmpty(applicationProcessName))
                throw new ArgumentNullException("applicationProcessName");
            _applicationProcessName = applicationProcessName;
        }

        public override Native.INativeBrowser NativeBrowser
        {
            get 
            {
                return _nativeBrowser ?? (_nativeBrowser = new EmbeddedIEBrowser(_applicationProcessName));
            }
        }

        public override void Close()
        {
            //it is supposed that the browser is closed together with host application, which is managed outside of this class
        }

        public override void WaitForComplete(int waitForCompleteTimeOut)
        {
        }
    }
}

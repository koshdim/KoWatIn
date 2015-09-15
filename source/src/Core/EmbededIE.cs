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

        public EmbededIE(string applicationProcessName)
            : base()
        {
            _applicationProcessName = applicationProcessName;
        }

        public override Native.INativeBrowser NativeBrowser
        {
            get { return new EmbeddedIEBrowser(); }
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void WaitForComplete(int waitForCompleteTimeOut)
        {
            throw new NotImplementedException();
        }
    }
}

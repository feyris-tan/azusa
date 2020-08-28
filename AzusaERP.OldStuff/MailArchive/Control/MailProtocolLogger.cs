using System;
using System.Diagnostics;
using System.Text;
using AzusaERP.OldStuff;

namespace moe.yo3explorer.azusa.Control.MailArchive.Control
{
    class MailProtocolLogger : IProtocolLogger
    {
        public MailProtocolLogger()
        {
            context = AzusaContext.GetInstance();
        }

        AzusaContext context;

        public void Dispose()
        {
            SetMessage("Dispose Mail-Log");
        }

        public void LogClient(byte[] buffer, int offset, int count)
        {
            SetMessage(Encoding.UTF8.GetString(buffer, offset, count));
        }

        public void LogConnect(Uri uri)
        {
            SetMessage("Connecting to: {0}", uri);
        }

        public void LogServer(byte[] buffer, int offset, int count)
        {
            //SetMessage(Encoding.UTF8.GetString(buffer, offset, count));
        }

        private void SetMessage(string s,params object[] args)
        {
            string msg = String.Format(s, args);
            context.Splash.SetLabel(msg);
            Debug.WriteLine(msg);
        }
    }
}

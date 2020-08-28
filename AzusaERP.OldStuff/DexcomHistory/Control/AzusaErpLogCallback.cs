using AzusaERP.OldStuff;

namespace moe.yo3explorer.azusa.DexcomHistory.Control
{
    class AzusaErpLogCallback : moe.yo3explorer.azusa.dex.ILogCallback
    {
        public AzusaErpLogCallback()
        {
            form = AzusaContext.GetInstance().MainForm;
        }

        private MainForm form;

        public void LogEvent(string s)
        {
            form.SetStatusBar(s);
        }
    }
}

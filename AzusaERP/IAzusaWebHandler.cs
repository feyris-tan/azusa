using System.Net;

namespace moe.yo3explorer.azusa
{
    interface IAzusaWebHandler
    {
        bool CanHandle(string url);
        void Handle(HttpListenerContext context);
    }
}

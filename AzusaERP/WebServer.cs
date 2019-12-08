using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace moe.yo3explorer.azusa
{
    public class WebServer
    {
        public WebServer()
        {
            context = AzusaContext.GetInstance();
            handlers = new List<IAzusaWebHandler>();
            int port = AzusaContext.FindFreePort();
            Prefix = String.Format("http://localhost:{0}/", port);
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(Prefix);
            httpListener.Start();
            thread = new Thread(ListenerThread);
            thread.Priority = ThreadPriority.Lowest;
            thread.Name = "Internal Webserver";
            thread.Start();
        }

        private void ListenerThread()
        {
            Debug.WriteLine(String.Format("Webserver listening on: {0}", Prefix));
            IsRunning = true;
            while (IsRunning)
            {
                var listenerContext = httpListener.GetContext();
                string url = listenerContext.Request.RawUrl;
                if (url.EndsWith("/stop.elf"))
                {
                    IsRunning = false;
                    SendString(listenerContext.Response, "See you later!");
                    Thread.Sleep(100);
                    httpListener.Stop();
                }
                else if (url.EndsWith("/ping.elf"))
                {
                    SendString(listenerContext.Response, "pong");
                }
                else if (url.EndsWith("/favicon.ico"))
                {
                    listenerContext.Response.ContentType = "image/x-icon";
                    context.Icon.Save(listenerContext.Response.OutputStream);
                    listenerContext.Response.OutputStream.Flush();
                    listenerContext.Response.Close();
                }
                else
                {
                    IAzusaWebHandler handler = handlers.FirstOrDefault(x => x.CanHandle(url));
                    if (handler != null)
                    {
                        handler.Handle(listenerContext);
                        continue;
                    }
                    else
                    {
                        listenerContext.Response.StatusCode = 404;
                        listenerContext.Response.Close();
                    }
                }
            }
        }

        private void SendString(HttpListenerResponse response, string value, params object[] args)
        {
            response.ContentEncoding = Encoding.UTF8;
            response.StatusCode = 200;
            StreamWriter sw = new StreamWriter(response.OutputStream, Encoding.UTF8);
            sw.Write(String.Format(value, args));
            sw.Flush();
            response.Close();
        }

        private List<IAzusaWebHandler> handlers;
        private AzusaContext context;
        private HttpListener httpListener;
        private Thread thread;
        public string Prefix { get; private set; }

        public bool IsRunning{ get; private set; }
    }
}

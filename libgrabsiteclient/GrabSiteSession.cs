using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ehentaiDumper.JsonThingies;
using Newtonsoft.Json;

namespace libgrabsiteclient
{
    public class GrabSiteSession
    {
        public GrabSiteSession()
        {
            grabSiteMessage = new GrabSiteMessage();
        }

        public GrabSiteJobData messageData => grabSiteMessage.job_data;

        private ClientWebSocket webSocket;
        private GrabSiteMessage grabSiteMessage;

        public void SendGrabsiteMessage(string msg)
        {
            if (webSocket == null)
            {
                dynamic m = new ExpandoObject();
                m.type = "hello";
                m.mode = "grabber";
                m.url = "Some AzusaERP Job";
                string helloMessageJson = JsonConvert.SerializeObject(m);
                byte[] helloMessageAsBytes = Encoding.UTF8.GetBytes(helloMessageJson);
                ArraySegment<byte> helloMessageSegment = new ArraySegment<byte>(helloMessageAsBytes);
                
                CancellationToken ct1 = new CancellationToken(false);
                webSocket = new ClientWebSocket();
                webSocket.ConnectAsync(new Uri("ws://172.20.20.31:29000"), ct1).Wait();
                webSocket.SendAsync(helloMessageSegment, WebSocketMessageType.Text, true, ct1);
            }

            CancellationToken ct2 = new CancellationToken(false);
            grabSiteMessage.message = msg;
            webSocket.SendAsync(grabSiteMessage.toArraySegment(), WebSocketMessageType.Text, true, ct2);
        }

    }
}

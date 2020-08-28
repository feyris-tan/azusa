using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzusaERP.OldStuff
{
    class StreamDevice : IDisposable
    {
        private Stream nmeaStream;

        public StreamDevice(Stream nmeaStream)
        {
            this.nmeaStream = nmeaStream;
        }

        public event MessageReceivedDelegate MessageReceived;
        public bool IsOpen { get; set; }

        public Task CloseAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            nmeaStream?.Dispose();
        }

        public Task OpenAsync()
        {
            throw new NotImplementedException();
        }
    }

    public delegate void MessageReceivedDelegate(object sender, NmeaMessageReceivedEventArgs e);

    public class NmeaMessageReceivedEventArgs
    {
        public NmeaMessage Message { get; set; }
    }

    public class NmeaMessage
    {
        public string MessageType { get; set; }
    }

    class Gpgsv : NmeaMessage
    {
        public int SVsInView { get; set; }
    }

    class Gpgll : NmeaMessage
    {
        public bool DataActive { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    class Gpgsa : NmeaMessage
    {
        public Mode FixMode { get; set; }

        internal enum Mode
        {

        }
    }


    class Gprmc : NmeaMessage
    {

    }

    class Gpgga : NmeaMessage
    {
        public double Altitude { get; set; }
    }
}

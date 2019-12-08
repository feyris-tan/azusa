using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace moe.yo3explorer.azusa.Control.Setup
{
    [Serializable]
    public class StartupFailedException : Exception
    {

        protected StartupFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        private StartupFailReason sfr;

        public StartupFailedException(StartupFailReason sfr)
        {
            this.sfr = sfr;
        }

        public StartupFailedException(Exception innerException, StartupFailReason sfr) : 
            base(sfr.ToString(), innerException)
        {
            this.sfr = sfr;
        }
    }

    public enum StartupFailReason
    {
        PrepareRunReturnedTrue,
        AzusaIniNotFound,
        NoDatabaseAvailable,
        IniBroken,
        LicenseNotValid
    }
}

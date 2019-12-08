using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.Control.Licensing
{

    internal class LicenseValidationFailedException : Exception
    {
        private LicenseState state;
        public LicenseValidationFailedException(LicenseState licenseState)
        {
            state = licenseState;
        }

        public LicenseState LicensingState
        {
            get => state;
        }

        public override string Message
        {
            get
            {
                switch (state)
                {
                    default:
                        return String.Format("???:" + state.ToString());
                }
            }
        }
    }

}

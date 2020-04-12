using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    [Serializable]
    public class SnapraidContentException : ApplicationException
    {

        public SnapraidContentException(SnapraidContentFailureReason reason)
        {
            this.Reason = reason;
        }

        public SnapraidContentFailureReason Reason { get; private set; }

        protected SnapraidContentException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

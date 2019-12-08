using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace retrodb4net
{

    [Serializable]
    public class RetroDbException : Exception
    {
        public RetroDbException() { }
        public RetroDbException(string message) : base(message) { }
        public RetroDbException(string message, Exception inner) : base(message, inner) { }
        protected RetroDbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

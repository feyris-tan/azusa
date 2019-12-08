using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamdump
{

    [Serializable]
    public class SteamdumpException : Exception
    {
        public SteamdumpException() { }
        public SteamdumpException(string message) : base(message) { }
        public SteamdumpException(string message, Exception inner) : base(message, inner) { }
        protected SteamdumpException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class FileAlreadyExistsException : SteamdumpException
    {
        public FileAlreadyExistsException() { }
        public FileAlreadyExistsException(string message) : base(message) { }
        public FileAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected FileAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

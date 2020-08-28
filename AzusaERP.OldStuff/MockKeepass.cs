using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AzusaERP.OldStuff
{
    class PwDatabase
    {
        public void Open(IOConnectionInfo connectionInfo, CompositeKey compositeKey, object o)
        {
            throw new NotImplementedException();
        }

        public PwGroup RootGroup { get; set; }
    }

    class PwGroup
    {
        public IEnumerable<PwEntry> Entries { get; set; }
        public IEnumerable<PwGroup> Groups { get; set; }
        public string Name { get; set; }
    }

    class IOConnectionInfo
    {
        public static IOConnectionInfo FromPath(string fullName)
        {
            throw new NotImplementedException();
        }
    }

    class KcpPassword
    {
        private string pw;

        public KcpPassword(string pw)
        {
            this.pw = pw;
        }
    }

    class CompositeKey
    {
        public void AddUserKey(KcpPassword kcpPassword)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class InvalidCompositeKeyException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidCompositeKeyException()
        {
        }

        public InvalidCompositeKeyException(string message) : base(message)
        {
        }

        public InvalidCompositeKeyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidCompositeKeyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    class PwEntry
    {
        public PwStringTable Strings { get; set; }
    }

    class PwStringTable
    {
        public PwString Get(string username)
        {
            throw new NotImplementedException();
        }
    }

    class PwString
    {
        public string ReadString()
        {
            throw new NotImplementedException();
        }
    }
}

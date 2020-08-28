using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using moe.yo3explorer.azusa.Control.MailArchive.Control;

namespace AzusaERP.OldStuff
{
    enum FolderAccess
    {
        ReadOnly
    }

    class ImapClient
    {
        public ImapClient(MailProtocolLogger mailProtocolLogger)
        {
            throw new NotImplementedException();
        }

        public ServerCertificateValidation ServerCertificateValidationCallback { get; set; }
        public SslProtocols SslProtocols { get; set; }
        public List<string> AuthenticationMechanisms { get; set; }
        public FolderNamespace[] PersonalNamespaces { get; set; }

        public void Connect(string server, int port, bool useSsl)
        {
            throw new NotImplementedException();
        }

        public void Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public IList<IMailFolder> GetFolders(FolderNamespace rootFolderNamespace)
        {
            throw new NotImplementedException();
        }

        public void Disconnect(bool b)
        {
            throw new NotImplementedException();
        }
    }

    class FolderNamespace
    {

    }

    interface IMailFolder
    {
        string Name { get; set; }
        IMailFolder ParentFolder { get; set; }
        string FullName { get; set; }
        void Open(FolderAccess readOnly);
        IList<UniqueId> Search(SearchQuery getSearchQuery);
        Message GetMessage(UniqueId uuid);
        void Close(bool b);
    }

    class Message
    {
        public void WriteTo(CryptoStream cryptoStream)
        {
            throw new NotImplementedException();
        }

        public Date Date { get; set; }
        public Participant[] From { get; set; }
        public List<Participant> To { get; set; }
        public string Subject { get; set; }
    }

    class Participant
    {
        
    }

    class Date
    {
        public DateTime DateTime { get; set; }
    }

    class SearchQuery
    {
        public static SearchQuery All { get; set; }

        public static SearchQuery DeliveredAfter(DateTime fromUnixTime)
        {
            throw new NotImplementedException();
        }
    }
    

    delegate bool ServerCertificateValidation(object sender, object certificate, object chain, object errors);

    [Flags]
    enum MessageSummaryItems
    {
        GMailMessageId,
        Id,
        UniqueId,
        Envelope
    }

    class UniqueId
    {
        public int Id { get; set; }
    }

    interface IProtocolLogger
    {
        
    }
}

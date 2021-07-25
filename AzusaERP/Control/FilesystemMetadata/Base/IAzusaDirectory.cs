using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.Control.FilesystemMetadata.Base
{
    interface IAzusaFsDirectory
    {
        string FullName { get; }
        DateTime LastModified { get; }
        IReadOnlyCollection<IAzusaFsDirectory> GetSubdirectories();
        IReadOnlyCollection<IAzusaFile> GetFiles();
    }
}

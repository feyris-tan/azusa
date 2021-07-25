using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.Control.FilesystemMetadata.Base
{
    interface IAzusaFile
    {
        string FullName { get; }
        DateTime LastModified { get; }
        long Length { get; }
        Stream OpenRead();
    }
}

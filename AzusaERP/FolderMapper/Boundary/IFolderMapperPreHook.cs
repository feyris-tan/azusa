using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libazuworker;
using moe.yo3explorer.ryuuguuKomachi.DicModBridge;

namespace moe.yo3explorer.azusa.FolderMapper.Boundary
{
    public interface IFolderMapperPreHook
    {
        void Call(MapperWorker mapperWorker, WorkerForm workerForm, FileExtensionDictionary files);
    }
}

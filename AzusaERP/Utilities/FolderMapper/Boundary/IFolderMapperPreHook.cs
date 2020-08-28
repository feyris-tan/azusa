using libazuworker;
using moe.yo3explorer.azusa.Utilities.FolderMapper.Control;

namespace moe.yo3explorer.azusa.Utilities.FolderMapper.Boundary
{
    public interface IFolderMapperPreHook
    {
        void Call(MapperWorker mapperWorker, WorkerForm workerForm, FileExtensionDictionary files);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class UpdateListEntryLinked
    {
        public string catalog;
        public string link;

        public int GetId()
        {
            string[] args = link.Split('/');
            return System.Convert.ToInt32(args[1]);
        }

        public UpdateType GetUpdateType()
        {
            if (link.StartsWith("album/"))
                return UpdateType.Album;
            else
                throw new NotImplementedException(link);
        }

        public object Convert()
        {
            switch (GetUpdateType())
            {
                case UpdateType.Album:
                    AlbumListAlbum albumListAlbum = new AlbumListAlbum();
                    albumListAlbum.catalog = catalog;
                    albumListAlbum.link = link;
                    return albumListAlbum;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

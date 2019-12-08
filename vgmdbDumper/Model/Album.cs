using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class Album
    {
        public ArtistListArtist[] arrangers;
        public string catalog;
        public string classification;
        public ArtistListArtist[] composers;
        public AlbumCover[] covers;
        public AlbumDisc[] discs;
        public string link;
        public ArtistListArtist[] lyricists;
        public string media_format;
        public AlbumMeta meta;
        public string name;
        public Dictionary<string, string> names;
        public string notes;
        public AlbumOrganization[] organizations;
        public ArtistListArtist[] performers;
        public string picture_full;
        public string picture_small;
        public string picture_thumb;
        public string publish_format;
        public LabelListLabel publisher;
        public double rating;
        public AlbumListAlbum[] related;
        public string release_date;
        public EventListEvent[] release_events;
        public AlbumReleasePrice release_price;
        public AlbumListAlbum[] reprints;
        public string vgmdb_link;
        public int votes;
        public ProductListProduct[] products;
        public Website[] stores;
        public Dictionary<string, Website[]> websites;


        public int GetId()
        {
            if (link.StartsWith("album/"))
            {
                return Convert.ToInt32(link.Substring(6));
            }
            throw new NotSupportedException(String.Format("can't get album id from: " + link));
        }
    }
}

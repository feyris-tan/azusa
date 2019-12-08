using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class UpdateListEntry
    {
        public string caption;
        public string catalog;
        public string category;
        public UpdateListEntryContributor contributor;
        public DateTime date;
        public string edit;
        public string image;
        public string link;
        public UpdateListEntryLinkData link_data;
        public string link_type;
        public UpdateListEntryLinked linked;
        public string media_format;
        public Dictionary<string, string> names;
        public bool @new;
        public string rating;
        public string release_date;
        public Dictionary<string, string> titles;
        public string type;

        public int GetId()
        {
            string[] args = link.Split('/');
            return System.Convert.ToInt32(args[1]);
        }

        public bool WasDeleted()
        {
            return string.IsNullOrEmpty(link);
        }

        public UpdateType GetUpdateType()
        {
            if (link.StartsWith("album/"))
                return UpdateType.Album;
            else if (link.StartsWith("artist/"))
                return UpdateType.Artist;
            else if (link.StartsWith("product/"))
                return UpdateType.Product;
            else if (link.StartsWith("org/"))
                return UpdateType.Label;
            else if (link.Equals("product.php?id="))
                return UpdateType.Invalid;
            else if (link.Equals("org.php?id="))
                return UpdateType.Invalid;
            else
                throw new NotImplementedException(link);
        }

        public object Convert()
        {
            switch (GetUpdateType())
            {
                case UpdateType.Album:
                    AlbumListAlbum albumListAlbum = new AlbumListAlbum();
                    albumListAlbum.album_type = type;
                    albumListAlbum.catalog = catalog;
                    albumListAlbum.link = link;
                    albumListAlbum.release_date = release_date;
                    albumListAlbum.titles = titles;
                    albumListAlbum.type = type;
                    return albumListAlbum;
                case UpdateType.Artist:
                    ArtistListArtist artistListArtist = new ArtistListArtist();
                    artistListArtist.link = link;
                    artistListArtist.names = names;
                    artistListArtist.name_real = names.First().Value;
                    return artistListArtist;
                case UpdateType.Product:
                    ProductListProduct productListProduct = new ProductListProduct();
                    productListProduct.link = link;
                    productListProduct.names = titles;
                    productListProduct.type = type;

                    if (productListProduct.names == null && names != null)
                        productListProduct.names = names;

                    return productListProduct;
                case UpdateType.Label:
                    LabelListLabel labelListLabel = new LabelListLabel();
                    labelListLabel.link = link;
                    labelListLabel.names = titles;
                    return labelListLabel;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

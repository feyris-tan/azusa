using System.Drawing;

namespace moe.yo3explorer.azusa.Control.Galleria
{
    public interface IGalleriaModel
    {
        int ImagesCount { get; }
        Image GetImage(int ordinal);
        Galleria Galleria { get; set; }
    }
}

using System.Collections.Generic;

namespace GameBussinesLogic.Songs.Models
{
    public interface ISong
    {
        string Title { get; set; }
        string PreviewUrl { get; set; }

        IExtendedUrls GetExtendedUrls();
        IEnumerable<IArtist> GetArtists();
        string GetImageUrl();
    }
}

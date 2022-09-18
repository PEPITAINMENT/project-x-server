namespace SongBussinsLogic.Models
{
    public interface ISong
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string PreviewLink { get; set; }
    }
}

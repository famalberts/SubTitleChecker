namespace SubtitleChecker.Domain
{
    public class Video
    {
        #region Constructors 

        public Video()
            : this(new Author(), new Medium(), new SubtitleCollection())
        {
        }
        
        public Video(Author author, Medium medium, SubtitleCollection subtitles)
        {
            Medium = medium;
            Author = author;
            Subtitles = subtitles;
        }

        #endregion

        #region Properties
        
        public Author Author { get; private set; }

        public Medium Medium { get; private set; }

        public SubtitleCollection Subtitles { get; private set; }

        #endregion
    }
}

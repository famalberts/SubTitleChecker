namespace SubtitleChecker.Domain
{
    public class Medium
    {
        #region Properties 

        public int CodePage { get; set; }

        public string DiscId { get; set; }

        public string DvdTitle { get; set; }

        #endregion

        #region Constructors 

        public Medium()
            : this(string.Empty, string.Empty, 0)
        {
        }

        public Medium(string dvdTitle, string discId, int codePage)
        {
            DvdTitle = dvdTitle;
            DiscId = discId;
            CodePage = codePage;
        }

        #endregion
    }
}

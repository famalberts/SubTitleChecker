using System;
using System.Collections.Generic;

namespace SubtitleChecker.Domain
{
    public class Author
    {
		#region Properties 

        public string Name { get; private set; }

		#endregion  

		#region Constructors 
                
        public Author(string name)
        {
            Name = name;
        }

		#endregion  
    }

    public class Medium
    {
		#region Properties 

        public int CodePage { get; private set; }

        public string DiscId { get; private set; }

        public string DvdTitle { get; private set; }

		#endregion  

		#region Constructors 
                
        public Medium(string dvdTitle, string discId, int codePage)
        {
            DvdTitle = dvdTitle;
            DiscId = discId;
            CodePage = codePage;
        }

		#endregion  
    }

    public class SubtitleCollection : IEnumerable<Subtitle>
    {
		#region Fields 

        private readonly List<Subtitle> _subtitles = new List<Subtitle>();

        #endregion 

		#region Properties 

        public string Info { get; private set; }

        public string Web { get; private set; }

        public string Title { get; private set; }

        public string Original { get; private set; }

        public string License { get; private set; }

        public string Language { get; private set; }

        public string Format { get; private set; }

        public Author Author { get; private set; }

        public Medium Medium { get; private set; }

		#endregion 

		#region Methods 

        public IEnumerator<Subtitle> GetEnumerator()
        {
            return _subtitles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _subtitles.GetEnumerator();
        }

        public void Add(Subtitle subtitle)
        {
            _subtitles.Add(subtitle);
        }

		#endregion  

        #region Constructors

        public SubtitleCollection(Author author, Medium medium, string language, string license, string original, string title, string web, string info, string format)
        {
            Medium = medium;
            Author = author;
            Language = language;
            License = license;
            Original = original;
            Info = info;
            Web = web;
            Title = title;
            Format = format;
        }

        #endregion
    }

    public class Subtitle
    {
        #region Fields

        private readonly List<string> _text;

        #endregion
        
        #region Properties 

        public TimeSpan Duration { get; private set; }

        public TimeSpan Offset { get; private set; }

        public IEnumerable<string> Text
        {
            get
            {
                return _text;
            }
        }

		#endregion  

        #region Constructors 
                
        public Subtitle(TimeSpan offset, TimeSpan duration, IEnumerable<string> text)
        {
            Duration = duration;
            Offset = offset;
            _text = text != null ? new List<string>(text) : new List<string>();
        }

		#endregion  
    }
}

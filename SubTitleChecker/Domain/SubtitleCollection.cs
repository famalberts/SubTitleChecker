using System;
using System.Collections.Generic;
using SubtitleChecker.Domain.Rules;

namespace SubtitleChecker.Domain
{
    public class SubtitleCollection : IEnumerable<Subtitle>
    {
        #region Fields 

        private readonly List<Subtitle> _subtitles = new List<Subtitle>();

        #endregion

        #region Properties 

        public string Info { get; set; }

        public string Web { get; set; }

        public string Title { get; set; }

        public string Original { get; set; }

        public string License { get; set; }

        public string Language { get; set; }

        public string Format { get; set; }

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

        public SubtitleCollection() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public SubtitleCollection(string language, string license, string original, string title, string web, string info, string format)
        {
            Language = language;
            License = license;
            Original = original;
            Info = info;
            Web = web;
            Title = title;
            Format = format;
        }

        #endregion

        public RuleValidationResult[] Validate()
        {
            var results = new List<RuleValidationResult>();
            foreach (var subtitle in _subtitles)
            {
                var result = subtitle.Validate();
                if (result != null)
                {
                    results.AddRange(result);
                }
            }
            return results.ToArray();
        }
    }
}
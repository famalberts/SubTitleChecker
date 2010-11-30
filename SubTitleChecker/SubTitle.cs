using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubTitleChecker
{
    public enum Format
    {
        Undefined,
        ASCII,
    }

    public struct HeaderInfo
    {
        private string _title;
        private string _author;
        private string _language;
        private string _dvdTitle;
        private string _discID;
        private string _original;
        private string _license;
        private string _web;
        private string _info;
        private int _codePage;
        private Format _format;

        public HeaderInfo(string title, string author, string dvdTitle, string discID, int codePage, string language, Format format, string original, string license, string web, string info)
        {
            _title = title;
            _author = author;
            _dvdTitle = dvdTitle;
            _discID = discID;
            _codePage = codePage;
            _language = language;
            _format = format;
            _original = original;
            _license = license;
            _web = web;
            _info = info;
        }

        public string Language
        {
            get
            {
                return _language;
            }
            private set
            {
                _language = value;
            }
        }

        public string DiscID
        {
            get
            {
                return _discID;
            }
            private set
            {
                _discID = value;
            }
        }

        public string DvdTitle
        {
            get
            {
                return _dvdTitle;
            }
            private set
            {
                _dvdTitle = value;
            }
        }

        public int CodePage
        {
            get
            {
                return _codePage;
            }
            private set
            {
                _codePage = value;
            }
        }

        public Format Format
        {
            get
            {
                return _format;
            }
            private set
            {
                _format = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            private set
            {
                _title = value;
            }
        }

        public string Original
        {
            get
            {
                return _original;
            }
            private set
            {
                _original = value;
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }
            private set
            {
                _author = value;
            }
        }

        public string Web
        {
            get
            {
                return _web;
            }
            private set
            {
                _web = value;
            }
        }

        public string Info
        {
            get
            {
                return _info;
            }
            private set
            {
                _info = value;
            }
        }

        public string License
        {
            get
            {
                return _license;
            }
            private set
            {
                _license = value;
            }
        }
    }

    public class SubTitles : IEnumerable<SubTitle>
    {
        private List<SubTitle> _subtitles = new List<SubTitle>();

        public HeaderInfo HeaderInfo
        {
            get;
            private set;
        }

        public IEnumerator<SubTitle> GetEnumerator()
        {
            return _subtitles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _subtitles.GetEnumerator();
        }
    }

    public struct SubTitle
    {
        public TimeSpan Offset
        {
            get;
            private set;
        }

        public TimeSpan Duration
        {
            get;
            private set;
        }

        public string Text
        {
            get;
            private set;
        }
    }
}

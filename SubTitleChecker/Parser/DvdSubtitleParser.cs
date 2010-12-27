using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SubtitleChecker.Domain;

namespace SubtitleChecker.Parser
{
    public class DvdSubtitleParser : ISubtitleParser
    {
        #region Enums

        private enum State
        {
            BeginHeader,
            Head,
            DiscId,
            DvdTitle,
            CodePage,
            Format,
            Lang,
            Title,
            Original,
            Author,
            Web,
            Info,
            License,
            EndHeader,
            BeginSubtitle,
            SubtitleStartTime,
            Subtitle,
            Failed,
        }

        #endregion

        #region Fields

        private State _state = State.BeginHeader;
        private Stream _stream;
        private long _origin;

        #endregion

        #region Constructors 
        
        public DvdSubtitleParser(Stream stream)
        {
            if (stream == null) throw new InvalidDataException("Not a valid stream to parse from.");
            _stream = stream;
            _origin = _stream.Position;
        }

        #endregion

        #region Methods

        public void Parse(Video video)
        {
            if (video == null) return;
            if (_stream.Position != _origin)
            {
                _stream.Seek(_origin, SeekOrigin.Begin);
            }
            var reader = new StreamReader(_stream);
            ParseHeader(reader, video);
            ParseSubtitles(reader, video.Subtitles);
        }

        private SubtitleCollection ParseSubtitles(StreamReader reader, SubtitleCollection subtitleCollection)
        {
            var subtitleLines = new List<string>();
            var subtitles = 0;
            Subtitle previousSubtitle = null;
            var startTime = new TimeSpan();
            bool readNextLine = true;
            string line = string.Empty;
            while (reader.Peek() >= 0)
            {
                if (readNextLine) line = reader.ReadLine();
                switch (_state)
                {
                    case State.EndHeader:
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line.Length > 0 && line[0].Equals('}'))
                            {
                                _state = State.BeginSubtitle;
                                line = line.Substring(1, line.Length - 1);
                                readNextLine = false;
                                break;
                            }
                        }
                        readNextLine = true;
                        break;
                    case State.BeginSubtitle:
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line.Length > 0 && line[0].Equals('{'))
                            {
                                _state = State.SubtitleStartTime;
                                line = line.Substring(1, line.Length - 1);
                                readNextLine = false;
                                break;
                            }
                        }
                        readNextLine = true;
                        break;
                    case State.SubtitleStartTime:
                        readNextLine = ParseSubtitleStartTime(subtitleLines, ref line, ref startTime);
                        break;
                    case State.Subtitle:
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line.Length > 0)
                            {
                                if (line[0].Equals('}'))
                                {
                                    _state = State.BeginSubtitle;
                                    line = line.Substring(1, line.Length - 1);
                                    readNextLine = false;
                                    if (previousSubtitle != null)
                                    {
                                        subtitleCollection.Add(new Subtitle(previousSubtitle.Number, previousSubtitle.Offset, startTime - previousSubtitle.Offset, previousSubtitle.Text));
                                        previousSubtitle = null;
                                    }
                                    if (subtitleLines.Count > 0)
                                    {
                                        subtitles++;
                                        previousSubtitle = new Subtitle(subtitles, startTime, new TimeSpan(), subtitleLines.ToArray());
                                    }
                                    break;
                                }
                                subtitleLines.Add(line);
                            }
                        }
                        readNextLine = true;
                        break;

                }
            }
            return subtitleCollection;
        }

        private bool ParseSubtitleStartTime(List<string> subtitleLines, ref string line, ref TimeSpan startTime)
        {
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Length > 0 && line[0].Equals('T'))
                {
                    if (line.Length > 0)
                    {
                        line = line.Substring(1, line.Length - 1);
                        var timeFractions = line.Split(':');
                        if (timeFractions.Length == 4)
                        {
                            if (TimeSpan.TryParse(string.Format("{0}:{1}:{2}.{3}", timeFractions[0], timeFractions[1], timeFractions[2], timeFractions[3]), out startTime))
                            {
                                subtitleLines.Clear();
                                _state = State.Subtitle;
                                return true;
                            }
                        }
                    }
                    _state = State.BeginSubtitle;
                    return true;
                }
            }
            return true;
        }

        private void ParseHeader(StreamReader reader, Video video)
        {
            bool result = false;
            string discId = string.Empty;
            string dvdTitle = string.Empty;
            int codePage = -1;
            string format = string.Empty;
            string language = string.Empty;
            string license = string.Empty;
            string original = string.Empty;
            string author = string.Empty;
            string title = string.Empty;
            string web = string.Empty;
            string info = string.Empty;

            bool readNextLine = true;
            string line = string.Empty;
            _state = State.BeginHeader;
            while (_state != State.EndHeader && reader.Peek() >= 0)
            {
                if (readNextLine) line = reader.ReadLine();
                switch (_state)
                {
                    case State.BeginHeader:
                        readNextLine = ParseBeginHeader(ref line);
                        break;
                    case State.Head:
                        readNextLine = ParseHead(line);
                        break;
                    case State.DiscId:
                        discId = ParseKeyValue(line, State.DiscId, State.DvdTitle, ref readNextLine);
                        break;
                    case State.DvdTitle:
                        dvdTitle = ParseKeyValue(line, State.DvdTitle, State.CodePage, ref readNextLine);
                        break;
                    case State.CodePage:
                        string codePageString = ParseKeyValue(line, State.CodePage, State.Format, ref readNextLine);
                        if (_state != State.CodePage)
                        {
                            if (!string.IsNullOrEmpty(codePageString))
                            {
                                if (!int.TryParse(codePageString, NumberStyles.Integer, CultureInfo.InvariantCulture, out codePage))
                                {
                                    _state = State.Failed;
                                }
                            }
                        }
                        break;
                    case State.Format:
                        format = ParseKeyValue(line, State.Format, State.Lang, ref readNextLine);
                        break;
                    case State.Lang:
                        language = ParseKeyValue(line, State.Lang, State.Title, ref readNextLine);
                        break;
                    case State.Title:
                        title = ParseKeyValue(line, State.Title, State.Original, ref readNextLine);
                        break;
                    case State.Original:
                        original = ParseKeyValue(line, State.Original, State.Author, ref readNextLine);
                        break;
                    case State.Author:
                        author = ParseKeyValue(line, State.Author, State.Web, ref readNextLine);
                        break;
                    case State.Web:
                        web = ParseKeyValue(line, State.Web, State.Info, ref readNextLine);
                        break;
                    case State.Info:
                        info = ParseKeyValue(line, State.Info, State.License, ref readNextLine);
                        break;
                    case State.License:
                        license = ParseKeyValue(line, State.License, State.EndHeader, ref readNextLine);
                        if (_state == State.EndHeader)
                        {
                            result = true;
                        }
                        break;
                }
            }
            if (result)
            {
                video.Author.Name = author;
                video.Medium.CodePage = codePage;
                video.Medium.DiscId = discId;
                video.Medium.DvdTitle = dvdTitle;
                video.Subtitles.Language = language;
                video.Subtitles.License = license;
                video.Subtitles.Original = original;
                video.Subtitles.Title = title;
                video.Subtitles.Web = web;
                video.Subtitles.Info = info;
                video.Subtitles.Format = format;
            }
        }

        private bool ParseHead(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Length > 0 && line.Equals(State.Head.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    _state = State.DiscId;
                    return true;
                }
            }
            return true;
        }

        private bool ParseBeginHeader(ref string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Length > 0 && line[0].Equals('{'))
                {
                    _state = State.Head;
                    line = line.Substring(1, line.Length - 1);
                    return false;
                }
            }
            return true;
        }

        private string ParseKeyValue(string line, State state, State nextState, ref bool readNextLine)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Length > 0)
                {
                    int index = line.IndexOf('=');
                    if (index > 0 && line.Substring(0, index).Equals(state.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        result = line.Substring(index + 1, line.Length - index - 1);
                        _state = nextState;
                    }
                    readNextLine = true;
                    return result;
                }
            }
            readNextLine = true;
            return result;
        }

        #endregion
    }
}

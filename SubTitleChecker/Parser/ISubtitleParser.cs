using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SubtitleChecker.Parser
{
    interface ISubtitleParser
    {
        void Parse(Domain.Video video);
    }
}

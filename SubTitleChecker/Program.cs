using System.IO;
using System.Linq;
using SubtitleChecker.Domain.Rules.BasicRules;
using SubtitleChecker.Domain;
using SubtitleChecker.Domain.Rules.TimingRules;

namespace SubtitleChecker
{
    class Program
    {
        static void Main()
        {
            using (var f = new FileStream(@"C:\Users\Simon\Documents\Visual Studio 2010\Projects\SubTitleChecker\SubTitleChecker\Test\DVDSubtitle.sub", FileMode.Open))
            {
                var p = new Parser.DvdSubtitleParser(f);
                var v = new Video();
                p.Parse(v);

                var res = v.Subtitles.Validate();
            }
        }
    }
}

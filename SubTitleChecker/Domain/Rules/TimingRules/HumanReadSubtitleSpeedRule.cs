using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SubtitleChecker.Domain.Rules.TimingRules
{
    public class HumanReadSubtitleSpeedRule : ISubtitleRule
    {
        private const int MaximumWordsPerMinute = 250;
        public static readonly RegexOptions Options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline;
        public static readonly Regex Regex = new Regex(@"[\S]+"); 

        public string RuleDescription
        {
            get { return "An average reader can read up to 250 words per minute"; }
        }

        public static int WordCount(string line)
        {
            var collection = Regex.Matches(line);
            return collection.Count;
        }

        public RuleValidationResult CheckRule(Subtitle subtitle)
        {
            if (subtitle == null) return new RuleValidationResult(RuleDescription, "Not a valid subtitle.", subtitle);
            var wordCount = subtitle.Text.Sum(text => WordCount(text));
            var time = subtitle.Duration.TotalMilliseconds / 1000;
            var minimumTimeLimit = 60.0*wordCount/MaximumWordsPerMinute;
            if (time < minimumTimeLimit)
            {
                var wordsPerMinute = (wordCount * 60000) / (subtitle.Duration.TotalMilliseconds);
                var reduceWords = (int)Math.Ceiling(((wordsPerMinute - MaximumWordsPerMinute) * subtitle.Duration.TotalMilliseconds) / 60000);
                var extendDuration = Math.Ceiling(1000.0 * (minimumTimeLimit - time));
                return new RuleValidationResult(RuleDescription, string.Format("Subtitle with '{0}' words per minute should be limited to '{1}' words per minute. Reduce with {2} words or extend duration of subtitle with {3} milliseconds", Math.Round(wordsPerMinute, 0), MaximumWordsPerMinute, reduceWords, extendDuration), subtitle);
            }
            return null;
        }
    }
}

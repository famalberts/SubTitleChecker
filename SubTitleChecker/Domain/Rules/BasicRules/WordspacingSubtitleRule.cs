using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitleChecker.Domain.Rules.BasicRules
{
    /// <summary>
    /// Words within a subtitle should be separated by a single space.
    /// </summary>
    public class WordSpacingSubtitleRule : ISubtitleRule
    {
        public static readonly RegexOptions Options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline;
        public static readonly Regex Regex = new Regex(@"\s{2,}", Options); 

        #region Properties

        public string RuleDescription
        {
            get
            {
                return "Words within a subtitle should be separated by a single space.";
            }
        }

        #endregion

        #region Methods

        public RuleValidationResult CheckRule(Subtitle subtitle)
        {
            if (subtitle == null) return new RuleValidationResult(RuleDescription, "Not a valid subtitle", subtitle);
            var result = true;
            var columnIndex = 0;
            var lineIndex = 0;
            
            foreach (var line in subtitle.Text)
            {
                lineIndex++;
                var matches = Regex.Match(line);
                if (!matches.Success) continue;
                result = false;
                columnIndex = matches.Index;
                break;
            }
            if (result)
            {
                return null;
            }
            return new RuleValidationResult(RuleDescription, string.Format("Words are not seperated with a single space character at line {0}, position {1}.", lineIndex, columnIndex), subtitle);
        }

        #endregion
    }
}

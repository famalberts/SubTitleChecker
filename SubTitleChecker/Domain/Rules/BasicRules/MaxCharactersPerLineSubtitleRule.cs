namespace SubtitleChecker.Domain.Rules.BasicRules
{
    /// <summary>
    /// Words within a subtitle should be separated by a single space.
    /// </summary>
    public class MaxCharactersPerLineSubtitleRule : ISubtitleRule
    {
        private const int Maxnumber = 40;

        #region Properties

        public string RuleDescription
        {
            get
            {
                return "Total number of characters should be limited to " + Maxnumber + ".";
            }
        }

        #endregion

        #region Methods

        public RuleValidationResult CheckRule(Subtitle subtitle)
        {
            if (subtitle == null) return new RuleValidationResult(RuleDescription, "Not a valid subtitle", subtitle);

            int linenumber = 0;
            foreach (var line in subtitle.Text)
            {
                linenumber++;
                if (line.Length > Maxnumber)
                {
                    return new RuleValidationResult(RuleDescription, string.Format("Total number of characters of {0} exceeds the maximum of {1} at line {2}. Reduce with {3} characters.", line.Length, Maxnumber, linenumber, line.Length - Maxnumber), subtitle);
                }
            }
            return null;
        }

        #endregion
    }
}

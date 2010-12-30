namespace SubtitleChecker.Domain.Rules.BasicRules
{
    /// <summary>
    /// Teletext characters should be displayed in double height and 
    /// mixed (upper and lower) case.
    /// </summary>
    public class UpperLowercaseSubtitleRule : ISubtitleRule
    {
        #region Properties

        public string RuleDescription
        {
            get
            {
                return "Teletext characters should be displayed in double height and mixed (upper and lower) case.";
            }
        }

        #endregion

        #region Methods

        public RuleValidationResult CheckRule(Subtitle subtitle)
        {
            if (subtitle == null) return new RuleValidationResult(RuleDescription, "Not a valid subtitle.", subtitle);
            var isUpper = false;
            var isLower = false;
            foreach (var line in subtitle.Text)
            {
                foreach (var t in line)
                {
                    if (char.IsUpper(t)) isUpper = true;
                    if (char.IsLower(t)) isLower = true;
                    if ((isUpper == isLower) && isUpper) break;
                }
                if ((isUpper == isLower) && isUpper) break;
            }
            if ((isUpper == isLower) && isUpper)
            {
                return null;
            }
            if (isLower)
            {
                return new RuleValidationResult(RuleValidationResult.SeverityLevel.Warning, RuleDescription, "Mix upper upper and lower case characters in text. It only contains lower case characters.", subtitle);
            }
            return isUpper ? new RuleValidationResult(RuleValidationResult.SeverityLevel.Warning, RuleDescription, "Mix upper upper and lower case characters in text. It only contains upper case characters.", subtitle) : new RuleValidationResult(RuleValidationResult.SeverityLevel.Warning, RuleDescription, "Text is not mixed upper and lower case.", subtitle);
        }

        #endregion
    }
}

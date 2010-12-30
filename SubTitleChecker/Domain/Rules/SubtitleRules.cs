using System.Collections.Generic;
using SubtitleChecker.Domain.Rules.BasicRules;
using SubtitleChecker.Domain.Rules.TimingRules;

namespace SubtitleChecker.Domain.Rules
{
    public class SubtitleRules
    {
        private static readonly List<ISubtitleRule> SubtitleRuleCollection = new List<ISubtitleRule>();

        static SubtitleRules()
        {
            //_rules.Add(new UpperLowercaseSubtitleRule());
            SubtitleRuleCollection.Add(new WordSpacingSubtitleRule());
            SubtitleRuleCollection.Add(new HumanReadSubtitleSpeedRule());
            SubtitleRuleCollection.Add(new MaxCharactersPerLineSubtitleRule());
        }

        public static ISubtitleRule[] Rules
        {
            get { return SubtitleRuleCollection.ToArray(); }
        }
    }
}

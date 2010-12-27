using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitleChecker.Domain.Rules.BasicRules;
using SubtitleChecker.Domain.Rules.TimingRules;

namespace SubtitleChecker.Domain.Rules
{
    public class SubtitleRules
    {
        private static readonly List<ISubtitleRule> _rules = new List<ISubtitleRule>();

        static SubtitleRules()
        {
            _rules.Add(new UpperLowercaseSubtitleRule());
            _rules.Add(new WordSpacingSubtitleRule());
            _rules.Add(new HumanReadSubtitleSpeedRule());
        }

        public static ISubtitleRule[] Rules
        {
            get { return _rules.ToArray(); }
        }
    }
}

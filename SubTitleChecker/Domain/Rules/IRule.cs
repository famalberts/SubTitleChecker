using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleChecker.Domain.Rules
{
    public interface IRule
    {
        string RuleDescription { get; }
    }
}

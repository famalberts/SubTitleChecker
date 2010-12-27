using System;
using System.Collections.Generic;
using SubtitleChecker.Domain.Rules;

namespace SubtitleChecker.Domain
{
    public class Subtitle
    {
        #region Fields 

        private readonly List<string> _text;

        #endregion
        
        #region Properties 

        public int Number { get; private set; } 

        public TimeSpan Duration { get; private set; }

        public TimeSpan Offset { get; private set; }

        public IEnumerable<string> Text
        {
            get
            {
                return _text;
            }
        }

		#endregion  

        #region Constructors 
                
        public Subtitle(int number, TimeSpan offset, TimeSpan duration, IEnumerable<string> text)
        {
            Duration = duration;
            Offset = offset;
            Number = number;
            _text = text != null ? new List<string>(text) : new List<string>();
        }

		#endregion  

        #region Methods

        public RuleValidationResult[] Validate()
        {
            var results = new List<RuleValidationResult>();
            foreach (var rule in SubtitleRules.Rules)
            {
                var result = rule.CheckRule(this);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            return results.ToArray();
        }

        #endregion
    }
}

namespace SubtitleChecker.Domain.Rules
{
    /// <summary>
    /// Rule validation result
    /// </summary>
    public class RuleValidationResult
    {
        #region Enums

        public enum SeverityLevel
        {
            Warning,
            Error,
            Exception
        }

        #endregion

        #region Fields

        private readonly SeverityLevel _severity;
        private readonly string _ruleDescription;
        private readonly string _description;
        private readonly Subtitle _subtitle;

        #endregion

        #region Constructors

        public RuleValidationResult(SeverityLevel severity)
            : this(severity, string.Empty, string.Empty, null)
        {
        }

        public RuleValidationResult(string ruleDescription, string violationDescription, Subtitle subtitle)
            : this(SeverityLevel.Error, ruleDescription, violationDescription, subtitle)
        {
        }

        public RuleValidationResult(SeverityLevel severity, string ruleDescription, string violationDescription, Subtitle subtitle)
        {
            _subtitle = subtitle;
            _severity = severity;
            _description = violationDescription;
            _ruleDescription = ruleDescription;
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == typeof (RuleValidationResult) && Equals((RuleValidationResult) obj);
        }

        public bool Equals(RuleValidationResult other)
        {
            if ((object)other == null) return false;
            return other._severity.Equals(_severity) && Equals(other._description, _description);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_severity.GetHashCode() * 397) ^ (_description != null ? _description.GetHashCode() : 0);
            }
        }

        #endregion

        #region Properties

        public static bool operator ==(RuleValidationResult result1, RuleValidationResult result2)
        {
            if ((object)result1 == null)
            {
                if ((object)result2 == null) return true;
                return false;
            }
            return result1.Equals(result2);
        }

        public static bool operator !=(RuleValidationResult result1, RuleValidationResult result2)
        {
            if ((object)result1 == null)
            {
                if ((object)result2 == null) return false;
                return true;
            }
            return !result1.Equals(result2);
        }

        public SeverityLevel Severity
        {
            get
            {
                return _severity;
            }
        }

        public string RuleDescription
        {
            get 
            { 
                return _ruleDescription;
            }
        }

        public string ViolationDescription
        {
            get
            {
                return _description;
            }
        }

        public Subtitle Subtitle
        {
            get 
            { 
                return _subtitle;
            }
        }

        #endregion
    }
}

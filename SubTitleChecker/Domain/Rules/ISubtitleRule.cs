namespace SubtitleChecker.Domain.Rules
{
    public interface ISubtitleRule : IRule
    {
        RuleValidationResult CheckRule(Subtitle subtitle);
    }
}

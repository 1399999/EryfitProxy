

using EryfitProxy.Kernel.Validators;
using System.Collections.Generic;

namespace EryfitProxy.Kernel
{
    public class AggregateEryfitSettingAnalyzer : ISettingAnalyzer
    {
        public static ISettingAnalyzer Instance { get; } = new AggregateEryfitSettingAnalyzer();

        private static readonly ISettingAnalyzer[] Validators = new ISettingAnalyzer[]
        {
            new RuleCountValidator(),
            new SkipSslEnableValidator(),
            new OutOfScopeValidator(),
            new ActionValidator()
        };

        public IEnumerable<ValidationResult> Validate(EryfitSetting setting)
        {
            foreach (var validator in Validators)
            {
                foreach (var result in validator.Validate(setting))
                {
                    yield return result;
                }
            }
        }
    }

    public enum ValidationRuleLevel
    {
        Information,
        Warning,
        Error,
        Fatal
    }

    public interface ISettingAnalyzer
    {
        IEnumerable<ValidationResult> Validate(EryfitSetting setting);
    }

    public class ValidationResult
    {
        public ValidationResult(ValidationRuleLevel level, string message, string senderName)
        {
            Level = level;
            Message = message;
            SenderName = senderName;
        }

        public ValidationRuleLevel Level { get;  }

        public string Message { get; }

        public string SenderName { get; }

        public override string ToString()
        {
            return $"[{Level}] [{SenderName}]: {Message}";
        }
    }
}

using System;

namespace Inbox2.Framework.Plugins.Recurrence
{
    public interface IRecurrenceValue
    {
        RecurrenceRuleType Type { get; }
        TimeSpan IntervalTime { get; }

        IRecurrenceValue Parse(string value);

        string ParseToString();

        bool Validate();
    }
}
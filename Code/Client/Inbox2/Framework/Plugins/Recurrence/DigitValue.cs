using System;

namespace Inbox2.Framework.Plugins.Recurrence
{
    public abstract class DigitValue : IRecurrenceValue
    {
        public TimeSpan IntervalTime
        {
            get { return new TimeSpan(); }
        }
        public int Value { get; set; }
        public abstract RecurrenceRuleType Type { get; }

        public abstract IRecurrenceValue Parse(string value);

        public abstract string ParseToString();

        public abstract bool Validate();

    }
}
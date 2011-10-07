using System;

namespace Inbox2.Framework.Plugins.Recurrence
{
    public abstract class FirstLastValue : IRecurrenceValue
    {
        public TimeSpan IntervalTime
        {
            get { return new TimeSpan(); }
        }
        public bool IsFirstOf { get; set; }
        public bool IsLastOf { get; set; }
        public abstract RecurrenceRuleType Type { get; }
        public int Value { get; set; }

        public abstract IRecurrenceValue Parse(string value);

        public abstract string ParseToString();

        public abstract bool Validate();
    }
}
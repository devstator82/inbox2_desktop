using System;
using System.Windows.Controls;

namespace Inbox2.Framework.UI.Controls
{
    public enum StepTypes
    {
        None,
        Seconds,
        Minutes,
        Hours,
    }

    public class TimePicker : ComboBox
    {
        private const int defaultstep = 30;
        private int step = defaultstep;
        private StepTypes steptype = StepTypes.None;

        public int Step
        {
            get { return step; }
            private set
            {
                // Check if the step is valid
                if (!IsValidStep(value))
                {
                    // Set defaults and return
                    SetDefaults();
                    return;
                }

                // The step is valid, set it    
                step = value;
            }
        }
        public StepTypes StepType
        {
            get { return steptype; }
            private set { steptype = value; }
        }
        public TimePicker StartTimeListBox { get; set; }
        public bool IsStartTimeListBox { get { return StartTimeListBox == null; } }
        public bool IsEndTimeListBox { get { return StartTimeListBox != null; } }
        //public TimeSpan SelectedValue { get; set; }

        public TimePicker()
        {
            // Set defaults
            SetDefaults();

            // Set the selectable items
            LoadItems();
        }

        public TimePicker(StepTypes steptype, int step)
        {
            // Set step values
            StepType = steptype;
            Step = step;

            // Set the selectable items
            LoadItems();
        }

        private void SetDefaults()
        {
            // Set the step type
            StepType = StepTypes.Minutes;

            // Set the step
            Step = defaultstep;

            // The ComboBox will be editable by default
            IsEditable = true;

            // Set the itemstring format to show only hours and minutes
            ItemStringFormat = "HH:mm";
        }

        private bool IsValidStep(int timestep)
        {
            // Check if StepType is set
            if (StepType == StepTypes.None) return false;

            // Check if the value is valid according it's type
            switch (StepType)
            {
                case StepTypes.Seconds:
                case StepTypes.Minutes:
                    return (timestep >= 0 && timestep < 60);
                case StepTypes.Hours:
                    return (timestep >= 0 && timestep <= 24);
                case StepTypes.None:
                    return false;
                default:
                    return false;
            }
        }

        private void LoadItems()
        {
            // Clear the items
            Items.Clear();

            // Set the items
            SetItems();

            // Set the first item as selected item
            SelectedIndex = 0;
        }

        private void SetItems()
        {
            DateTime current = new DateTime(1, 1, 1, 0, 0, 0, 0);
            switch (StepType)
            {
                case StepTypes.Minutes:
                    while (current.Day < 2)
                    {
                        // Add the current timespan to the items
                        Items.Add(current);

                        // Add the step to the current timespan
                        current = current.AddMinutes(Step);
                    }
                    break;
                case StepTypes.None:
                    return;
                default: return;
            }
        }
    }
}

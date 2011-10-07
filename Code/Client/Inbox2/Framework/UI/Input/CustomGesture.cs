using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Inbox2.Framework.UI.Input
{
    public class CustomGesture : InputGesture
    {
        private readonly Key key;
        private readonly ModifierKeys modifers;
        private bool useModifiers;
        private Key previous;

        public CustomGesture(Key key) : this(key, ModifierKeys.None)
        {
        }

        public CustomGesture(Key key, ModifierKeys modifers)
        {
            this.key = key;
            this.modifers = modifers;

			if (modifers != ModifierKeys.None)
				this.useModifiers = true;
        }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            var args = inputEventArgs as KeyEventArgs;

            // Don't execute input binding in case focus is on a textbox
            if ((inputEventArgs.Device.Target is TextBoxBase || inputEventArgs.Device.Target is WebBrowser)
				&& !(modifers == ModifierKeys.Control || modifers == ModifierKeys.Alt))
                return false;

            if (args == null)
                return false;

            bool match;            

            if (useModifiers)
                match = args.Key == key && args.KeyboardDevice.Modifiers == modifers;
            else
            {
                match = (args.Key == key && args.KeyboardDevice.Modifiers == ModifierKeys.None);

                // To not interfere with multiple key gestures
                if (previous == Key.G || previous == Key.N || previous == Key.A)
                    match = false;
            }

            previous = args.Key;

            return match;
        }
    }
}

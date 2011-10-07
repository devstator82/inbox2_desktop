using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.Calendar.Controls;
using System.Windows;

namespace Inbox2.Plugins.Calendar.Helpers
{
    class PluginHelper : IColumnPlugin
    {
        private CalendarState state;

        public PluginHelper(CalendarState state)
        {
            this.state = state;
        }

        #region IColumnPlugin

        double IColumnPlugin.PreferredWidth
        {
            get { return 1.5; }
        }

        UIElement IColumnPlugin.CreateColumnView()
        {
            return new Column();
        }

        #endregion
    }
}

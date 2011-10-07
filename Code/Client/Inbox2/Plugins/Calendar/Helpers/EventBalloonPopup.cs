using Inbox2.Framework.UI.Controls;
using System;
using System.Windows.Controls.Primitives;

namespace Inbox2.Plugins.Calendar.Helpers
{
    public class EventBalloonPopup : BalloonPopup
    {
        protected override void OnInitialized(EventArgs e)
        {
            Placement = PlacementMode.Left;
            HorizontalOffset = 85;
            VerticalOffset = 27;
            StaysOpen = false;

            base.OnInitialized(e);
        }
    }
}

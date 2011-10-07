using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Inbox2.Framework.UI.Controls
{
	public class ButtonBalloonPopup : BalloonPopup
	{
	    public ButtonBalloonPopup()
	    {
            HorizontalOffset = 85;
            VerticalOffset = 27;
			Placement = PlacementMode.Left;
	    }

	    protected override void OnInitialized(EventArgs e)
		{			
            StaysOpen = false;

			base.OnInitialized(e);
		}

		protected override void OnOpened(EventArgs e)
		{
			base.OnOpened(e);

			PopupManager.ActivePopup = this;
		}

		protected override void OnClosed(EventArgs e)
		{
			PopupManager.ActivePopup = null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;

namespace Inbox2.Framework.UI.Controls
{
	public class ToolbarPopup : BalloonPopup
	{
		protected override void OnInitialized(EventArgs e)
		{
			Placement = PlacementMode.Top;
			StaysOpen = false;

			base.OnInitialized(e);			
		}
	}
}

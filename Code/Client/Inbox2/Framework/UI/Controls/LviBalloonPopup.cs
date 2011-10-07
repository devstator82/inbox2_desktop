using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;

namespace Inbox2.Framework.UI.Controls
{
	public class LviBalloonPopup : BalloonPopup
	{
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			Placement = PlacementMode.Right;
		}
	}
}

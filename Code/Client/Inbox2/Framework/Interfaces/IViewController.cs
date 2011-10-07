using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;

namespace Inbox2.Framework.Interfaces
{
	public interface IViewController
	{
		/// <summary>
		/// Creates an instance of the given type and shows it modally.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void ShowPopup<T>() where T : Control, new();

		/// <summary>
		/// Shows the given control instance modally.
		/// </summary>
		void ShowPopup(Control control);

		/// <summary>
		/// Hides the active popup.
		/// </summary>
		void HidePopup();

		/// <summary>
		/// Moves to the given overview.
		/// </summary>
		/// <param name="view"></param>
		void MoveTo(WellKnownView view);

		/// <summary>
		/// Moves to the given details view and passes in the given data object.
		/// </summary>
		void MoveTo(IDetailsViewPlugin view, object dataInstance);

		/// <summary>
		/// Moves to the given new item view and passes in the given data object.
		/// </summary>
		/// <param name="plugin">The plugin.</param>
		/// <param name="dataInstance">The data instance.</param>
		void MoveTo(INewItemViewPlugin plugin, object dataInstance);
	}
}

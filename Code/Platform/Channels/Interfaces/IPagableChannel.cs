using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Interfaces
{
	public interface IPagableChannel
	{
		/// <summary>
		/// Gets or sets the start index.
		/// </summary>
		/// <value>The start index.</value>
		long StartIndex { get; set; }

		/// <summary>
		/// Gets or sets the end index.
		/// </summary>
		/// <value>The end index.</value>
		long EndIndex { get; set; }

		/// <summary>
		/// Gets or sets the size of the page.
		/// </summary>
		/// <value>The size of the page.</value>
		long PageSize { get; set; }

		/// <summary>
		/// Gets the number of available items.
		/// </summary>
		/// <returns></returns>
		long GetNumberOfItems();
	}
}

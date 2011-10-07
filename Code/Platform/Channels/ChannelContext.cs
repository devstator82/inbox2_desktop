using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Interfaces;

namespace Inbox2.Platform.Channels
{
	public class ChannelContext
	{
		[ThreadStatic] 
		private static ChannelContext _Current;

		public static ChannelContext Current
		{
			get
			{
				if (_Current == null)
				{
					_Current = new ChannelContext();
				}

				return _Current;
			}				
		}

		public IClientContext ClientContext { get; set; }}
}
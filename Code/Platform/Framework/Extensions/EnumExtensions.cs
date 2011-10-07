using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Platform.Framework.Extensions
{
	public static class EnumExtensions
	{
		public static bool IsSet(this EntityStates source, EntityStates option)
		{
			return (source & option) == option;
		}

		public static void Set(this EntityStates source, EntityStates options)
		{
			source |= options;
		}

		public static void Unset(this EntityStates source, EntityStates options)
		{
			source &= ~options;
		}
	}
}

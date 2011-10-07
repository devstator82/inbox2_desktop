using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IndexAttribute : Attribute
	{
		public bool Tokenize { get; set; }

		public bool Store { get; set; }

		public IndexAttribute()
		{
			Tokenize = true;

			Store = false;
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IndexAndTokenizeAttribute : IndexAttribute
	{
		public IndexAndTokenizeAttribute()
		{
			Tokenize = true;
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IndexAndStoreAttribute : IndexAttribute
	{
		public IndexAndStoreAttribute()
		{
			Store = true;
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IndexTokenizeAndStoreAttribute : IndexAttribute
	{
		public IndexTokenizeAndStoreAttribute()
		{			
			Tokenize = true;
			Store = true;
		}
	}
}

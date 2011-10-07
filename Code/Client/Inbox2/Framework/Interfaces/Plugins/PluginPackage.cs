using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Interfaces.Plugins
{
	public abstract class PluginPackage
	{
		public abstract string Name { get; }
		
		public virtual IStatePlugin State
		{
			get { return null; }
		}

		public virtual IEnumerable<IStreamViewPlugin> StreamViews
		{
			get { return null; }
		}

		public virtual IEnumerable<IOverviewPlugin> Overviews
		{
			get { return null; }
		}

		public virtual IEnumerable<IToolbarPlugin> ToolbarItems
		{
			get { return null; }
		}

		public virtual IColumnPlugin Colomn
		{
			get { return null; }
		}

		public virtual IDetailsViewPlugin DetailsView
		{
			get { return null; }
		}

		public virtual IOptionsPlugin Options
		{
			get { return null; }
		}

		public virtual INewItemViewPlugin NewItemView
		{
			get { return null; }
		}		

		public virtual void Initialize()
		{			
		}
	}
}

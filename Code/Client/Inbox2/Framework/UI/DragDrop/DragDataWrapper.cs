using System.Windows;

namespace Inbox2.Framework.UI.DragDrop
{
	public class DragDataWrapper
	{
		public DependencyObject Source { get; set; }
		
		public object Data { get; set; }
		
		public bool AllowChildrenRemove { get; set; }
		
		public IDataDropObjectProvider Shim { get; set; }
	}
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Inbox2.Framework.UI.DragDrop
{
	public class DropEventArgs : EventArgs
	{
		public Object Data { get; private set; }
		public DependencyObject Source { get; private set; }
		public UIElement Target { get; private set; }

		public DropEventArgs(object data, DependencyObject source, UIElement target)
		{
			Data = data;
			Source = source;
			Target = target;
		}
	}

	public class DropHelper
	{
		public event EventHandler<DropEventArgs> Drop;

		UIElement _dropTarget = null;
		string[] _datatypes = { typeof(UIElement).ToString(), "Text" };

		public string[] AllowedDataTypes
		{
			get { return _datatypes; }
			set
			{
				_datatypes = value;
				for (int x = 0; x < _datatypes.Length; x++)
				{
					_datatypes[x] = _datatypes[x].ToLower();
				}
			}
		}

		public DropHelper(UIElement wrapper)
		{
			_dropTarget = wrapper;

			_dropTarget.AllowDrop = true;
			_dropTarget.PreviewDragOver += DropTarget_DragOver;
			_dropTarget.PreviewDrop += DropTarget_Drop;
		}

		void DropTarget_Drop(object sender, DragEventArgs e)
		{
			IDataObject data = e.Data;

			DragDataWrapper dw = null;
			bool isDataOperation = false;

			if (data.GetDataPresent(typeof(DragDataWrapper).ToString()))
			{
				dw = data.GetData(typeof(DragDataWrapper).ToString()) as DragDataWrapper;
				if ((dw.Shim.SupportedActions & DragDropProviderActions.Data) != 0)
				{
					isDataOperation = true;
				}
			}

			var target = _dropTarget;

			// When dropping inside a listview, find the listview item we targetted
			if (_dropTarget is ListView)
				target = (e.OriginalSource as DependencyObject).FindListViewItem();

			if (!isDataOperation)
			{
				if (data.GetDataPresent(typeof(UIElement).ToString()))
				{
					UIElement uie = (UIElement)data.GetData(typeof(UIElement).ToString());

					if (Drop != null)
						Drop(this, new DropEventArgs(uie, dw.Source, target));
					
				}
				else if (data.GetDataPresent(DataFormats.Text))
				{
					string datastring = (string)data.GetData(DataFormats.Text);

					if (Drop != null)
						Drop(this, new DropEventArgs(datastring, dw.Source, target));
				}
			}
			else
			{
				if (target != null)
				{
					if (Drop != null)
						Drop(this, new DropEventArgs(dw.Data, dw.Source, target));
				}
			}
		}

		private DragDropEffects _allowedEffects;

		public DragDropEffects AllowedEffects
		{
			get { return _allowedEffects; }
			set { _allowedEffects = value; }
		}

		void DropTarget_DragOver(object sender, DragEventArgs e)
		{
			string[] types = e.Data.GetFormats();
			bool match = false;

			if (_datatypes == null || types == null)
			{
				//TODO: ??? Should we set for DragDropEffects.None? 
				return;
			}

			foreach (string s in types)
			{

				foreach (string type in _datatypes)
				{
					match = (s.ToLower() == type);
					if (match)
						break;
				}
				if (match)
					break;
			}
			if (match)
			{
				e.Effects = AllowedEffects;
				e.Handled = true;
			}
		}
	}

	internal static class Helper
	{
		internal static ListBoxItem FindListViewItem(this DependencyObject source)
		{
			DependencyObject dep = source;

			if (dep is Hyperlink)
				return FindListViewItem((dep as Hyperlink).Parent);

			if (dep is Run)
				return FindListViewItem((dep as Run).Parent);

			while ((dep != null) && !(dep is ListBoxItem))
			{
				dep = VisualTreeHelper.GetParent(dep);
			}

			if (dep == null) return null;

			return (ListBoxItem)dep;
		}
	}
}
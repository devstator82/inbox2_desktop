using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inbox2.Framework.UI.DragDrop
{
	public class ListViewDragDropDataProvider : IDataDropObjectProvider
	{
		protected ListView listView;

		public ListViewDragDropDataProvider(ListView listView)
		{
			this.listView = listView;
		}

		public DragDropProviderActions SupportedActions
		{
			get
			{
				return DragDropProviderActions.Data | DragDropProviderActions.Visual | DragDropProviderActions.Unparent | DragDropProviderActions.MultiFormatData;
			}
		}

		public object GetData()
		{
			return listView.SelectedItem;
		}

		public void AppendData(ref IDataObject data, MouseEventArgs e)
		{
			object selected = listView.SelectedItem;

			if (selected != null)
				data.SetData(selected.GetType().ToString(), selected); 
		}

		public UIElement GetVisual(MouseEventArgs e)
		{
			return listView.ItemContainerGenerator.ContainerFromItem(listView.SelectedItem) as UIElement;
		}

		public void GiveFeedback(GiveFeedbackEventArgs args)
		{
			throw new System.NotImplementedException();
		}

		public void ContinueDrag(QueryContinueDragEventArgs args)
		{
			throw new System.NotImplementedException();
		}

		public bool UnParent()
		{
			throw new System.NotImplementedException();
		}
	}
}

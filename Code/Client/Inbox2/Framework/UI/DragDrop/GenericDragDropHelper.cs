using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inbox2.Framework.UI.DragDrop
{
	public class GenericDragDropHelper : IDataDropObjectProvider
	{
		private readonly Control control;

		public GenericDragDropHelper(Control control)
		{
			this.control = control;
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
			return control.DataContext;
		}

		public void AppendData(ref IDataObject data, MouseEventArgs e)
		{
			if (control.DataContext != null)
				data.SetData(control.DataContext.GetType().ToString(), control.DataContext); 
		}

		public UIElement GetVisual(MouseEventArgs e)
		{
			return control;
		}

		public void GiveFeedback(GiveFeedbackEventArgs args)
		{
			throw new NotImplementedException();
		}

		public void ContinueDrag(QueryContinueDragEventArgs args)
		{
			throw new NotImplementedException();
		}

		public bool UnParent()
		{
			throw new NotImplementedException();
		}
	}
}

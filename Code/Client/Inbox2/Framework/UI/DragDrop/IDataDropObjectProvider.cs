using System.Windows;
using System.Windows.Input;

namespace Inbox2.Framework.UI.DragDrop
{
	public interface IDataDropObjectProvider
	{
		//Flag of actions sypported by implementation of  IDataDropObjectProvider
		DragDropProviderActions SupportedActions { get; }

		// Called before StartDrag () to get the Data () to be used in the DataObject 
		object GetData();

		// Called before StartDrag () to add other formats , this way you can drag drop externally.. 
		void AppendData(ref IDataObject data, MouseEventArgs e);
		
		// Called to get the visual ( UIElement visual brush of the object being dragged..         
		UIElement GetVisual(MouseEventArgs e);
		
		// Gives feedback during Drag 
		void GiveFeedback(GiveFeedbackEventArgs args);
		
		// implements ContinueDrag -- to canceld the D&D.. 
		void ContinueDrag(QueryContinueDragEventArgs args);
		
		// called by the TARGET object .. this will attempt to "unparent" the current child so we can add it a child some where else.. 
		bool UnParent();
	}
}
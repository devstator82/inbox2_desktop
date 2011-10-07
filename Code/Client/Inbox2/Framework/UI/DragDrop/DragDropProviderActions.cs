namespace Inbox2.Framework.UI.DragDrop
{
	public enum DragDropProviderActions
	{
		None = 0,
		Data = 1,
		Visual = 2,
		Feedback = 4,
		ContinueDrag = 8,
		Clone = 16,
		MultiFormatData = 32,
		// 64, 128  left for decent operations 
		// unparent feels hacky 
		Unparent = 256,
	}
}
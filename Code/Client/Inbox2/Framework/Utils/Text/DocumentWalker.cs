using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace Inbox2.Framework.Utils.Text
{
	/// <summary>
	/// THe delegate type of the event that will be raised
	/// </summary>
	public delegate void DocumentVisitedEventHandler(object sender, object visitedObject, bool start);

	public class DocumentWalker
	{
		/// <summary>
		/// This is the event to hook on.
		/// </summary>
		public event DocumentVisitedEventHandler VisualVisited;

		/// <summary>
		/// Traverses  whole document
		/// </summary>
		/// <param name="fd"></param>
		public void Walk(FlowDocument fd)
		{
			TraverseBlockCollection(fd.Blocks);
		}

		/// <summary>
		/// Traverses only passed paragraph
		/// </summary>
		/// <param name="p"></param>
		public void TraverseParagraph(Paragraph p)
		{
			if (p.Inlines != null && p.Inlines.Count > 0)
			{
				Inline il = p.Inlines.FirstInline;
				while (il != null)
				{
					Run r = il as Run;
					if (r != null)
					{
						VisualVisited(this, r, true);
						il = il.NextInline;
						continue;
					}
					InlineUIContainer uc = il as InlineUIContainer;
					if (uc != null && uc.Child != null)
					{
						VisualVisited(this, uc.Child, true);
						il = il.NextInline;
						continue;
					}
					Figure fg = il as Figure;
					if (fg != null)
					{
						TraverseBlockCollection(fg.Blocks);
					}
					il = il.NextInline;
				}
			}
		}

		/// <summary>
		/// Traverses passed block collection
		/// </summary>
		/// <param name="blocks"></param>
		public void TraverseBlockCollection(BlockCollection blocks)
		{
			foreach (Block b in blocks)
			{
				Paragraph p = b as Paragraph;
				if (p != null)
					TraverseParagraph(p);
				else
				{
					BlockUIContainer bui = b as BlockUIContainer;
					if (bui != null)
					{
						VisualVisited(this, bui.Child, true);
					}
					else
					{
						Section s = b as Section;
						if (s != null)
							TraverseBlockCollection(s.Blocks);
						else
						{
							Table t = b as Table;
							if (t != null)
							{
								VisualVisited(this, t, true);
								foreach (TableRowGroup trg in t.RowGroups)
								{
									foreach (TableRow tr in trg.Rows)
									{
										foreach (TableCell tc in tr.Cells)
											TraverseBlockCollection(tc.Blocks);
									}
								}
								VisualVisited(this, t, false);
							}
						}
					}
				}

			}
		}
	}

}

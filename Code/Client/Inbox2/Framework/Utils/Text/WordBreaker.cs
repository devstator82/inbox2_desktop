using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace Inbox2.Framework.Utils.Text
{
	public static class WordBreaker
	{
		/// <summary>
		/// Returns a TextRange covering a word containing or following this TextPointer.
		/// </summary>
		/// <remarks>
		/// If this TextPointer is within a word or at start of word, the containing word range is returned.
		/// If this TextPointer is between two words, the following word range is returned.
		/// If this TextPointer is at trailing word boundary, the following word range is returned.
		/// </remarks>
		public static TextRange GetWordRange(TextPointer position)
		{
			TextRange wordRange = null;
			TextPointer wordStartPosition = null;
			TextPointer wordEndPosition = null;

			// Go forward first, to find word end position.
			wordEndPosition = GetPositionAtWordBoundary(position, /*wordBreakDirection*/LogicalDirection.Forward);

			if (wordEndPosition != null)
			{
				// Then travel backwards, to find word start position.
				wordStartPosition = GetPositionAtWordBoundary(wordEndPosition, /*wordBreakDirection*/LogicalDirection.Backward);
			}

			if (wordStartPosition != null && wordEndPosition != null)
			{
				wordRange = new TextRange(wordStartPosition, wordEndPosition);
			}

			return wordRange;
		}

		/// <summary>
		/// 1.  When wordBreakDirection = Forward, returns a position at the end of the word,
		///     i.e. a position with a wordBreak character (space) following it.
		/// 2.  When wordBreakDirection = Backward, returns a position at the start of the word,
		///     i.e. a position with a wordBreak character (space) preceeding it.
		/// 3.  Returns null when there is no workbreak in the requested direction.
		/// </summary>
		private static TextPointer GetPositionAtWordBoundary(TextPointer position, LogicalDirection wordBreakDirection)
		{
			if (!position.IsAtInsertionPosition)
			{
				position = position.GetInsertionPosition(wordBreakDirection);
			}

			TextPointer navigator = position;
			while (navigator != null && !IsPositionNextToWordBreak(navigator, wordBreakDirection))
			{
				navigator = navigator.GetNextInsertionPosition(wordBreakDirection);
			}

			return navigator;
		}

		// Helper for GetPositionAtWordBoundary.
		// Returns true when passed TextPointer is next to a wordBreak in requested direction.
		private static bool IsPositionNextToWordBreak(TextPointer position, LogicalDirection wordBreakDirection)
		{
			bool isAtWordBoundary = false;

			// Skip over any formatting.
			if (position.GetPointerContext(wordBreakDirection) != TextPointerContext.Text)
			{
				position = position.GetInsertionPosition(wordBreakDirection);
			}

			if (position.GetPointerContext(wordBreakDirection) == TextPointerContext.Text)
			{
				LogicalDirection oppositeDirection = (wordBreakDirection == LogicalDirection.Forward) ?
					LogicalDirection.Backward : LogicalDirection.Forward;

				char[] runBuffer = new char[1];
				char[] oppositeRunBuffer = new char[1];

				position.GetTextInRun(wordBreakDirection, runBuffer, /*startIndex*/0, /*count*/1);
				position.GetTextInRun(oppositeDirection, oppositeRunBuffer, /*startIndex*/0, /*count*/1);

				if (runBuffer[0] == ' ' && !(oppositeRunBuffer[0] == ' '))
				{
					isAtWordBoundary = true;
				}
			}
			else
			{
				// If we're not adjacent to text then we always want to consider this position a "word break".  
				// In practice, we're most likely next to an embedded object or a block boundary.
				isAtWordBoundary = true;
			}

			return isAtWordBoundary;
		}
	}
}

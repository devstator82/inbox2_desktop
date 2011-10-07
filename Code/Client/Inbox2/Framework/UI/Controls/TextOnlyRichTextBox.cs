using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Inbox2.Framework.UI.Controls
{
	public class TextOnlyRichTextBox : RichTextBox
	{
		// Static list of editing formatting commands. In the ctor we disable all these commands.
		private static readonly RoutedUICommand[] _formattingCommands = new[]
            {
                EditingCommands.ToggleBold,
                EditingCommands.ToggleItalic,
                EditingCommands.ToggleUnderline,
                EditingCommands.ToggleSubscript,
                EditingCommands.ToggleSuperscript,
                EditingCommands.IncreaseFontSize,
                EditingCommands.DecreaseFontSize,
                EditingCommands.ToggleBullets,
                EditingCommands.ToggleNumbering,
            };	    

		static TextOnlyRichTextBox()
        {
			// Register command handlers for all rich text formatting commands.
			// We disable all commands by returning false in OnCanExecute event handler,
			// thus making this control a "plain text only" RichTextBox.
			foreach (var command in _formattingCommands)
				CommandManager.RegisterClassCommandBinding(typeof(TextOnlyRichTextBox), new CommandBinding(command, OnFormattingCommand, OnCanExecuteFormattingCommand));

			// Command handlers for Cut, Copy and Paste commands.
			// To enforce that data can be copied or pasted from the clipboard in text format only.
			CommandManager.RegisterClassCommandBinding(typeof(TextOnlyRichTextBox), new CommandBinding(ApplicationCommands.Copy, OnCopy, OnCanExecuteCopy));
			CommandManager.RegisterClassCommandBinding(typeof(TextOnlyRichTextBox), new CommandBinding(ApplicationCommands.Paste, OnPaste, OnCanExecutePaste));
			CommandManager.RegisterClassCommandBinding(typeof(TextOnlyRichTextBox), new CommandBinding(ApplicationCommands.Cut, OnCut, OnCanExecuteCut));
        }

		/// <summary>
		/// Event handler for all formatting commands.
		/// </summary>
		private static void OnFormattingCommand(object sender, ExecutedRoutedEventArgs e)
		{
			// Do nothing, and set command handled to true.
			e.Handled = true;
		}

		/// <summary>
		/// Event handler for ApplicationCommands.Copy command.
		/// <remarks>
		/// We want to enforce that data can be set on the clipboard 
		/// only in plain text format from this RichTextBox.
		/// </remarks>
		/// </summary>
		private static void OnCopy(object sender, ExecutedRoutedEventArgs e)
		{
			TextOnlyRichTextBox TextOnlyRichTextBox = (TextOnlyRichTextBox)sender;
			string selectionText = TextOnlyRichTextBox.Selection.Text;
			var range = new TextRange(TextOnlyRichTextBox.Selection.Start, TextOnlyRichTextBox.Selection.End);

			if (String.IsNullOrEmpty(selectionText.Trim()))
			{
				
			}
			else
			{
				Clipboard.SetText(selectionText);
			}
			
			e.Handled = true;
		}

		/// <summary>
		/// Event handler for ApplicationCommands.Cut command.
		/// <remarks>
		/// We want to enforce that data can be set on the clipboard 
		/// only in plain text format from this RichTextBox.
		/// </remarks>
		/// </summary>
		private static void OnCut(object sender, ExecutedRoutedEventArgs e)
		{
			TextOnlyRichTextBox TextOnlyRichTextBox = (TextOnlyRichTextBox)sender;
			string selectionText = TextOnlyRichTextBox.Selection.Text;

			if (String.IsNullOrEmpty(selectionText.Trim()))
			{
				
			}
			else
			{
				Clipboard.SetText(selectionText);
			}
						
			TextOnlyRichTextBox.Selection.Text = String.Empty;
			
			e.Handled = true;
		}

		/// <summary>
		/// Event handler for ApplicationCommands.Paste command.
		/// <remarks>
		/// We want to allow paste only in plain text format.
		/// </remarks>
		/// </summary>
		private static void OnPaste(object sender, ExecutedRoutedEventArgs e)
		{
			TextOnlyRichTextBox TextOnlyRichTextBox = (TextOnlyRichTextBox)sender;

			// Handle paste only if clipboard supports text format.
			if (Clipboard.ContainsText())
			{
				TextOnlyRichTextBox.Selection.Text = Clipboard.GetText();
			}
			e.Handled = true;
		}

		/// <summary>
		/// CanExecute event handler.
		/// </summary>
		private static void OnCanExecuteFormattingCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
		}

		/// <summary>
		/// CanExecute event handler for ApplicationCommands.Copy.
		/// </summary>
		private static void OnCanExecuteCopy(object target, CanExecuteRoutedEventArgs args)
		{
			TextOnlyRichTextBox TextOnlyRichTextBox = (TextOnlyRichTextBox)target;
			args.CanExecute = TextOnlyRichTextBox.IsEnabled && !TextOnlyRichTextBox.Selection.IsEmpty;
		}

		/// <summary>
		/// CanExecute event handler for ApplicationCommands.Cut.
		/// </summary>
		private static void OnCanExecuteCut(object target, CanExecuteRoutedEventArgs args)
		{
			TextOnlyRichTextBox TextOnlyRichTextBox = (TextOnlyRichTextBox)target;
			args.CanExecute = TextOnlyRichTextBox.IsEnabled && !TextOnlyRichTextBox.IsReadOnly && !TextOnlyRichTextBox.Selection.IsEmpty;
		}

		/// <summary>
		/// CanExecute event handler for ApplicationCommand.Paste.
		/// </summary>
		private static void OnCanExecutePaste(object target, CanExecuteRoutedEventArgs args)
		{
			TextOnlyRichTextBox TextOnlyRichTextBox = (TextOnlyRichTextBox)target;
			args.CanExecute = TextOnlyRichTextBox.IsEnabled && !TextOnlyRichTextBox.IsReadOnly && Clipboard.ContainsText();
		}
	}
}

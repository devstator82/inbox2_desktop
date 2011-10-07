using System;
using System.Windows;
using System.Windows.Input;
using Inbox2.Framework.UI.Input;

namespace Inbox2.Framework.UI
{
    public static class InputCommands
    {
        public static RoutedUICommand Escape = new RoutedUICommand();
		public static RoutedUICommand Refresh = new RoutedUICommand();
        public static RoutedUICommand Compose = new RoutedUICommand();
        public static RoutedUICommand Search = new RoutedUICommand();
		public static RoutedUICommand UploadDocment = new RoutedUICommand();
		public static RoutedUICommand UpdateStatus = new RoutedUICommand();
        
        public static RoutedUICommand NewerConversation = new RoutedUICommand();
        public static RoutedUICommand OlderConversation = new RoutedUICommand();
        
        public static RoutedUICommand NextMessage = new RoutedUICommand();
        public static RoutedUICommand PreviousMessage = new RoutedUICommand();

        public static RoutedUICommand FirstImportant = new RoutedUICommand();
        public static RoutedUICommand NextImportant = new RoutedUICommand();
		
		public static RoutedCommand Todo = new RoutedCommand();
		public static RoutedCommand WaitingFor = new RoutedCommand();
		public static RoutedCommand Someday = new RoutedCommand();
		public static RoutedCommand ClearAction = new RoutedCommand();

        public static RoutedUICommand Archive = new RoutedUICommand();
        public static RoutedUICommand Star = new RoutedUICommand();
        
        public static RoutedUICommand InlineReply = new RoutedUICommand();
        public static RoutedUICommand Reply = new RoutedUICommand();
        public static RoutedUICommand InlineForward = new RoutedUICommand();
        public static RoutedUICommand Forward = new RoutedUICommand();
        public static RoutedUICommand AddLabel = new RoutedUICommand();
        public static RoutedUICommand MarkRead = new RoutedUICommand();
        public static RoutedUICommand MarkUnread = new RoutedUICommand();

        public static RoutedUICommand RemoveFromCurrentView = new RoutedUICommand();        
        public static RoutedUICommand SaveDraft = new RoutedUICommand();

        public static RoutedUICommand GotoInbox = new RoutedUICommand();
        public static RoutedUICommand GotoArchive = new RoutedUICommand();
        public static RoutedUICommand GotoSentItems = new RoutedUICommand();
        public static RoutedUICommand GotoDrafts = new RoutedUICommand();
        public static RoutedUICommand GotoStarred = new RoutedUICommand();
        public static RoutedUICommand GotoLabel = new RoutedUICommand();
        public static RoutedUICommand GotoContacts = new RoutedUICommand();
        public static RoutedUICommand GotoDocuments = new RoutedUICommand();
        public static RoutedUICommand GotoImages = new RoutedUICommand();

        public static RoutedUICommand ToggleChannel1 = new RoutedUICommand();
        public static RoutedUICommand ToggleChannel2 = new RoutedUICommand();
        public static RoutedUICommand ToggleChannel3 = new RoutedUICommand();
        public static RoutedUICommand ToggleChannel4 = new RoutedUICommand();
        public static RoutedUICommand ToggleChannel5 = new RoutedUICommand();
        public static RoutedUICommand ToggleChannel6 = new RoutedUICommand();
        public static RoutedUICommand ToggleChannel7 = new RoutedUICommand();
        public static RoutedUICommand ToggleChannel8 = new RoutedUICommand();
        public static RoutedUICommand ToggleChannel9 = new RoutedUICommand();

        public static RoutedUICommand StatusUpdate1 = new RoutedUICommand();
        public static RoutedUICommand StatusUpdate2 = new RoutedUICommand();
        public static RoutedUICommand StatusUpdate3 = new RoutedUICommand();
        public static RoutedUICommand StatusUpdate4 = new RoutedUICommand();
        public static RoutedUICommand StatusUpdate5 = new RoutedUICommand();
        public static RoutedUICommand StatusUpdate6 = new RoutedUICommand();
        public static RoutedUICommand StatusUpdate7 = new RoutedUICommand();
        public static RoutedUICommand StatusUpdate8 = new RoutedUICommand();
        public static RoutedUICommand StatusUpdate9 = new RoutedUICommand();

        static InputCommands()
        {
			Refresh.InputGestures.Add(new CustomGesture(Key.F5));
            Escape.InputGestures.Add(new CustomGesture(Key.Escape));
            Compose.InputGestures.Add(new CustomGesture(Key.C));
            Search.InputGestures.Add(new CustomGesture(Key.Oem2));

			Compose.InputGestures.Add(new MultiKeyGesture(new[] { Key.N, Key.M }, ModifierKeys.None));
			UploadDocment.InputGestures.Add(new MultiKeyGesture(new[] { Key.N, Key.D }, ModifierKeys.None));
			UpdateStatus.InputGestures.Add(new MultiKeyGesture(new[] { Key.N, Key.S }, ModifierKeys.None));

            NewerConversation.InputGestures.Add(new CustomGesture(Key.K));
            OlderConversation.InputGestures.Add(new CustomGesture(Key.J));
            NextMessage.InputGestures.Add(new CustomGesture(Key.N));
            PreviousMessage.InputGestures.Add(new CustomGesture(Key.P));

            FirstImportant.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.F }, ModifierKeys.None));
            NextImportant.InputGestures.Add(new CustomGesture(Key.Space));

			Todo.InputGestures.Add(new MultiKeyGesture(new[] { Key.A, Key.T }, ModifierKeys.None));
			Someday.InputGestures.Add(new MultiKeyGesture(new[] { Key.A, Key.S }, ModifierKeys.None));
			WaitingFor.InputGestures.Add(new MultiKeyGesture(new[] { Key.A, Key.W }, ModifierKeys.None));
			ClearAction.InputGestures.Add(new MultiKeyGesture(new[] { Key.A, Key.C }, ModifierKeys.None));

            Archive.InputGestures.Add(new CustomGesture(Key.E));
            Star.InputGestures.Add(new CustomGesture(Key.S));
            InlineReply.InputGestures.Add(new CustomGesture(Key.R));
            Reply.InputGestures.Add(new CustomGesture(Key.R, ModifierKeys.Shift));
            InlineForward.InputGestures.Add(new CustomGesture(Key.F));
            Forward.InputGestures.Add(new CustomGesture(Key.F, ModifierKeys.Shift));
            AddLabel.InputGestures.Add(new CustomGesture(Key.L));
            MarkRead.InputGestures.Add(new CustomGesture(Key.I, ModifierKeys.Shift));
            MarkUnread.InputGestures.Add(new CustomGesture(Key.U, ModifierKeys.Shift));

            RemoveFromCurrentView.InputGestures.Add(new CustomGesture(Key.Y));
            SaveDraft.InputGestures.Add(new CustomGesture(Key.S, ModifierKeys.Control));

            GotoInbox.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.I }, ModifierKeys.None));
            GotoArchive.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.A }, ModifierKeys.None));
            GotoSentItems.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.T }, ModifierKeys.None));
            GotoDrafts.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.D }, ModifierKeys.None));
            GotoStarred.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.S }, ModifierKeys.None));
            GotoLabel.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.L }, ModifierKeys.None));
            GotoContacts.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.C }, ModifierKeys.None));
            GotoDocuments.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.O }, ModifierKeys.None));
            GotoImages.InputGestures.Add(new MultiKeyGesture(new[] { Key.G, Key.M }, ModifierKeys.None));

            ToggleChannel1.InputGestures.Add(new CustomGesture(Key.D1, ModifierKeys.Control));
			ToggleChannel2.InputGestures.Add(new CustomGesture(Key.D2, ModifierKeys.Control));
			ToggleChannel3.InputGestures.Add(new CustomGesture(Key.D3, ModifierKeys.Control));
			ToggleChannel4.InputGestures.Add(new CustomGesture(Key.D4, ModifierKeys.Control));
			ToggleChannel5.InputGestures.Add(new CustomGesture(Key.D5, ModifierKeys.Control));
			ToggleChannel6.InputGestures.Add(new CustomGesture(Key.D6, ModifierKeys.Control));
			ToggleChannel7.InputGestures.Add(new CustomGesture(Key.D7, ModifierKeys.Control));
			ToggleChannel8.InputGestures.Add(new CustomGesture(Key.D8, ModifierKeys.Control));
			ToggleChannel9.InputGestures.Add(new CustomGesture(Key.D9, ModifierKeys.Control));

            StatusUpdate1.InputGestures.Add(new CustomGesture(Key.D1));
            StatusUpdate2.InputGestures.Add(new CustomGesture(Key.D2));
            StatusUpdate3.InputGestures.Add(new CustomGesture(Key.D3));
            StatusUpdate4.InputGestures.Add(new CustomGesture(Key.D4));
            StatusUpdate5.InputGestures.Add(new CustomGesture(Key.D5));
            StatusUpdate6.InputGestures.Add(new CustomGesture(Key.D6));
            StatusUpdate7.InputGestures.Add(new CustomGesture(Key.D7));
            StatusUpdate8.InputGestures.Add(new CustomGesture(Key.D8));
            StatusUpdate9.InputGestures.Add(new CustomGesture(Key.D9));
        }

        public static void Stub(UIElement control)
        {
            control.CommandBindings.Add(new CommandBinding(Escape, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(Compose, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(Search, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(NewerConversation, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(OlderConversation, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(NextMessage, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(PreviousMessage, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(Archive, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(Star, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(InlineReply, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(Reply, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(InlineForward, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(Forward, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(AddLabel, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(MarkRead, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(MarkUnread, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(RemoveFromCurrentView, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(SaveDraft, Command_Executed, Command_CanExecute));

            control.CommandBindings.Add(new CommandBinding(GotoInbox, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(GotoArchive, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(GotoSentItems, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(GotoDrafts, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(GotoStarred, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(GotoLabel, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(GotoContacts, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(GotoDocuments, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(GotoImages, Command_Executed, Command_CanExecute));

            control.CommandBindings.Add(new CommandBinding(ToggleChannel1, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(ToggleChannel2, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(ToggleChannel3, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(ToggleChannel4, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(ToggleChannel5, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(ToggleChannel6, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(ToggleChannel7, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(ToggleChannel8, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(ToggleChannel9, Command_Executed, Command_CanExecute));

            control.CommandBindings.Add(new CommandBinding(StatusUpdate1, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(StatusUpdate2, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(StatusUpdate3, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(StatusUpdate4, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(StatusUpdate5, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(StatusUpdate6, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(StatusUpdate7, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(StatusUpdate8, Command_Executed, Command_CanExecute));
            control.CommandBindings.Add(new CommandBinding(StatusUpdate9, Command_Executed, Command_CanExecute));
        }

        static void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        static void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}

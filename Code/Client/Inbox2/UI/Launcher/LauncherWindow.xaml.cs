using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Inbox2.Framework;
using Inbox2.Framework.Collections;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interop;
using Inbox2.Framework.Plugins;
using Inbox2.Framework.UI;
using Inbox2.UI.Resources;

namespace Inbox2.UI.Launcher
{
	/// <summary>
	/// Interaction logic for LauncherWindow.xaml
	/// </summary>
	public partial class LauncherWindow : Window
	{
		public List<LauncherCommand> LauncherCommands
		{
			get { return LauncherState.Current.LauncherCommands; }
		}

		public LauncherWindow()
		{
			InitializeComponent();

			DataContext = this;
		}

		void LauncherWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(LauncherState.Current.LauncherCommands);
			view.Filter = CommandFilter;

			IntPtr windowHandle = new WindowInteropHelper(this).Handle;

			WinApi.SetWindowPos(windowHandle, WinApi.HWND_TOPMOST, 0, 0, 0, 0, WinApi.TOPMOST_FLAGS);

			FocusHelper.Focus(SearchTextBox);
			
			SearchTextBox.TextChanged += delegate { UpdateCommands(); };
			SearchTextBox.Recipients.CollectionChanged += delegate { UpdateCommands(); };
		}

		void Launcher_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}

		void Window_SourceInitialized(object sender, EventArgs e)
		{
			IntPtr windowHandle = new WindowInteropHelper(this).Handle;

			LauncherState.Current.SelectedFiles.Clear();
			LauncherState.Current.SelectedAddresses.Clear();
			LauncherState.Current.SelectedUris.Clear();

			CommandsListView.SelectedIndex = 0;

			var items = Desktop.GetSelectedItems(windowHandle);
			int count = items.Count;

			foreach (var item in items)
			{
				if (item is BrowserSelectedUrl)
				{
					BrowserSelectedUrl uri = (BrowserSelectedUrl)item;

					LauncherState.Current.SelectedUris.Add(new Uri(uri.Url));

					Title = uri.Url;
				}

				if (item is ExplorerSelectedFile)
				{
					ExplorerSelectedFile file = (ExplorerSelectedFile)item;

					FileInfo fi = file.GetFileInfo();

					if (fi != null)
					{
						LauncherState.Current.SelectedFiles.Add(fi);

						Title = fi.Name;
					}
				}
			}

			// Update top titlebar
			if (count > 0)
			{
				if (count == 1)
				{
					string filename = items[0] is ExplorerSelectedFile
										? ((ExplorerSelectedFile)items[0]).GetFileInfo().Name
										: "somefile.html";

					Icon = (ImageSource)new FilenameToIconConverter().Convert(filename, null, null, null);
				}
				else
				{
					Title = String.Format("{0} files selected", count);

					Icon = null;
				}
			}
		}

		void UpdateCommands()
		{
			LauncherState.Current.SelectedAddresses.Replace(SearchTextBox.Recipients);

			foreach (var command in LauncherState.Current.LauncherCommands)
				command.UpdateDescription(SearchTextBox.Text);
		}

		bool CommandFilter(object source)
		{
			LauncherCommand command = (LauncherCommand)source;

			return command.CanExecute(SearchTextBox.Text);
		}

		void ResizeLauncherWindow()
		{
			//Storyboard sb = FindResource("ResizeLauncherWindow") as Storyboard;
			//DoubleAnimationUsingKeyFrames animation = sb.Children[0] as DoubleAnimationUsingKeyFrames;
			//DoubleKeyFrameCollection keyFrames = animation.KeyFrames;

			//if (keyFrames.Count > 0)
			//{
			//    SplineDoubleKeyFrame keyFrame = keyFrames[0] as SplineDoubleKeyFrame;
			//    keyFrame.Value = 75 + CommandsListView.Items.Count * 35;
			//}

			//sb.Begin(this);

			Height = 75 + CommandsListView.Items.Count*35;
		}

		void AnimationWindow_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Up && !SearchTextBox.IsPopupOpen)
			{
				if (CommandsListView.SelectedIndex <= 0)
					return;

				CommandsListView.SelectedIndex--;
			}

			if (e.Key == Key.Down && !SearchTextBox.IsPopupOpen)
			{
				CommandsListView.SelectedIndex++;
			}

			if (e.Key == Key.Enter && !SearchTextBox.IsPopupOpen)
			{
				LauncherCommand command = (LauncherCommand)CommandsListView.SelectedItem;

				Dispatcher.BeginInvoke((Action)(() => command.Execute(SearchTextBox.Text)));

				Close();

				// Prevents the RecipientEditorControl from receiving this key when we're closing the window
				e.Handled = true;
			}

			if (e.Key == Key.Escape)
			{
				if (SearchTextBox.SuppressListForCurrentWord || !SearchTextBox.IsPopupOpen)
					Close();

				SearchTextBox.SuppressListForCurrentWord = true;
			}
		}

		void Launcher_LocationChanged(object sender, EventArgs e)
		{
			SearchTextBox.HideList();
		}

		void CommandsListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var command = (e.OriginalSource as DependencyObject)
				.FindListViewItem<LauncherCommand>(CommandsListView.ItemContainerGenerator);

			if (command != null)
			{
				Dispatcher.BeginInvoke((Action)(() => command.Execute(SearchTextBox.Text)));

				Close();
			}
		}

		void CommandsListView_LayoutUpdated(object sender, EventArgs e)
		{
			ResizeLauncherWindow();
		}
	}
}

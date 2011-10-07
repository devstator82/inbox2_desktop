using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.StatusUpdates.Helpers
{
	internal static class CommandHelper
	{
		public static bool CanSendExecute(CanExecuteRoutedEventArgs e)
		{
			if (e.Parameter == null)
				return false;
			else
			{
				var tb = (TextBox)e.Parameter;

				return !String.IsNullOrEmpty(tb.Text.Trim());
			}
		}

		internal static bool CanShortenUrlExecute(CanExecuteRoutedEventArgs e)
		{
			if (e.Parameter == null)
				return false;
			else
			{
				var tb = (TextBox)e.Parameter;

				return !String.IsNullOrEmpty(tb.Text.Trim());
			}
		}

		internal static void Send(IEnumerable<ChannelInstance> channels, string inreplyTo, ExecutedRoutedEventArgs e)
		{
			var mailbox = VirtualMailBox.Current;
			var tb = (TextBox)e.Parameter;
			var channelsArray = channels
				.Where(s => s != null)
				.Select(s => s.Configuration.ChannelId.ToString());

			var status = new UserStatus
			{
				Status = tb.Text.Trim(),
				StatusType = StatusTypes.MyUpdate,
				SortDate = DateTime.Now,
				InReplyTo = inreplyTo,
				DateCreated = DateTime.Now,
				TargetChannelId = String.Join(";", channelsArray.ToArray())
			};

			mailbox.StatusUpdates.Add(status);

			ClientState.Current.DataService.Save(status);

			// Save command
			CommandQueue.Enqueue(AppCommands.SendStatusUpdate, status);

			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				ClientState.Current.ShowMessage(
					new AppMessage(Strings.StatusWillBeUpdatedLater)
					{
						EntityId = status.StatusId,
						EntityType = EntityType.UserStatus
					}, MessageType.Success);
			}

			tb.Text = String.Empty;
		}

		internal static void SendExecute(ChannelInstance sourceChannel, ExecutedRoutedEventArgs e)
		{
			var source = (ButtonBase)e.OriginalSource;			
			var inreplyto = source.DataContext as UserStatus;

			// Reply to parent if that is available, othwerwise facebook replies
			// will break down.
			if (inreplyto != null && inreplyto.Parent != null)
				inreplyto = inreplyto.Parent;

			var channel = inreplyto == null ? sourceChannel :
				ChannelsManager.GetChannelObject(inreplyto.SourceChannel.ChannelId);

			Send(new[] { channel }, inreplyto == null ? null : inreplyto.ChannelStatusKey, e);
		}

		internal static void ShortenUrlExecute(ExecutedRoutedEventArgs e)
		{
			var tb = (TextBox)e.Parameter;

			tb.Text = UrlShortenerService.Shorten(tb.Text);
		}
	}
}

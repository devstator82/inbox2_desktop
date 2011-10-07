using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Plugins.SharedControls;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.Conversations.Controls;
using Inbox2.Plugins.Conversations.Helpers;

namespace Inbox2.Plugins.Conversations
{
	[Export(typeof(PluginPackage))]
	public class ConversationsPlugin : PluginPackage
	{
		private readonly ConversationsState state;
		
		private StreamView streamView;
		private ActivityStreamViewHelper view1;
		private SingleLineViewHelper view2;

		public override string Name
		{
			get { return "Conversations"; }
		}

		public override IStatePlugin State
		{
			get { return state; }
		}

		public override IEnumerable<IStreamViewPlugin> StreamViews
		{
			get
			{
				BuildViews();

				return new IStreamViewPlugin[] { view1, view2 };
			}
		}

		public override IColumnPlugin Colomn
		{
			get { return new PluginHelper(state); }
		}

		public override IDetailsViewPlugin DetailsView
		{
			get { return new PluginHelper(state); }
		}

		public override INewItemViewPlugin NewItemView
		{
			get { return new NewItemsPluginHelper(state); }
		}

		public ConversationsPlugin()
		{
			state = new ConversationsState();

			EventBroker.Subscribe<Message>(AppEvents.New, delegate { State.New(); });
			EventBroker.Subscribe(AppEvents.New, delegate(SourceAddress address)
             	{
					ClientState.Current.ViewController.MoveTo(
						PluginsManager.Current.GetPlugin<ConversationsPlugin>().NewItemView,
							new NewMessageDataHelper { To = address.ToList() });
             	});

			EventBroker.Subscribe(AppEvents.New, delegate(string url)
				{
					ClientState.Current.ViewController.MoveTo(
						PluginsManager.Current.GetPlugin<ConversationsPlugin>().NewItemView,
							NewMessageDataHelper.Parse(url));
				});

			EventBroker.Subscribe(AppEvents.View, delegate(Message message)
	           	{
	           		if (message.MessageFolder == Folders.Drafts)
					{
						var access = new ClientMessageAccess(message);

						ClientState.Current.ViewController.MoveTo(
							PluginsManager.Current.GetPlugin<ConversationsPlugin>().NewItemView,
								new NewMessageDataHelper
									{
										SourceMessageId = message.MessageId,
										Context = message.Context,
										To = message.To,
										Cc = message.CC,
										Bcc = message.BCC,
										Body = access.BodyHtml,
										SelectedChannelId = message.SourceChannelId,
										AttachedFiles = message.Attachments.Select(a => new AttachmentDataHelper(a)).ToList(),
										SuppressSignature = true
									}
								);
					}
					else
					{
						state.SelectedMessages.Replace(new[] { message });
						state.View();	
					}
	           	});

            EventBroker.Subscribe(AppEvents.RequestFirstImportant, delegate(Message message)
                {
                    state.SelectedMessages.Replace(new[] { message });

                    ClientState.Current.ViewController.MoveTo(
                        PluginsManager.Current.GetPlugin<ConversationsPlugin>().DetailsView,
                            new OverviewDataHelper { MessageId = message.MessageId.Value, MakeNavigatorCurrent = true });
                });
		}

		void BuildViews()
		{
			if (streamView == null)
			{
				// We initialize the streamview and keep a reference to it here because
				// both the ActivityStreamView and the SingleLineStreamView use this instace
				// and only effect the visual templates shown. This would not be the
				// best reference implementation for implementing stream views but in this
				// case it gets the job done.
				streamView = new StreamView();
				view1 = new ActivityStreamViewHelper(state, streamView);
				view2 = new SingleLineViewHelper(state, streamView);
			}
		}
	}
}
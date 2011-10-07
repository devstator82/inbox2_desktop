using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Inbox2.Framework.Localization;

namespace Inbox2.Framework.VirtualMailBox
{
	public class SearchKeyWords
	{
		private readonly object synclock = new object();
		private List<String> cached;

		public void Add(string channelname, string keyword)
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				Inbox2MessageBox.Show(Strings.OperationCouldNotBeCompletedDueToConnection, Inbox2MessageBoxButton.OK);
				return;
			}

			BuildKeywordsCache();

			lock (synclock)
			{
				cached.Add(String.Format("{0}|{1}", channelname, keyword).ToLower());

				ClientState.Current.Context.SaveSetting("/Settings/MailBox/SearchKeyword", cached);
			}			
		}

		public void Remove(string keyword)
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				Inbox2MessageBox.Show(Strings.OperationCouldNotBeCompletedDueToConnection, Inbox2MessageBoxButton.OK);
				return;
			}

			BuildKeywordsCache();

			lock (synclock)
			{
				cached.Remove(keyword.ToLower());

				ClientState.Current.Context.SaveSetting("/Settings/MailBox/SearchKeyword", cached);
			}
		}

		public List<string> GetKeyWords()
		{
			BuildKeywordsCache();

			lock (synclock)
			{
				return cached.ToList();
			}
		}

		void BuildKeywordsCache()
		{
			if (cached == null)
			{
				lock (synclock)
				{
					cached = ClientState.Current.Context.GetSetting("/Settings/MailBox/SearchKeyword") as List<String>;

					// Doesn't exist yet, create a new list
					if (cached == null)
						cached = new List<string>();
				}
			}
		}		
	}
}

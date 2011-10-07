using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Channels.Imap2
{
	internal static class FolderHelper
	{
		static string[] inbox = new[] { "Inbox", "Posteingang", "Boîte de réception", "Recibidos", "Postvak IN", "Innboks", "Indbakke", "Inkorgen", "Posta in arrivo" };
		static string[] sentItems = new[] { "Sent Mail", "Gesendet", "Messages envoyés", "Enviados", "Verzonden berichten", "Sendt e-post", "Sendte e-mails", "Skickade mail", "Posta inviata" };
		static string[] drafts = new[] { "Drafts ", "Entwürfe ", "Brouillons ", "Borradores ", "Concepten", "Utkast", "Kladder", "Utkast", "Bozze" };
		static string[] spam = new[] { "Spam", "Spam", "Spam", "Spam", "Spam", "Søppelpost", "Spam", "Skräppost", "Spam" };
		static string[] trash = new[] { "Trash", "Papierkorb", "Corbeille", "Papelera", "Prullenbak", "Papirkurv", "Papirkurv", "Papperskorgen", "Cestino" };
		static string[] archive = new[] { "All Mail", "Alle Nachrichten", "Tous les messages", "Todos", "Alle berichten", "All e-post", "Alle e-mails", "Alla mail", "Tutti i messaggi" };

		internal static ChannelFolderType GetImapFolderType(string name)
		{
			string[] parts = name.Split('/');
			string foldername = parts.Last();

			if (IsMatch(foldername, inbox))
				return ChannelFolderType.Inbox;

			if (IsMatch(foldername, sentItems))
				return ChannelFolderType.SentItems;

			if (IsMatch(foldername, drafts))
				return ChannelFolderType.Drafts;

			if (IsMatch(foldername, spam))
				return ChannelFolderType.Spam;

			if (IsMatch(foldername, trash))
				return ChannelFolderType.Trash;

			if (IsMatch(foldername, archive))
				return ChannelFolderType.Archive;

			return ChannelFolderType.Label;
		}

		static bool IsMatch(string foldername, string[] list)
		{
			foreach (var item in list)
			{
				if (item.Equals(foldername, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}

			return false;
		}
	}
}

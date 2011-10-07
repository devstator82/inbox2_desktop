using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using Inbox2.Platform.Channels.Configuration;
using LumiSoft.Net.Mime;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	[DataContract]
	public class SourceAddress : IComparable, IComparable<SourceAddress>, IEquatable<SourceAddress>
	{
		private string displayName;

		/// <summary>
		/// Helper for serialization.
		/// </summary>
		[DataMember(Order = 1)]
		public string SerializedAddress
		{
			get { return ToString(true); }
			set { Parse(value); }
		}

		[XmlIgnore]
		public string Address { get; private set; }		

		[XmlIgnore]
		public string DisplayName
		{
			get { return displayName; }
			private set
			{
				displayName = value != null && value.StartsWith("=?") ? 
					MimeUtils.DecodeWords(value) : value;
			}
		}

		[XmlIgnore]
		public string AvatarUrl { get; private set; }

		[XmlIgnore]
		public string ChannelName { get; private set; }

		[XmlIgnore]
		public string ProfileUrl { get; set; }

		public SourceAddress()
		{
			Address = String.Empty;
			DisplayName = String.Empty;
		}

		public SourceAddress(MailAddress mailaddress)
		{
			Address = mailaddress.Address;
			DisplayName = mailaddress.DisplayName;
		}

		public SourceAddress(string rawAddress, string displayName, string avatarUrl)
			: this(rawAddress, displayName)
		{
			this.AvatarUrl = avatarUrl;
		}

		public SourceAddress(string rawAddress, string displayName, string avatarUrl, string profileUrl)
			: this(rawAddress, displayName, avatarUrl)
		{
			this.ProfileUrl = profileUrl;
		}

		public SourceAddress(string rawAddress, string displayName)
			: this(rawAddress)
		{
			this.DisplayName = displayName;
		}

		public SourceAddress(string rawAddress)
		{
			Parse(rawAddress);
		}

		private void Parse(string rawAddress)
		{
			if (rawAddress == null) 
				throw new ArgumentNullException("rawAddress");

			rawAddress = rawAddress.Trim();

			var extendedPartIndex = rawAddress.IndexOf("|", StringComparison.InvariantCultureIgnoreCase);

			if (extendedPartIndex > -1)
			{
				var extendedPart = rawAddress.Substring(extendedPartIndex);
				var parts = extendedPart.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length > 0)
					AvatarUrl = parts[0];

				if (parts.Length > 1)
					ChannelName = parts[1];

				// Remove extended part from rawAddress and continue parsing
				rawAddress = rawAddress.Substring(0, extendedPartIndex);
			}

			if (rawAddress.Contains("<") && rawAddress.Contains(">"))
			{
				string[] parts = rawAddress.Split(new[] {'<', '>'}, StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length == 1)
				{
					this.Address = parts[0];
					this.DisplayName = parts[0];

					// See if the displayname is quoted, if so, remove the quotes
					if (DisplayName.StartsWith("\"") && DisplayName.EndsWith("\""))
						DisplayName = DisplayName.Substring(1, DisplayName.Length - 2);
				}
				if (parts.Length == 2)
				{					
					this.DisplayName = parts[0].Trim();
					this.Address = parts[1].Trim();

					// Can happen when you receive addresses like " <waseem@sadiq.nl>" (space in the beginning)
					if (String.IsNullOrEmpty(this.DisplayName))
						this.DisplayName = this.Address;

					// See if the displayname is quoted, if so, remove the quotes
					if (DisplayName.StartsWith("\"") && DisplayName.EndsWith("\""))
						DisplayName = DisplayName.Substring(1, DisplayName.Length - 2);

					return;
				}
			}
			else
			{
				this.DisplayName = rawAddress;
				this.Address = rawAddress;
			}

			if (String.IsNullOrEmpty(Address))
				this.Address = rawAddress;
		}

		public int CompareTo(object obj)
		{
			return CompareTo((SourceAddress)obj);
		}

		public int CompareTo(SourceAddress other)
		{
			return Address.CompareTo(other.Address);
		}

		public bool Equals(SourceAddress other)
		{
			return CompareTo(other) == 0;
		}

		public MailAddress ToMailAddress()
		{
			return new MailAddress(Address, DisplayName);
		}		

		public void SetChannel(ChannelConfiguration channel)
		{
			if (channel != null)
				ChannelName = channel.DisplayName;
		}

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToEncodedString()
		{
			return HttpUtility.HtmlEncode(ToString());
		}

		public string ToString(bool renderProperties)
		{
			StringBuilder sb = new StringBuilder();

			bool hasDisplayName = !String.IsNullOrEmpty(DisplayName) && !DisplayName.Equals(Address);

			if (hasDisplayName)
			{
				sb.Append(DisplayName);
				sb.AppendFormat(" <{0}>", Address);
			}
			else
			{
				sb.Append(Address);
			}

			if (renderProperties)
			{
				sb.Append("|");

				if (!String.IsNullOrEmpty(AvatarUrl))
					sb.AppendFormat("{0}", AvatarUrl);

				sb.Append("|");

				if (!String.IsNullOrEmpty(ChannelName))
					sb.AppendFormat("{0}", ChannelName);
			}

			return sb.ToString();
		}

		public static bool IsValidEmail(string email)
		{
			string _EmailRegexPattern = @"(['\""]{1,}.+['\""]{1,}\s+)?<?[\w\.\-]+@([\w\-]+\.)+[A-Za-z]{2,}>?";

			return Regex.IsMatch(email, _EmailRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		public static bool IsValidEmail(SourceAddress address)
		{
			return IsValidEmail(address.Address);
		}

		public SourceAddressCollection ToList()
		{
			return new SourceAddressCollection { this };
		}

		public static SourceAddress Empty
		{
			get { return new SourceAddress("noreply@inbox2.com", "Inbox2 dummy user"); }
		}
	}
}
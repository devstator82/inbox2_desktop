using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using Inbox2.Platform.Channels.Text;

namespace Inbox2.Platform.Channels.Entities
{
	[Serializable]
	[DataContract]
	public class SourceAddressCollection : IList<SourceAddress>
	{
		protected List<SourceAddress> inner = new List<SourceAddress>();
		
		public Action ListChanged { get; set; }

		/// <summary>
		/// Helper for serializing with WCF protocol buffers.
		/// </summary>
		[DataMember(Order = 1)]
		public string SerializedAddressCollection
		{
			get { return ToString(); }
			set { ParseStringInternal(value); }
		}

		public int Count
		{
			get { return inner.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public SourceAddress this[int index]
		{
			get { return inner[index]; }
			set { inner[index] = value; }
		}

		public SourceAddressCollection()
		{
		}

		public SourceAddressCollection(string addressString)
		{
			ParseStringInternal(addressString);
		}

		public SourceAddressCollection(IEnumerable<SourceAddress> collection)
		{
			foreach (var address in collection)
			{
				Add(address);
			}
		}

		public SourceAddressCollection(SourceAddress address)
		{
			Add(address);
		}

		public IEnumerator<SourceAddress> GetEnumerator()
		{
			return inner.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(SourceAddress item)
		{
			inner.Add(item);

			OnListChanged();
		}

		public SourceAddressCollection AddRange(IEnumerable<SourceAddress> source)
		{
			foreach (var address in source)
			{
				inner.Add(address);
			}

			OnListChanged();

			return this;
		}

		public void Clear()
		{
			inner.Clear();

			OnListChanged();
		}

		public bool Contains(SourceAddress item)
		{
			return inner.Contains(item, new SourceAddressComparer());
		}

		public void CopyTo(SourceAddress[] array, int arrayIndex)
		{
			inner.CopyTo(array, arrayIndex);
		}

		public bool Remove(SourceAddress item)
		{
			bool val = inner.Remove(item);

			OnListChanged();

			return val;
		}

		public int IndexOf(SourceAddress item)
		{
			return inner.IndexOf(item);
		}

		public void Insert(int index, SourceAddress item)
		{
			inner.Insert(index, item);

			OnListChanged();
		}

		public void RemoveAt(int index)
		{
			inner.RemoveAt(index);

			OnListChanged();
		}		

		void OnListChanged()
		{
			if (ListChanged != null)
			{
				ListChanged();
			}
		}

		public SourceAddressCollection GetInvalidEmailAddresses()
		{
			SourceAddressCollection collection = new SourceAddressCollection();

			foreach (var address in this)
			{
				if (!SourceAddress.IsValidEmail(address))
					collection.Add(address);
			}

			return collection;
		}

		void ParseStringInternal(string addressString)
		{
			if (inner == null)
				inner = new List<SourceAddress>();

			if (String.IsNullOrEmpty(addressString))
				return;

			string[] parts = addressString.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length > 0)
			{
				foreach (var part in parts)
					if (!String.IsNullOrEmpty(part.Trim()))
						Add(new SourceAddress(part.Trim()));
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Count; i++)
			{
				var address = this[i];

				if (address == null)
					continue;

				sb.Append(address.ToString());

				if (i < Count - 1)
					sb.Append("; ");
			}

			return sb.ToString();
		}

		public string ToEncodedString()
		{
			return HttpUtility.HtmlEncode(ToString());
		}

		public string ToSummarizedString()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Count; i++)
			{
				var address = this[i];

				if (address == null)
					continue;

				if (String.IsNullOrEmpty(address.DisplayName))
				{					
					if (SourceAddress.IsValidEmail(address.Address))
					{
						// No display-name
						string[] parts = address.Address.Split('@');
						sb.Append(HttpUtility.HtmlEncode(parts[0]));
					}
					else
					{
						// Fallback, just add the address
						sb.Append(HttpUtility.HtmlEncode(address.ToString()));
					}
				}
				else
				{					
					if (SourceAddress.IsValidEmail(address.DisplayName))
					{
						// Display-name is an email address (we are being tricked)
						string[] parts = address.DisplayName.Split('@');
						sb.Append(HttpUtility.HtmlEncode(parts[0]));
					}
					else
					{
						// Have display-name, try parsing into first/lastname
						var name = PersonName.Parse(address.DisplayName);

						sb.Append(HttpUtility.HtmlEncode(name.Firstname));
					}					
				}

				if (i == 0 && Count > 3)
				{
					sb.Append(" ... ");

					// Jump to beforelast item
					i = Count - 3;
				}

				else if (i < Count - 1)
					sb.Append(", ");
			}

			if (Count > 3)			
				sb.AppendFormat(" <em>and {0} other{1}</em>", Count - 3, Count == 1 ? String.Empty : "s");

			return sb.ToString();	
		}
	}
}

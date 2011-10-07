using System;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.VirtualMailBox.Entities
{
	[PersistableClass]
	[Serializable]
	public class Label : IComparable, IComparable<Label>, IEquatable<Label>
	{
		public string Labelname { get; set; }

		public LabelType LabelType { get; set; }

		public string MessageNumber { get; set; }

		public AdvancedObservableCollection<Message> Messages { get; set; }

		public AdvancedObservableCollection<Document> Documents { get; set; }

		private Label()
		{
			Messages = new AdvancedObservableCollection<Message>();
			Documents = new AdvancedObservableCollection<Document>();
		}

		public static bool IsSystemLabel(string labelname)
		{
			return labelname.ToLower() == Strings.Todo.ToLower()
				|| labelname.ToLower() == Strings.WaitingFor.ToLower()
				|| labelname.ToLower() == Strings.Someday.ToLower();
		}

		public Label(string labelname) : this()
		{
			// Parse labelname
			string[] parts = labelname.Split(new[] { ':' }, 3);

			try
			{
				if (parts.Length == 3)
				{
					MessageNumber = parts[0];
					LabelType = (LabelType)Enum.Parse(typeof(LabelType), parts[1]);
					Labelname = parts[2];
				}
				else if (parts.Length == 2)
				{
					LabelType = (LabelType) Enum.Parse(typeof (LabelType), parts[0]);
					Labelname = parts[1];
				}
				else
				{
					ParseFallback(labelname);
				}
			}
			catch
			{
				ParseFallback(labelname);
			}
		}

		public Label(LabelType labelType) : this()
		{
			Labelname = GetLabelName(labelType);
			LabelType = labelType;			
		}

		public Label(string labelname, LabelType labelType, string messageNumber)  : this()
		{
			if (String.IsNullOrEmpty(labelname) || labelname.Trim().Length == 0)
				throw new ArgumentNullException("labelname");

			Labelname = labelname;
			LabelType = labelType;
			MessageNumber = messageNumber;
		}

		void ParseFallback(string label)
		{
			Labelname = label;

			if (label == Strings.Todo)
				LabelType = LabelType.Todo;
			else if (label == Strings.Someday)
				LabelType = LabelType.Someday;
			else if (label == Strings.WaitingFor)
				LabelType = LabelType.WaitingFor;
			else
				LabelType = LabelType.Custom;			
		}

		string GetLabelName(LabelType labelType)
		{
			switch (labelType)
			{
				case LabelType.Todo:
					return Strings.Todo;
				case LabelType.Someday:
					return Strings.Someday;
				case LabelType.WaitingFor:
					return Strings.WaitingFor;
				default:
					throw new NotImplementedException();
			}
		}

		public int CompareTo(object obj)
		{
			return CompareTo((Label)obj);
		}

		public int CompareTo(Label other)
		{
			return Math.Min(
				Labelname.CompareTo(other.Labelname),
				LabelType.CompareTo(other.LabelType));
		}

		public bool Equals(Label other)
		{
			return LabelType == other.LabelType &&
				   Labelname == other.Labelname;
		}

		public override string ToString()
		{
			return String.Format("{0}:{1}:{2}", MessageNumber, LabelType, Labelname);
		}
	}
}
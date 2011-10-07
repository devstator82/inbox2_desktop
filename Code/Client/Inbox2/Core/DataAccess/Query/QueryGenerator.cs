using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Inbox2.Core.DataAccess.Reflection;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Platform.Channels.Extensions;

namespace Inbox2.Core.DataAccess.Query
{
	public class QueryGenerator<T>
	{
		private TableMap map;

		internal TableMap Map
		{
			get { return map; }
		}

		public SQLiteCommand GetSelectCommand()
		{
			map = Reflector.BuildMapFrom<T>();

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("select * from {0}", map.TableName);

			return new SQLiteCommand(sb.ToString());
		}

		public SQLiteCommand GetSelectCommand(string key)
		{
			map = Reflector.BuildMapFrom<T>();

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("select * from {0} where [{1}]=@{1}", map.TableName, map.PrimaryKey.ColumName);

			return new SQLiteCommand(sb.ToString());
		}

		public SQLiteCommand GetSelectCommand(List<PropertyValue> properties)
		{
			map = Reflector.BuildMapFrom<T>();

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("select * from {0} ", map.TableName);
			sb.AppendFormat(" where ");

			for (int i = 0; i < properties.Count; i++)
			{
				var property = properties[i];

				if (i > 0)
					sb.Append(" and ");

				sb.AppendFormat("[{0}]=@{0}", property.Name);
			}

			var command = new SQLiteCommand(sb.ToString());

			// Add values to command
			foreach (var property in properties)
				command.Parameters.AddWithValue("@" + property.Name, property.Value);

			return command;
		}

		public SQLiteCommand GetInsertCommand(T instance)
		{
			map = Reflector.BuildMapFromInstance(instance);

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("insert into [{0}] ", map.TableName);
			sb.Append("(");

			// Build columns part
			AppendColumnsForInsert(sb);

			sb.Append(") values (");

			// Build values part
			AppendValuesForInsert(sb);

			sb.Append("); SELECT last_insert_rowid()");

			SQLiteCommand command = BuildCommand(sb);

			// Copy parameters to command
			CopyColumnValuesToCommand(command);

			return command;
		}		

		public SQLiteCommand GetUpdateCommand(T instance, params string[] columns)
		{
			map = Reflector.BuildMapFromInstance(instance);

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("update [{0}] set ", map.TableName);

			// Build columns/values part
			AppendColumnsForUpdate(sb, columns);

			sb.AppendFormat(" where [{0}]=@{0}", map.PrimaryKey.ColumName);

			SQLiteCommand command = BuildCommand(sb);

			// Copy parameters to command
			CopyColumnValuesToCommand(command);

			command.Parameters.AddWithValue("@" + map.PrimaryKey.ColumName, map.PrimaryKey.Value);

			return command;
		}

		public SQLiteCommand GetDeleteCommand(T instance)
		{
			map = Reflector.BuildMapFromInstance(instance);

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("delete from [{0}] ", map.TableName);
			sb.AppendFormat(" where [{0}]=@{0}", map.PrimaryKey.ColumName);

			SQLiteCommand command = BuildCommand(sb);

			// Copy parameters to command
			CopyColumnValuesToCommand(command);

			command.Parameters.AddWithValue("@" + map.PrimaryKey.ColumName, map.PrimaryKey.Value);

			return command;
		}

		public SQLiteCommand GetDeleteCommand(List<PropertyValue> properties)
		{
			map = Reflector.BuildMapFrom<T>();

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("delete from {0} ", map.TableName);
			sb.AppendFormat(" where ");

			for (int i = 0; i < properties.Count; i++)
			{
				var property = properties[i];

				if (i > 0)
					sb.Append(" and ");

				sb.AppendFormat("[{0}]=@{0}", property.Name);
			}

			var command = new SQLiteCommand(sb.ToString());

			// Add values to command
			foreach (var property in properties)
				command.Parameters.AddWithValue("@" + property.Name, property.Value);

			return command;
		}

		void AppendColumnsForInsert(StringBuilder sb)
		{
			for (int i = 0; i < map.Columns.Count; i++)
			{
				PropertyToken token = map.Columns[i];

				sb.AppendFormat("[{0}]", token.ColumName);

				if (i < map.Columns.Count - 1)
					sb.Append(", ");
			}
		}

		void AppendColumnsForUpdate(StringBuilder sb, params string[] columns)
		{
			if (columns.Length > 0)
			{
				for (int i = 0; i < columns.Length; i++)
				{
					sb.AppendFormat("[{0}]=@{0}", columns[i]);

					if (i < columns.Length - 1)
						sb.Append(", ");
				}
			}
			else
			{
				for (int i = 0; i < map.Columns.Count; i++)
				{
					PropertyToken token = map.Columns[i];

					sb.AppendFormat("[{0}]=@{0}", token.ColumName);

					if (i < map.Columns.Count - 1)
						sb.Append(", ");
				}	
			}			
		}

		void AppendValuesForInsert(StringBuilder sb)
		{
			for (int i = 0; i < map.Columns.Count; i++)
			{
				PropertyToken token = map.Columns[i];

				sb.AppendFormat("@{0}", token.ColumName);

				if (i < map.Columns.Count - 1)
					sb.Append(", ");
			}
		}

		void CopyColumnValuesToCommand(SQLiteCommand command)
		{
			using (new JoeCulture())
			{
				foreach (var token in map.Columns)
				{
					TypeConverter conv = TypeDescriptor.GetConverter(token.SourceType);


					// The default DateTime converter does not save the seonds, this causes
					// problems with the threading feature when two messages are sent/received
					// in the same minute (which is quite likely to happen).
					string valueStr = token.Value is DateTime
					                  	? ((DateTime) token.Value).ToString()
					                  	: token.Value == null ? String.Empty : conv.ConvertToString(token.Value);

					command.Parameters.AddWithValue("@" + token.ColumName, valueStr);
				}
			}
		}

		SQLiteCommand BuildCommand(StringBuilder sb)
		{
			SQLiteCommand command = new SQLiteCommand(sb.ToString());

			foreach (var token in map.Columns)
				command.Parameters.AddWithValue(token.ColumName, token.Value);

			return command;
		}		
	}
}
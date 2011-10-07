using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Core.DataAccess.Reflection;
using Inbox2.Framework;

namespace Inbox2.Core.DataAccess
{
	public static class Migrate
	{
		public static void Up(Type targetType)
		{
			var map = Reflector.BuildMapFromType(targetType);

			if (TableExists(map.TableName))
				return;

			StringBuilder sql = new StringBuilder();
			sql.AppendFormat("CREATE TABLE \"{0}\"{1}", map.TableName, Environment.NewLine);
			sql.AppendLine("(");
			
			sql.AppendFormat("\t\"{0}\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1}", 
				map.PrimaryKey.ColumName, Environment.NewLine);

			for (int i = 0; i < map.Columns.Count; i++)
			{
				var token = map.Columns[i];

				string dbType = "TEXT";

				if (token.SourceType == typeof (Int16)
				    || token.SourceType == typeof (Int32)
				    || token.SourceType == typeof (Int64))
				{
					dbType = "INTEGER";
				}

				sql.AppendFormat("\t\"{0}\" {1}{2}{3}", 
					token.ColumName, 
					dbType, 
					i < map.Columns.Count -1 ? "," : "", // Skip comma for last column
					Environment.NewLine);
			}

			// Remove last comma
			sql.AppendLine(")");

			// Execute creation script for table
			ClientState.Current.DataService.ExecuteNonQuery(sql.ToString());
		}

		static bool TableExists(string tableName)
		{
			string sql = String.Format("select count(*) from sqlite_master where name='{0}'", tableName);

			return ClientState.Current.DataService.ExecuteScalar<int>(sql) > 0;
		}
	}
}

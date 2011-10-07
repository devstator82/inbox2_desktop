using System.Data.SQLite;
using System.IO;
using Inbox2.Core.Configuration;

namespace Inbox2.Core.DataAccess
{
	public static class DatabaseUtil
	{
		public static void InitializeDataStore()
		{
			// Touch the database to create the file
			if (File.Exists(DebugKeys.DefaultDataDirectory + "\\Inbox2.db3") == false)
			{
				using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + DebugKeys.DefaultDataDirectory + "\\Inbox2.db3"))
				{
					connection.Open();
				}
			}
		}		
	}
}

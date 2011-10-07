using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Inbox2.Core.Configuration;
using Inbox2.Core.DataAccess.Query;
using Inbox2.Core.DataAccess.Reflection;
using Inbox2.Framework;
using Inbox2.Framework.Persistance;
using Inbox2.Platform.Channels.Extensions;
using ObjectExtensions=Inbox2.Platform.Framework.Extensions.ObjectExtensions;

namespace Inbox2.Core.DataAccess
{
	[Export(typeof(IDataService))]
	public class SQLiteDataService : IDataService
	{		
		public T SelectByKey<T>(long key) where T : new()
		{
			QueryGenerator<T> generator = new QueryGenerator<T>();

			using (SQLiteConnection connection = CreateConnection())
			{
				SQLiteCommand command = generator.GetSelectCommand(key.ToString());

				command.Parameters.AddWithValue("@" + generator.Map.PrimaryKey.ColumName, key);
				command.Connection = connection;

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())			
							ObjectExtensions.CreateFrom(instance, reader);

						return instance;
					}
				}
			}

			return default(T);
		}

		public T SelectBy<T>(object queryObject) where T : new()
		{
			QueryGenerator<T> generator = new QueryGenerator<T>();

			using (SQLiteConnection connection = CreateConnection())
			{
				var properties = queryObject.GetProperties().ToList();

				SQLiteCommand command = generator.GetSelectCommand(properties);

				command.Connection = connection;

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())
							ObjectExtensions.CreateFrom(instance, reader);

						return instance;
					}
				}
			}

			return default(T);
		}
		
		public T SelectBy<T>(string query) where T : new()
		{
			var command = CreateCommand();

			command.CommandText = query;

			using (SQLiteConnection connection = CreateConnection())
			{
				command.Connection = connection;

				using (SQLiteDataReader reader = (SQLiteDataReader)command.ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())
							ObjectExtensions.CreateFrom(instance, reader);

						return instance;
					}
				}
			}

			return default(T);
		}

		public T SelectBy<T>(IDbCommand command) where T : new()
		{
			using (SQLiteConnection connection = CreateConnection())
			{
				command.Connection = connection;

				using (SQLiteDataReader reader = (SQLiteDataReader)command.ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())
							ObjectExtensions.CreateFrom(instance, reader);

						return instance;
					}
				}
			}

			return default(T);
		}

		public IEnumerable<T> SelectAllBy<T>(object queryObject) where T : new()
		{
			QueryGenerator<T> generator = new QueryGenerator<T>();

			using (SQLiteConnection connection = CreateConnection())
			{
				var properties = queryObject.GetProperties().ToList();

				SQLiteCommand command = generator.GetSelectCommand(properties);

				command.Connection = connection;

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())
							ObjectExtensions.CreateFrom(instance, reader);

						yield return instance;
					}
				}
			}
		}

		public IEnumerable<T> SelectAllBy<T>(IDbCommand command) where T : new()
		{
			using (SQLiteConnection connection = CreateConnection())
			{
				command.Connection = connection;

				using (SQLiteDataReader reader = (SQLiteDataReader)command.ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())
							ObjectExtensions.CreateFrom(instance, reader);

						yield return instance;
					}
				}
			}
		}

		/// <summary>
		/// Selects all items from the table associated with T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IEnumerable<T> SelectAll<T>() where T : new()
		{
			QueryGenerator<T> generator = new QueryGenerator<T>();

			using (SQLiteConnection connection = CreateConnection())
			{
				SQLiteCommand command = generator.GetSelectCommand();

				command.Connection = connection;

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())
							ObjectExtensions.CreateFrom(instance, reader);

						yield return instance;
					}
				}
			}
		}

		public IEnumerable<T> SelectAll<T>(IDbCommand command) where T : new()
		{
			using (SQLiteConnection connection = CreateConnection())
			{				
				command.Connection = connection;

				using (SQLiteDataReader reader = ((SQLiteCommand) command).ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())
							ObjectExtensions.CreateFrom(instance, reader);

						yield return instance;
					}
				}
			}
		}

		public IEnumerable<T> SelectAll<T>(string query) where T : new()
		{
			using (SQLiteConnection connection = CreateConnection())
			{
				SQLiteCommand command = new SQLiteCommand(query);

				command.Connection = connection;

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						T instance = new T();

						using (new JoeCulture())
							ObjectExtensions.CreateFrom(instance, reader);

						yield return instance;
					}
				}
			}
		}

		/// <summary>
		/// Saves the specified instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		public void Save<T>(T instance)
		{
			if (instance is IDataServiceHooks)
				(instance as IDataServiceHooks).BeforeSave();

			QueryGenerator<T> generator = new QueryGenerator<T>();

			using (SQLiteConnection connection = CreateConnection())
			{
				SQLiteCommand command = generator.GetInsertCommand(instance);

				command.Connection = connection;

				object id = command.ExecuteScalar();

				// Set the new id on the primary key
				Reflector.SetPropertyValue(instance, generator.Map.PrimaryKey, id);
			}

			if (instance is IDataServiceHooks)
				(instance as IDataServiceHooks).AfterSave();
		}

		/// <summary>
		/// Updates the specified instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		public void Update<T>(T instance)
		{
			if (instance is IDataServiceHooks)
				(instance as IDataServiceHooks).BeforeUpdate();

			QueryGenerator<T> generator = new QueryGenerator<T>();

			SQLiteCommand command = generator.GetUpdateCommand(instance);

			ExecuteNonQuery(command);

			if (instance is IDataServiceHooks)
				(instance as IDataServiceHooks).AfterUpdate();
		}

		/// <summary>
		/// Deletes the specified instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		public void Delete<T>(T instance)
		{
			if (instance is IDataServiceHooks)
				(instance as IDataServiceHooks).BeforeDelete();

			QueryGenerator<T> generator = new QueryGenerator<T>();

			SQLiteCommand command = generator.GetDeleteCommand(instance);

			ExecuteNonQuery(command);

			if (instance is IDataServiceHooks)
				(instance as IDataServiceHooks).AfterDelete();
		}

		/// <summary>
		/// Deletes all the rows of the specified type with the specified where clause.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryObject">The query object.</param>
		public void Delete<T>(object queryObject)
		{			
			QueryGenerator<T> generator = new QueryGenerator<T>();

			var properties = queryObject.GetProperties().ToList();

			SQLiteCommand command = generator.GetDeleteCommand(properties);

			ExecuteNonQuery(command);			
		}

		/// <summary>
		/// Executes the given command and returns the scalar value casted to type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		public T ExecuteScalar<T>(IDbCommand command) where T : new()
		{
			using (SQLiteConnection connection = CreateConnection())
			{
				command.Connection = connection;

				var value = command.ExecuteScalar();

				TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));

				return (T)conv.ConvertFromString(value.ToString());
			}
		}

		/// <summary>
		/// Executes the given query and returns the scalar value casted to type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		public T ExecuteScalar<T>(string query) where T : new()
		{
			return ExecuteScalar<T>(new SQLiteCommand(query));
		}

		/// <summary>
		/// Crreates the command.
		/// </summary>
		/// <returns></returns>
		public IDbCommand CreateCommand()
		{
			return new SQLiteCommand();
		}

		/// <summary>
		/// Creates the parameter.
		/// </summary>
		/// <returns></returns>
		public IDataParameter CreateParameter()
		{
			return new SQLiteParameter();
		}

		/// <summary>
		/// Creates the parameter with the given name and value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public IDataParameter CreateParameter(string name, object value)
		{
			return new SQLiteParameter { ParameterName = name, Value = value };
		}

		/// <summary>
		/// Executes the query and returns a IDataReader instance.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		public IDataReader ExecuteReader(IDbCommand command)
		{
			SQLiteConnection connection = CreateConnection();
			
			command.Connection = connection;

			return command.ExecuteReader(CommandBehavior.CloseConnection);			
		}

		/// <summary>
		/// Executes the query and returns a IDataReader instance.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public IDataReader ExecuteReader(string query)
		{
			var command = CreateCommand();

			command.CommandText = query;

			return ExecuteReader(command);
		}

		/// <summary>
		/// Executes the given command.
		/// </summary>
		/// <param name="command">The command.</param>
		public void ExecuteNonQuery(IDbCommand command)
		{
			using (SQLiteConnection connection = CreateConnection())
			{
				command.Connection = connection;

				command.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Executes the given query.
		/// </summary>
		/// <param name="query">The query.</param>
		public void ExecuteNonQuery(string query)
		{
			ExecuteNonQuery(new SQLiteCommand(query));
		}

		/// <summary>
		/// Clones the existing connection on the UI thread.
		/// </summary>
		/// <returns></returns>
		SQLiteConnection CreateConnection()
		{
			var connection = new SQLiteConnection("Data Source=" + DebugKeys.DefaultDataDirectory + "\\Inbox2.db3");

			connection.Open();

			return connection;
		}		
	}
}

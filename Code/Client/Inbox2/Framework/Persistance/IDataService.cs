using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Inbox2.Framework.Persistance
{
	public interface IDataService
	{
		T SelectByKey<T>(long key) where T : new();

		T SelectBy<T>(object queryObject) where T : new();

		T SelectBy<T>(string query) where T : new();

		IEnumerable<T> SelectAllBy<T>(object queryObject) where T : new();

		IEnumerable<T> SelectAll<T>() where T : new();

		IEnumerable<T> SelectAll<T>(IDbCommand command) where T : new();

		IEnumerable<T> SelectAll<T>(string query) where T : new();

		void Save<T>(T instance);

		void Update<T>(T instance);

		void Delete<T>(T instance);

		void Delete<T>(object queryObject);

		T ExecuteScalar<T>(IDbCommand command) where T : new();

		T ExecuteScalar<T>(string query) where T : new();

		IDbCommand CreateCommand();

		IDataParameter CreateParameter();

		IDataParameter CreateParameter(string name, object value);

		IDataReader ExecuteReader(IDbCommand command);

		IDataReader ExecuteReader(string query);

		void ExecuteNonQuery(IDbCommand command);

		void ExecuteNonQuery(string query);		
	}
}

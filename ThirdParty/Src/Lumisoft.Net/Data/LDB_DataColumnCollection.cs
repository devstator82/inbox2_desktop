using System;
using System.Collections.Generic;

namespace LumiSoft.Data.lsDB
{
	/// <summary>
	/// lsDB data column collection.
	/// </summary>
	public class LDB_DataColumnCollection //: IEnumerable<LDB_DataColumn>
	{
		private object               m_pOwner   = null;
		private List<LDB_DataColumn> m_pColumns = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="owner">Table that owns this collection.</param>
		internal LDB_DataColumnCollection(object owner)
		{
			m_pOwner = owner;

			m_pColumns = new List<LDB_DataColumn>();
		}


		#region method Add

		/// <summary>
		/// Ads specified data column to collection.
		/// </summary>
		/// <param name="column"></param>
		public void Add(LDB_DataColumn column)
		{
			if(Contains(column.ColumnName)){
				throw new Exception("Data column with specified name '" + column.ColumnName + "' already exists !");
			}

            if(m_pOwner.GetType() == typeof(DbFile)){
			    ((DbFile)m_pOwner).AddColumn(column);
            }
            else if(m_pOwner.GetType() == typeof(lsDB_FixedLengthTable)){
                 ((lsDB_FixedLengthTable)m_pOwner).AddColumn(column);
            }

			m_pColumns.Add(column);
		}

		#endregion


		#region method Remove

		/// <summary>
		/// Removes specified data column from collection.
		/// </summary>
		/// <param name="columName">Column name which to remove.</param>
		public void Remove(string columName)
		{
			foreach(LDB_DataColumn column in m_pColumns){
				if(column.ColumnName.ToLower() == columName.ToLower()){
					Remove(column);
					break;
				}
			}
		}

		/// <summary>
		/// Removes specified data column from collection.
		/// </summary>
		/// <param name="column">Data column which to remove.</param>
		public void Remove(LDB_DataColumn column)
		{
			m_pColumns.Remove(column);
		}

		#endregion


		#region method IndexOf

		/// <summary>
		///  Gets specified data column index in collection. Returns -1 if no such column.
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public int IndexOf(string columnName)
		{
			for(int i=0;i<m_pColumns.Count;i++){
				if(((LDB_DataColumn)m_pColumns[i]).ColumnName.ToLower() == columnName.ToLower()){
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Gets specified data column index in collection. Returns -1 if no such column.
		/// </summary>
		/// <param name="column">Data column.</param>
		/// <returns></returns>
		public int IndexOf(LDB_DataColumn column)
		{
			return m_pColumns.IndexOf(column);
		}

		#endregion


		#region method Contains

		/// <summary>
		/// Gets if data column collection contains specified column.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <returns></returns>
		public bool Contains(string columnName)
		{
			foreach(LDB_DataColumn column in m_pColumns){
				if(column.ColumnName.ToLower() == columnName.ToLower()){
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets if data column collection contains specified column.
		/// </summary>
		/// <param name="column">Data column.</param>
		/// <returns></returns>
		public bool Contains(LDB_DataColumn column)
		{
			return m_pColumns.Contains(column);
		}

		#endregion


		#region method Parse

		/// <summary>
		/// Parses and adds data column to the collection.
		/// </summary>
		/// <param name="columnData"></param>
		internal void Parse(byte[] columnData)
		{
			LDB_DataColumn column = new LDB_DataColumn();
			column.Parse(columnData);

			m_pColumns.Add(column);
		}

		#endregion


		#region interface IEnumerator

		/// <summary>
		/// Gets enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<LDB_DataColumn> GetEnumerator()
		{
			return m_pColumns.GetEnumerator();
		}

		#endregion

		#region Properties Implementation

		/// <summary>
		/// Gets column from specified index.
		/// </summary>
		public LDB_DataColumn this[int index]
		{
			get{ return m_pColumns[index]; }
		}

		/// <summary>
		/// Gets column count in the collection.
		/// </summary>
		public int Count
		{
			get{ return m_pColumns.Count; }
		}

		#endregion

	}
}

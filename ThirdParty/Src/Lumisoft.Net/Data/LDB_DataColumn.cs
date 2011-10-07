using System;

namespace LumiSoft.Data.lsDB
{
	/// <summary>
	/// 
	/// </summary>
	public class LDB_DataColumn
	{
		private LDB_DataType m_DataType   = LDB_DataType.String;
		private string       m_ColumnName = "";
		private int          m_ColumSize  = -1;

        /// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="dataType">Column data type.</param>
        public LDB_DataColumn(string columnName,LDB_DataType dataType) : this(columnName,dataType,-1)
        {
        }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="dataType">Column data type.</param>  
        /// <param name="columnSize">Specifies column data size. This is available for String datatype only.</param>  
		public LDB_DataColumn(string columnName,LDB_DataType dataType,int columnSize)
		{
			m_ColumnName = columnName;
			m_DataType = dataType;

			// TODO: check that column name won't exceed 50 bytes

            if(dataType == LDB_DataType.Bool){
                m_ColumSize = 1;
            }
            else if(dataType == LDB_DataType.DateTime){
                m_ColumSize = 13;
            }
            else if(dataType == LDB_DataType.Int){
                m_ColumSize = 4;
            }
            else if(dataType == LDB_DataType.Long){
                m_ColumSize = 8;
            }
            else{
                m_ColumSize = columnSize;
            }
		}

        /// <summary>
        /// Internal constructor.
        /// </summary>
		internal LDB_DataColumn()
		{
		}


		#region method Parse

		/// <summary>
		/// Parses column from byte[] data.
		/// </summary>
		/// <param name="columnData">Column data.</param>
		internal void Parse(byte[] columnData)
		{
			if(columnData.Length != 102){
				throw new Exception("Invalid column data length '" + columnData.Length.ToString() + "' !");
			}

			//-- total 102 bytes
			// 1 byte   - column type
			// 4 bytes  - column size
			// 45 bytes - reserved
			// 50 bytes - column name
            // 2  bytes - CRLF

			// column type
			m_DataType = (LDB_DataType)columnData[0];
			// column size
			m_ColumSize = (columnData[1] << 24) | (columnData[2] << 16) | (columnData[3] << 8) | (columnData[4] << 0);
			// reserved
			// 45 bytes
			// column name
			byte[] columnName = new byte[50];
			Array.Copy(columnData,50,columnName,0,columnName.Length);
			m_ColumnName = GetChar0TerminatedString(System.Text.Encoding.UTF8.GetString(columnName));

            if(m_DataType == LDB_DataType.Bool){
                m_ColumSize = 1;
            }
            else if(m_DataType == LDB_DataType.DateTime){
                m_ColumSize = 13;
            }
            else if(m_DataType == LDB_DataType.Int){
                m_ColumSize = 4;
            }
            else if(m_DataType == LDB_DataType.Long){
                m_ColumSize = 8;
            }
		}

		#endregion

		#region method ToColumnInfo

		/// <summary>
		/// Convert column to byte[] data.
		/// </summary>
		/// <returns></returns>
		internal byte[] ToColumnInfo()
		{
			//-- total 100 + CRLF bytes
			// 1 byte   - column type
			// 4 bytes  - column size
			// 45 bytes - reserved
			// 50 bytes - column name
			// CRLF

			byte[] columnData = new byte[102];
			// column type
			columnData[0] = (byte)m_DataType;
			// column size
			columnData[1] = (byte)((m_ColumSize & (1 << 24)) >> 24);
			columnData[2] = (byte)((m_ColumSize & (1 << 16)) >> 16);
			columnData[3] = (byte)((m_ColumSize & (1 << 8)) >> 8);
			columnData[4] = (byte)((m_ColumSize & (1 << 0)) >> 0);
			// reserved
			// 45 bytes
			// column name
			byte[] columnName = System.Text.Encoding.UTF8.GetBytes(m_ColumnName);
			Array.Copy(columnName,0,columnData,50,columnName.Length);
			// CRLF
			columnData[100] = (byte)'\r';
			columnData[101] = (byte)'\n';

			return columnData;
		}

		#endregion


		#region method GetChar0TerminatedString

		/// <summary>
		/// Gets string from char(0) terminated text.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <returns></returns>
		internal static string GetChar0TerminatedString(string text)
		{
			if(text.IndexOf('\0') > -1){
				return text.Substring(0,text.IndexOf('\0'));
			}
			else{
				return text;
			}
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets LDB data type.
		/// </summary>
		public LDB_DataType DataType
		{
			get{ return m_DataType; }
		}

		/// <summary>
		/// Gets column name.
		/// </summary>
		public string ColumnName
		{
			get{ return m_ColumnName; }
		}

        /// <summary>
        /// Gets column size. Returns -1 if column is with variable length.
        /// </summary>
        public int ColumnSize
        {
            get{ return m_ColumSize; }
        }

		#endregion

	}
}

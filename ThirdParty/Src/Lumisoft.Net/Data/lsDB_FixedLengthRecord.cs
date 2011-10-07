using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Data.lsDB
{
    /// <summary>
    /// lsDB_FixedLengthTable table record.
    /// </summary>
    public class lsDB_FixedLengthRecord
    {
        private lsDB_FixedLengthTable m_pOwnerDb               = null;
        private long                  m_Pointer                = -1;
        private byte[]                m_RowData                = null;
        private int[]                 m_ColumnDataSizes        = null;
        private LDB_DataType[]        m_ColumnDataTypes        = null;
        private int[]                 m_ColumnDataStartOffsets = null;
        private int                   m_ColumnCount            = -1;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="ownerDb">Table that owns this row.</param>
        /// <param name="pointer">Row start offset in data base file.</param>
        /// <param name="rowData">Row data.</param>
        internal lsDB_FixedLengthRecord(lsDB_FixedLengthTable ownerDb,long pointer,byte[] rowData)
        {
            m_pOwnerDb = ownerDb;
            m_Pointer  = pointer;
            m_RowData  = rowData;

            m_ColumnCount = ownerDb.Columns.Count;
                 
            m_ColumnDataTypes = new LDB_DataType[m_ColumnCount];
            m_ColumnDataSizes = new int[m_ColumnCount];
            for(int i = 0;i < m_ColumnDataSizes.Length;i++){
                m_ColumnDataTypes[i] = m_pOwnerDb.Columns[i].DataType;
                m_ColumnDataSizes[i] =  m_pOwnerDb.Columns[i].ColumnSize;
            }

            m_ColumnDataStartOffsets = new int[m_ColumnCount];
            int columnDataStartOffset = 1;
            for(int i = 0;i < m_ColumnDataStartOffsets.Length;i++){
                m_ColumnDataStartOffsets[i] = columnDataStartOffset;
                columnDataStartOffset += m_pOwnerDb.Columns[i].ColumnSize;
            }
        }


        #region method ReuseRecord

        /// <summary>
        /// Reuses lsDB_FixedLengthRecord object,
        /// </summary>
        /// <param name="ownerDb">Table that owns this row.</param>
        /// <param name="pointer">Row start offset in data base file.</param>
        /// <param name="rowData">Row data.</param>
        internal void ReuseRecord(lsDB_FixedLengthTable ownerDb,long pointer,byte[] rowData)
        {
            m_pOwnerDb = ownerDb;
            m_Pointer  = pointer;
            m_RowData  = rowData;
        }

        #endregion


        #region static method ConvertFromInternalData

		/// <summary>
		/// Converts internal data to .NET data type.
		/// </summary>
		/// <param name="dataType">Data type.</param>
		/// <param name="val">Data buffer.</param>
        /// <param name="offset">Offset in data buffer where to start reading data.</param>
        /// <param name="length">Lenght of data to read from data buffer.</param>
		/// <returns></returns>
		public static object ConvertFromInternalData(LDB_DataType dataType,byte[] val,int offset,int length)
        {
			if(dataType == LDB_DataType.Bool){
				return Convert.ToBoolean(val[offset + 0]);
			}
			else if(dataType == LDB_DataType.DateTime){
				/* Data structure
					1 byte day
					1 byte month
					4 byte year (int)
					1 byte hour
					1 byte minute
					1 byte second
				*/
				
				// day
				int day = val[offset + 0];
				// month
				int month = val[offset + 1];
				// year
				int year = ldb_Utils.ByteToInt(val,offset + 2);
				// hour
				int hour = val[offset + 6];
				// minute
				int minute = val[offset + 7];
				// second
				int second = val[offset + 8];

				return new DateTime(year,month,day,hour,minute,second);
			}				
			else if(dataType == LDB_DataType.Long){
				return ldb_Utils.ByteToLong(val,offset + 0);
			}								
			else if(dataType == LDB_DataType.Int){
				return ldb_Utils.ByteToInt(val,offset + 0);
			}
			else if(dataType == LDB_DataType.String){
				return System.Text.Encoding.UTF8.GetString(val,offset,length);
			}
			else{
				throw new Exception("Invalid column data type, never must reach here !");
			}
		}

		#endregion


        #region Properties Implementation

        /// <summary>
        /// Gets or sets specified data column value.
        /// </summary>
        /// <param name="columnIndex">Zero based column index.</param>
        /// <returns></returns>
        public object this[int columnIndex]
        {
            /* Fixed record structure:
                    1 byte     - specified is row is used or free space
                                 u - used
                                 f - free space
                    x bytes    - columns data
                    2 bytes    - CRLF
            */

            get{  
                if(columnIndex < 0){
					throw new Exception("The columnIndex can't be negative value !");
				}
				if(columnIndex > m_ColumnCount){
					throw new Exception("The columnIndex out of columns count !");
				}
                                
                return ConvertFromInternalData(m_ColumnDataTypes[columnIndex],m_RowData,m_ColumnDataStartOffsets[columnIndex],m_ColumnDataSizes[columnIndex]);        
            }

            set{
                if(columnIndex < 0){
					throw new Exception("The columnIndex can't be negative value !");
				}
				if(columnIndex > m_ColumnCount){
					throw new Exception("The columnIndex out of columns count !");
				}

                byte[] val = LDB_Record.ConvertToInternalData(m_pOwnerDb.Columns[columnIndex],value);
                // Check that value won't exceed maximum cloumn allowed size
                if(val.Length > m_ColumnDataSizes[columnIndex]){
                    throw new Exception("Value exceeds maximum allowed value for column '" + m_pOwnerDb.Columns[columnIndex].ColumnName + "' !");
                }

                // TODO: String, string must be char(0) terminated and padded to column length
                if(m_ColumnDataTypes[columnIndex] == LDB_DataType.String){
                    throw new Exception("TODO: String not implemented !");
                }

                // Update value in database file
                m_pOwnerDb.WriteToFile(m_Pointer + m_ColumnDataStartOffsets[columnIndex],val,0,val.Length);

                // Update value in buffer
                Array.Copy(val,0,m_RowData,m_ColumnDataStartOffsets[columnIndex],val.Length);
            }
        }


        /// <summary>
        /// Gets row pointer.
        /// </summary>
        internal long Pointer
        {
            get{ return m_Pointer; }
        }

        #endregion

    }
}

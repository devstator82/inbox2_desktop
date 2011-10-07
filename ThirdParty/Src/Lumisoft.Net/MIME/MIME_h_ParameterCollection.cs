using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.MIME
{
    /// <summary>
    /// Represents MIME header field parameters collection.
    /// </summary>
    public class MIME_h_ParameterCollection : IEnumerable
    {
        private bool                                m_IsModified  = false;
        private MIME_h                              m_pOwner      = null;
        private Dictionary<string,MIME_h_Parameter> m_pParameters = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="owner">Owner MIME header field.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>owner</b> is null reference.</exception>
        public MIME_h_ParameterCollection(MIME_h owner)
        {
            if(owner == null){
                throw new ArgumentNullException("owner");
            }

            m_pOwner = owner;

            m_pParameters = new Dictionary<string,MIME_h_Parameter>(StringComparer.CurrentCultureIgnoreCase);
        }


        #region method Remove

        /// <summary>
        /// Removes specified parametr from the collection.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>name</b> is null reference.</exception>
        public void Remove(string name)
        {
            if(name == null){
                throw new ArgumentNullException("name");
            }

            if(m_pParameters.Remove(name)){
                m_IsModified = true;
            }
        }

        #endregion

        #region method Clear

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            m_pParameters.Clear();
            m_IsModified = true;
        }

        #endregion

        #region method ToArray

        /// <summary>
        /// Copies header fields parameters to new array.
        /// </summary>
        /// <returns>Returns header fields parameters array.</returns>
        public MIME_h_Parameter[] ToArray()
        {
            MIME_h_Parameter[] retVal = new MIME_h_Parameter[m_pParameters.Count];
            m_pParameters.Values.CopyTo(retVal,0);

            return retVal;
        }

        #endregion


        #region method ToString

        /// <summary>
        /// Returns header field parameters as string.
        /// </summary>
        /// <returns>Returns header field parameters as string.</returns>
        public override string ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Returns header field parameters as string.
        /// </summary>
        /// <param name="charset">Charset to use to encode 8-bit characters. Value null means parameters not encoded.</param>
        /// <returns>Returns header field parameters as string.</returns>
        public string ToString(Encoding charset)
        {
            /* RFC 2231.
             *      If parameter conatins 8-bit byte, we need to encode parameter value
             *      If parameter value length bigger than MIME maximum allowed line length,
             *      we need split value.
            */

            if(charset == null){
                charset = Encoding.Default;
            }

            StringBuilder retVal = new StringBuilder();
            foreach(MIME_h_Parameter parameter in this.ToArray()){
                if(string.IsNullOrEmpty(parameter.Value)){
                    retVal.Append("; " + parameter.Name);
                }
                // We don't need to encode or split value.
                else if((charset == null || Core.IsAscii(parameter.Value)) && parameter.Value.Length < 76){
                    retVal.Append("; " + parameter.Name + "=" + TextUtils.QuoteString(parameter.Value));
                }
                // We need to encode/split value.
                else{
                    byte[] byteValue = charset.GetBytes(parameter.Value);

                    List<string> values = new List<string>();            
                    // Do encoding/splitting.
                    int    offset    = 0;
                    char[] valueBuff = new char[50];
                    foreach(byte b in byteValue){                                        
                        // We need split value as RFC 2231 says.
                        if(offset >= (50 - 3)){
                            values.Add(new string(valueBuff,0,offset));
                            offset = 0;
                        }
                        
                        // Normal char, we don't need to encode.
                        if(MIME_Reader.IsAttributeChar((char)b)){
                            valueBuff[offset++] = (char)b;
                        }
                        // We need to encode byte as %X2.
                        else{
                            valueBuff[offset++] = '%';
                            valueBuff[offset++] = (b >> 4).ToString("X")[0];
                            valueBuff[offset++] = (b & 0xF).ToString("X")[0];
                        }
                    }
                    // Add pending buffer value.
                    if(offset > 0){
                        values.Add(new string(valueBuff,0,offset));
                    }

                    for(int i=0;i<values.Count;i++){
                        // Only fist value entry has charset and language info.
                        if(charset != null && i == 0){
                            retVal.Append("; " + parameter.Name + "*" + i.ToString() + "*=" + charset.WebName + "''" + values[i]);
                        }
                        else{
                            retVal.Append("; " + parameter.Name + "*" + i.ToString() + "*=" + values[i]);
                        }
                    }
                }
            }

            return retVal.ToString();
        }

        #endregion

        #region method Parse

        /// <summary>
        /// Parses parameters from the specified value.
        /// </summary>
        /// <param name="value">Header field parameters string.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>value</b> is null reference.</exception>
        public void Parse(string value)
        {
            if(value == null){
                throw new ArgumentNullException("value");
            }

            Parse(new MIME_Reader(value));
        }

        /// <summary>
        /// Parses parameters from the specified reader.
        /// </summary>
        /// <param name="reader">MIME reader.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>reader</b> is null reference.</exception>
        public void Parse(MIME_Reader reader)
        {
            if(reader == null){
                throw new ArgumentNullException("reader");
            }

            /* RFC 2231.
            */

            while(true){
                // End os stream reached.
                if(reader.Peek(true) == -1){
                    break;
                }
                // Next parameter start, just eat that char.
                else if(reader.Peek(true) == ';'){
                    reader.Char(false);
                }
                else{
                    string name = reader.Token();

                    string value = "";
                    // Parameter value specified.
                    if(reader.Peek(true) == '='){
                        reader.Char(false);

                        string v = reader.Word();
                        // Normally value may not be null, but following case: paramName=EOS.
                        if(v != null){
                            value = v;
                        }
                    }
                    
                    // RFC 2231 encoded/splitted parameter.
                    if(name.IndexOf('*') > -1){
                        string[] name_x_no_x = name.Split('*');
                        name = name_x_no_x[0];
                        
                        Encoding      charset     = Encoding.ASCII;
                        StringBuilder valueBuffer = new StringBuilder();
                        // We must have charset'language'value.
                        // Examples:
                        //      URL*=utf-8''test;
                        //      URL*0*=utf-8''"test";
                        if((name_x_no_x.Length == 2 && name_x_no_x[1] == "") || name_x_no_x.Length == 3){                            
                            string[] charset_language_value = value.Split('\'');
                            charset = Encoding.GetEncoding(charset_language_value[0]);
                            valueBuffer.Append(DecodeExtOctet(charset_language_value[2],charset));
                        }
                        // No encoding, probably just splitted ASCII value.
                        // Example:
                        //     URL*0="value1";
                        //     URL*1="value2";
                        else{
                            valueBuffer.Append(value);
                        }

                        // Read while value continues.
                        while(true){
                            // End os stream reached.
                            if(reader.Peek(true) == -1){
                                break;
                            }
                            // Next parameter start, just eat that char.
                            else if(reader.Peek(true) == ';'){
                                reader.Char(false);
                            }
                            else{
                                if(!reader.StartsWith(name + "*")){
                                    break;
                                }
                                reader.Token();

                                // Parameter value specified.
                                if(reader.Peek(true) == '='){
                                    reader.Char(false);

                                    string v = reader.Word();
                                    // Normally value may not be null, but following case: paramName=EOS.
                                    if(v != null){
                                        valueBuffer.Append(DecodeExtOctet(v,charset));
                                    }
                                }
                            }
                        }
                                               
                        this[name] = valueBuffer.ToString();
                    }
                    // Regular parameter.
                    else{
                        this[name] = value;
                    }                    
                }
            }

            m_IsModified = false;
        }

        #endregion

        

        #region static method DecodeExtOctet

        /// <summary>
        /// Decodes non-ascii text with MIME <b>ext-octet</b> method. Defined in RFC 2231 7.
        /// </summary>
        /// <param name="text">Text to decode,</param>
        /// <param name="charset">Charset to use.</param>
        /// <returns>Returns decoded text.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>text</b> or <b>charset</b> is null.</exception>
        private static string DecodeExtOctet(string text,Encoding charset)
        {
            if(text == null){
                throw new ArgumentNullException("text");
            }
            if(charset == null){
                throw new ArgumentNullException("charset");
            }

            int    offset        = 0;
            byte[] decodedBuffer = new byte[text.Length];            
            for(int i=0;i<text.Length;i++){
                if(text[i] == '%'){
                    decodedBuffer[offset++] = byte.Parse(text[i + 1].ToString() + text[i + 2].ToString(),System.Globalization.NumberStyles.HexNumber);
                    i += 2;
                }
                else{
                    decodedBuffer[offset++] = (byte)text[i];
                }
            }

            return charset.GetString(decodedBuffer,0,offset);
        }

        #endregion


        #region interface IEnumerator

        /// <summary>
		/// Gets enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return m_pParameters.GetEnumerator();
		}

		#endregion

        #region Properties implementation

        /// <summary>
        /// Gets if this header field parameters are modified since it has loaded.
        /// </summary>
        /// <remarks>All new added header fields has <b>IsModified = true</b>.</remarks>
        /// <exception cref="ObjectDisposedException">Is riased when this class is disposed and this property is accessed.</exception>
        public bool IsModified
        {
            get{
                if(m_IsModified){
                    return true;
                }
                else{
                    foreach(MIME_h_Parameter parameter in this.ToArray()){
                        if(parameter.IsModified){
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// Gets owner MIME header field.
        /// </summary>
        public MIME_h Owner
        {
            get{ return m_pOwner; }
        }

        /// <summary>
        /// Gets number of items in the collection.
        /// </summary>
        public int Count
        {
            get{ return m_pParameters.Count; }
        }

        /// <summary>
        /// Gets or sets specified header field parameter value. Value null means not specified.
        /// </summary>
        /// <param name="name">Header field name.</param>
        /// <returns>Returns specified header field value or null if specified parameter doesn't exist.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>name</b> is null reference.</exception>
        public string this[string name]
        {
            get{
                if(name == null){
                    throw new ArgumentNullException("name");
                }

                MIME_h_Parameter retVal = null;
                if(m_pParameters.TryGetValue(name,out retVal)){
                    return retVal.Value;
                }
                else{
                    return null;
                }
            }

            set{
                if(name == null){
                    throw new ArgumentNullException("name");
                }

                MIME_h_Parameter retVal = null;
                if(m_pParameters.TryGetValue(name,out retVal)){
                    retVal.Value = value;
                }
                else{
                    m_pParameters.Add(name,new MIME_h_Parameter(name,value));
                }
            }
        }

        #endregion
    }
}

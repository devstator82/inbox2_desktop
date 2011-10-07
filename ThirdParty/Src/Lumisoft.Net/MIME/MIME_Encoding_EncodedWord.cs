using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.MIME
{
    /// <summary>
    /// Implements 'encoded-word' encoding. Defined in RFC 2047.
    /// </summary>
    public class MIME_Encoding_EncodedWord
    {
        private MIME_EncodedWordEncoding m_Encoding;
        private Encoding                 m_pCharset = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="encoding">Encoding to use to encode text.</param>
        /// <param name="charset">Charset to use for encoding. If not sure UTF-8 is strongly recommended.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>charset</b> is null reference.</exception>
        public MIME_Encoding_EncodedWord(MIME_EncodedWordEncoding encoding,Encoding charset)
        {
            if(charset == null){
                throw new ArgumentNullException("charset");
            }

            m_Encoding = encoding;
            m_pCharset = charset;
        }

                
        #region method Encode

        /// <summary>
        /// Encodes specified text if it contains 8-bit chars, otherwise text won't be encoded.
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <returns>Returns encoded text.</returns>
        public string Encode(string text)
        {            
            if(MustEncode(text)){
                return EncodeS(m_Encoding,m_pCharset,text);
            }
            else{
                return text;
            }
        }

        #endregion

        #region method Decode

        /// <summary>
        /// Decodes specified encoded-word.
        /// </summary>
        /// <param name="text">Encoded-word value.</param>
        /// <returns>Returns decoded text.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>text</b> is null reference.</exception>
        public string Decode(string text)
        {
            if(text == null){
                throw new ArgumentNullException("text");
            }

            return DecodeS(text);
        }

        #endregion


        #region static method MustEncode

        /// <summary>
        /// Checks if specified text must be encoded.
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <returns>Returns true if specified text must be encoded, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>text</b> is null reference.</exception>
        public static bool MustEncode(string text)
        {
            if(text == null){
                throw new ArgumentNullException("text");
            }

            // Encoding is needed only for non-ASCII chars.

            foreach(char c in text){
                if(c > 127){
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region static method EncodeS

        /// <summary>
        /// Encodes specified text if it contains 8-bit chars, otherwise text won't be encoded.
        /// </summary>
        /// <param name="encoding">Encoding to use to encode text.</param>
        /// <param name="charset">Charset to use for encoding. If not sure UTF-8 is strongly recommended.</param>
        /// <param name="text">Text to encode.</param>
        /// <returns>Returns encoded text.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>charset</b> or <b>text</b> is null reference.</exception>
        public static string EncodeS(MIME_EncodedWordEncoding encoding,Encoding charset,string text)
        {
            if(charset == null){
                throw new ArgumentNullException("charset");
            }
            if(text == null){
                throw new ArgumentNullException("text");
            }

            /* RFC 2047 2.
                encoded-word = "=?" charset "?" encoding "?" encoded-text "?="
             
                An 'encoded-word' may not be more than 75 characters long, including
                'charset', 'encoding', 'encoded-text', and delimiters.  If it is
                desirable to encode more text than will fit in an 'encoded-word' of
                75 characters, multiple 'encoded-word's (separated by CRLF SPACE) may
                be used.
             
               RFC 2231 (updates syntax)
                encoded-word := "=?" charset ["*" language] "?" encoded-text "?="
            */

            if(MustEncode(text)){
                StringBuilder retVal             = new StringBuilder();
                byte[]        data               = charset.GetBytes(text);
                int           maxEncodedTextSize = 75 - ((string)("=?" + charset.WebName + "?" + encoding.ToString() + "?" + "?=")).Length;

                #region B encode

                if(encoding == MIME_EncodedWordEncoding.B){
                    retVal.Append("=?" + charset.WebName + "?B?");
                    int    stored = 0;
                    string base64 = Convert.ToBase64String(data);
                    for(int i=0;i<base64.Length;i+=4){
                        // Encoding buffer full, create new encoded-word.
                        if(stored + 4 > maxEncodedTextSize){
                            retVal.Append("?=\r\n =?" + charset.WebName + "?B?");
                            stored = 0;
                        }

                        retVal.Append(base64,i,4);
                        stored += 4;
                    }
                    retVal.Append("?=");
                }

                #endregion

                #region Q encode

                else{
                    retVal.Append("=?" + charset.WebName + "?Q?");
                    int stored = 0;
                    foreach(byte b in data){
                        string val = null;
                        // We need to encode byte. Defined in RFC 2047 4.2.
                        if(b > 127 || b == '=' || b == '?' || b == '_' || b == ' '){
                            val = "=" + b.ToString("X2");
                        }
                        else{
                            val = ((char)b).ToString();
                        }

                        // Encoding buffer full, create new encoded-word.
                        if(stored + val.Length > maxEncodedTextSize){
                            retVal.Append("?=\r\n =?" + charset.WebName + "?Q?");
                            stored = 0;
                        }

                        retVal.Append(val);
                        stored += val.Length;
                    }
                    retVal.Append("?=");
                }

                #endregion

                return retVal.ToString();
            }
            else{
                return text;
            }
        }

        #endregion

        #region static method DecodeS

		public static string DecodeS(string word)
		{
			try
			{
				// MWA 26-02 fix for encodings that send multiple mime encoded words in the header
				// For example =?TIS-620?Q?=B7=B4=CA=CD=BA=A1=D2=C3=CA=E8=A7?= =?TIS-620?Q?=B4=E9=C7=C2=C0=D2=C9=D2=E4=B7=C2_?= =?TIS-620?Q?[Test_Send_Thai_Message]?=
				string[] wordParts = word.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

				StringBuilder sb = new StringBuilder();

				foreach (var wp in wordParts)
				{
					sb.Append(DecodeSWord(wp) + " ");
				}

				return sb.ToString().Trim();
			}
			catch
			{
				// Failed to parse encoded-word, leave it as is. RFC 2047 6.3.
				return word;
			}
		}

        /// <summary>
        /// Decodes non-ascii word with MIME <b>encoded-word</b> method. Defined in RFC 2047 2.
        /// </summary>
        /// <param name="word">MIME encoded-word value.</param>
        /// <returns>Returns decoded word.</returns>
        /// <remarks>If <b>word</b> is not encoded-word or has invalid syntax, <b>word</b> is leaved as is.</remarks>
        /// <exception cref="ArgumentNullException">Is raised when <b>word</b> is null reference.</exception>
        public static string DecodeSWord(string word)
        {
            if(word == null){
                throw new ArgumentNullException("word");
            }

            /* RFC 2047 2.
                encoded-word = "=?" charset "?" encoding "?" encoded-text "?="
             
               RFC 2231.
                encoded-word := "=?" charset ["*" language] "?" encoded-text "?="
            */

            try{
                string[] parts = word.Split('?');
                // Not encoded-word.
                if(parts.Length != 5){
                    return word;
                }
                else if(parts[2].ToUpper() == "Q"){
                    return Core.QDecode(Encoding.GetEncoding(parts[1].Split('*')[0]),parts[3]);
                }
                else if(parts[2].ToUpper() == "B"){                        
                    return Encoding.GetEncoding(parts[1].Split('*')[0]).GetString(Core.Base64Decode(Encoding.Default.GetBytes(parts[3])));
                }
                // Unknown encoding.
                else{
                    return word;
                }
            }
            catch{
                // Failed to parse encoded-word, leave it as is. RFC 2047 6.3.
                return word;
            }
        }

        #endregion

    }
}

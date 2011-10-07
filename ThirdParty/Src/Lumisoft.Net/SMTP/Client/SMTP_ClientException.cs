using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.SMTP.Client
{
    /// <summary>
    /// SMTP client exception.
    /// </summary>
    public class SMTP_ClientException : Exception
    {
        private int    m_StatusCode   = 500;
        private string m_ResponseText = "";

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="responseLine">SMTP server response line.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>responseLine</b> is null.</exception>
        public SMTP_ClientException(string responseLine) : base(responseLine)
        {
            if(responseLine == null){
                throw new ArgumentNullException("responseLine");
            }

            string[] code_text = responseLine.Split(new char[]{' ','-'},2);
            try{
                m_StatusCode = Convert.ToInt32(code_text[0]);
            }
            catch{
            }
            if(code_text.Length == 2){
                m_ResponseText =  code_text[1];                
            }
        }


        #region Properties Implementation

        /// <summary>
        /// Gets SMTP status code.
        /// </summary>
        public int StatusCode
        {
            get{ return m_StatusCode; }
        }

        /// <summary>
        /// Gets SMTP server response text after status code.
        /// </summary>
        public string ResponseText
        {
            get{ return m_ResponseText; }
        }

        /// <summary>
        /// Gets if it is permanent SMTP(5xx) error.
        /// </summary>
        public bool IsPermanentError
        {
            get{
                if(m_StatusCode >= 500 && m_StatusCode <= 599){
                    return true;
                }
                else{
                    return false;
                }
            }
        }

        #endregion

    }
}

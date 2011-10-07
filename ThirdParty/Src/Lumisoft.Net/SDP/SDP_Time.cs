using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.SDP
{
    /// <summary>
    /// A SDP_Time represents an <B>t=</B> SDP message field. Defined in RFC 4566 5.9. Timing.
    /// </summary>
    public class SDP_Time
    {
        private long m_StartTime = 0;
        private long m_StopTime  = 0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SDP_Time()
        {
        }


        #region static method Parse

        /// <summary>
        /// Parses media from "t" SDP message field.
        /// </summary>
        /// <param name="tValue">"t" SDP message field.</param>
        /// <returns></returns>
        public static SDP_Time Parse(string tValue)
        {
            // t=<start-time> <stop-time>

            SDP_Time time = new SDP_Time();

            // Remove t=
            StringReader r = new StringReader(tValue);
            r.QuotedReadToDelimiter('=');

            //--- <start-time> ------------------------------------------------------------
            string word = r.ReadWord();
            if(word == null){
                throw new Exception("SDP message \"t\" field <start-time> value is missing !");
            }
            time.m_StartTime = Convert.ToInt64(word);

            //--- <stop-time> -------------------------------------------------------------
            word = r.ReadWord();
            if(word == null){
                throw new Exception("SDP message \"t\" field <stop-time> value is missing !");
            }
            time.m_StopTime = Convert.ToInt64(word);

            return time;
        }

        #endregion

        #region method ToValue

        /// <summary>
        /// Converts this to valid "t" string.
        /// </summary>
        /// <returns></returns>
        public string ToValue()
        {
            // t=<start-time> <stop-time>

            return "t=" + StartTime + " " + StopTime + "\r\n";
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets or sets start time when session must start. Network Time Protocol (NTP) time values in 
        /// seconds since 1900. 0 value means not specified, if StopTime is also 0, then means infinite session.
        /// </summary>
        public long StartTime
        {
            get{ return m_StartTime; }

            set{
                if(value < 0){
                    throw new ArgumentException("Property StartTime value must be >= 0 !");
                }

                m_StopTime = value;
            }
        }

        /// <summary>
        /// Gets or sets stop time when session must end. Network Time Protocol (NTP) time values in 
        /// seconds since 1900. 0 value means not specified, if StopTime is also 0, then means infinite session.
        /// </summary>
        public long StopTime
        {
            get{ return m_StopTime; }

            set{
                if(value < 0){
                    throw new ArgumentException("Property StopTime value must be >= 0 !");
                }

                m_StopTime = value;
            }
        }

        #endregion

    }
}

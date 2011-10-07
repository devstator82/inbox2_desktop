using System;
using System.Collections.Generic;

namespace LumiSoft.Net.IMAP
{
	/// <summary>
	/// IMAP sequence-set. RFC 3501.
	/// <code>
	/// Examples:
	///		2        -> seq-number (2)
	///		2:4      -> seq-range  (from 2 - 4)
	///		2:*      -> seq-range  (from 2 to last)
	///		2,3,10:* -> sequence-set (seq-number,seq-number,seq-range)
	///		                       (2,3, 10 - last)
	///		
	///		NOTES:
	///			*) comma separates sequence parts
	///			*) * means maximum value.
	/// </code>
	/// </summary>
	public class IMAP_SequenceSet
    {
        #region class Range

        /// <summary>
        /// Implements range.
        /// </summary>
        private class Range
        {
            private long m_Start = 0;
            private long m_End   = 0;

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="value">Range value.</param>
            public Range(long value)
            {
                m_Start = value;
                m_End   = value;
            }

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="start">Range start.</param>
            /// <param name="end">Range end.</param>
            public Range(long start,long end)
            {
                m_Start = start;
                m_End   = end;
            }


            #region Properties Implementation

            /// <summary>
            /// Gets range start.
            /// </summary>
            public long Start
            {
                get{ return m_Start; }
            }

            /// <summary>
            /// Gets range end.
            /// </summary>
            public long End
            {
                get{ return m_End; }
            }

            #endregion
        }

        #endregion

        private List<Range> m_pSequenceParts = null;
        private string      m_SequenceString = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		public IMAP_SequenceSet()
		{
            m_pSequenceParts = new List<Range>();
		}

		public IMAP_SequenceSet(string sequenceSetString)
		{
			m_pSequenceParts = new List<Range>();
			Parse(sequenceSetString, long.MaxValue);
		}

		#region method Parse

        /// <summary>
		/// Parses sequence-set from specified string. Throws exception if invalid sequnce-set value.
		/// </summary>
		/// <param name="sequenceSetString">Sequence-set string.</param>
		public void Parse(string sequenceSetString)
		{
            Parse(sequenceSetString,long.MaxValue);
        }

		/// <summary>
		/// Parses sequence-set from specified string. Throws exception if invalid sequnce-set value.
		/// </summary>
		/// <param name="sequenceSetString">Sequence-set string.</param>
		/// <param name="seqMaxValue">Maximum value. This if for replacement of * value.</param>
		public void Parse(string sequenceSetString,long seqMaxValue)
		{
			/* RFC 3501
				seq-number     = nz-number / "*"
								; message sequence number (COPY, FETCH, STORE
								; commands) or unique identifier (UID COPY,
								; UID FETCH, UID STORE commands).
								; * represents the largest number in use.  In
								; the case of message sequence numbers, it is
								; the number of messages in a non-empty mailbox.
								; In the case of unique identifiers, it is the
								; unique identifier of the last message in the
								; mailbox or, if the mailbox is empty, the
								; mailbox's current UIDNEXT value.
								; The server should respond with a tagged BAD
								; response to a command that uses a message
								; sequence number greater than the number of
								; messages in the selected mailbox.  This
								; includes "*" if the selected mailbox is empty.

				seq-range      = seq-number ":" seq-number
								; two seq-number values and all values between
								; these two regardless of order.
								; Example: 2:4 and 4:2 are equivalent and indicate
								; values 2, 3, and 4.
								; Example: a unique identifier sequence range of
								; 3291:* includes the UID of the last message in
								; the mailbox, even if that value is less than 3291.

				sequence-set    = (seq-number / seq-range) *("," sequence-set)
								; set of seq-number values, regardless of order.
								; Servers MAY coalesce overlaps and/or execute the
								; sequence in any order.
								; Example: a message sequence number set of
								; 2,4:7,9,12:* for a mailbox with 15 messages is
								; equivalent to 2,4,5,6,7,9,12,13,14,15
								; Example: a message sequence number set of *:4,5:7
								; for a mailbox with 10 messages is equivalent to
								; 10,9,8,7,6,5,4,5,6,7 and MAY be reordered and
								; overlap coalesced to be 4,5,6,7,8,9,10.
			*/

			//--- Validate sequence-set --------------------------------------------------------//
			string[] sequenceSets = sequenceSetString.Trim().Split(',');
			foreach(string sequenceSet in sequenceSets){
				// seq-range 
				if(sequenceSet.IndexOf(":") > -1){
					string[] rangeParts = sequenceSet.Split(':');
					if(rangeParts.Length == 2){
                        long start = Parse_Seq_Number(rangeParts[0],seqMaxValue);
                        long end   = Parse_Seq_Number(rangeParts[1],seqMaxValue);
                        if(start <= end){
                            m_pSequenceParts.Add(new Range(start,end));
                        }
                        else{
                            m_pSequenceParts.Add(new Range(end,start));
                        }                        			
					}
					else{
						throw new Exception("Invalid <seq-range> '" + sequenceSet + "' value !");
					}
				}
				// seq-number
				else{
					m_pSequenceParts.Add(new Range(Parse_Seq_Number(sequenceSet,seqMaxValue)));
				}
			}
			//-----------------------------------------------------------------------------------//

            m_SequenceString = sequenceSetString;

		}

		#endregion

		#region method Contains

		/// <summary>
		/// Gets if sequence set contains specified number.
		/// </summary>
		/// <param name="seqNumber">Number to check.</param>
		public bool Contains(long seqNumber)
		{
			foreach(Range range in m_pSequenceParts){
                if(seqNumber >= range.Start && seqNumber <= range.End){
                    return true;
                }
            }

			return false;
		}

		#endregion

		#region method ToSequenceSetString

		/// <summary>
		/// Converts IMAP_SequenceSet to IMAP sequence-set string.
		/// </summary>
		/// <returns></returns>
		public string ToSequenceSetString()
		{
			return m_SequenceString;
		}

		#endregion


		#region method Parse_Seq_Number

		/// <summary>
		/// Parses seq-number from specified value. Throws exception if invalid seq-number value.
		/// </summary>
		/// <param name="seqNumberValue">Integer number or *.</param>
		/// <param name="seqMaxValue">Maximum value. This if for replacement of * value.</param>
		private long Parse_Seq_Number(string seqNumberValue,long seqMaxValue)
		{
			seqNumberValue = seqNumberValue.Trim();

			// * max value
			if(seqNumberValue == "*"){
				// Replace it with max value
				return seqMaxValue;
			}
			// Number
			else{
				try{
					return Convert.ToInt64(seqNumberValue);
				}
				catch{
					throw new Exception("Invalid <seq-number> '" + seqNumberValue + "' value !");
				}
			}
		}

		#endregion

	}
}

// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>
using System;

namespace HtmlAgilityPack
{

    /// <summary>
    /// Represents a parsing error found during document parsing.
    /// </summary>
    public class HtmlParseError
    {
        private HtmlParseErrorCode _code;
        private int _line;
        private int _linePosition;
        private int _streamPosition;
        private string _sourceText;
        private string _reason;

        internal HtmlParseError(
            HtmlParseErrorCode code,
            int line,
            int linePosition,
            int streamPosition,
            string sourceText,
            string reason)
        {
            _code = code;
            _line = line;
            _linePosition = linePosition;
            _streamPosition = streamPosition;
            _sourceText = sourceText;
            _reason = reason;
        }

        /// <summary>
        /// Gets the type of error.
        /// </summary>
        public HtmlParseErrorCode Code
        {
            get
            {
                return _code;
            }
        }

        /// <summary>
        /// Gets the line number of this error in the document.
        /// </summary>
        public int Line
        {
            get
            {
                return _line;
            }
        }

        /// <summary>
        /// Gets the column number of this error in the document.
        /// </summary>
        public int LinePosition
        {
            get
            {
                return _linePosition;
            }
        }

        /// <summary>
        /// Gets the absolute stream position of this error in the document, relative to the start of the document.
        /// </summary>
        public int StreamPosition
        {
            get
            {
                return _streamPosition;
            }
        }

        /// <summary>
        /// Gets the the full text of the line containing the error.
        /// </summary>
        public string SourceText
        {
            get
            {
                return _sourceText;
            }
        }

        /// <summary>
        /// Gets a description for the error.
        /// </summary>
        public string Reason
        {
            get
            {
                return _reason;
            }
        }
    }
}

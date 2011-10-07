using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Channels.Text
{
    public class Base64Helper
    {
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            return System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Inbox2.Core.Configuration;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interop;

namespace Inbox2.Framework.VirtualMailBox
{
    public static class Inbox2Environment
    {
        
        /// <summary>
        /// Based on: http://andrewensley.com/2009/06/c-detect-windows-os-part-1/
        /// </summary>
        /// <returns></returns>
        public static string GetOperatingSystem()
        {
            //Get Operating system information.
            OperatingSystem os = System.Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "";

            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        operatingSystem = vs.Minor == 0 ? "2000" : "XP";
                        break;
                    case 6:
                        operatingSystem = vs.Minor == 0 ? "Vista" : "7";
                        break;
                    default:
                        break;
                }
            }

            return operatingSystem;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Configuration;

namespace Inbox2.Framework.Plugins.SharedControls.Profiles
{
    internal class ChannelColorHelper
    {
        public static string GetChannelColor(ChannelConfiguration config)
        {
            switch (config.DisplayName)
            {
                case "AOL":
                    return "#adcbe2";
                case "Exchange":
                    return "#ffffff";
                case "Facebook":
                    return "#6597ff";
                case "GMail":
                    return "#de6161";
                case "Hyves":
                    return "#ffab04";
                case "LinkedIn":
                    return "#1f9fdf";
                case "Live":
                    return "#ffffff";
                case "Twitter":
                    return "#77dbf3";
                case "Yahoo":
                    return "#82138c";
                case "Yammer":
                    return "#00b3da";
                default:
                    return "#124";
            }
        }
    }
}

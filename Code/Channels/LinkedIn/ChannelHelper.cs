using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;

namespace Inbox2.Channels.LinkedIn
{
    public static class ChannelHelper
    {
        public const string ConsumerKey = "dIGHvmnGRaola5G-nWZnLyo-dxxMfKcu0XwkW4TNMrEiPa8MSNzX_gPLUoX7onBU";
        public const string ConsumerSecret = "z1JhKifIwPARS6_p58zoc_i9nhL1gZG9xX23csxGBb3aBiyT1iwy4G6K3gvFofa5";

        public static string Token
        {
            get { return ChannelContext.Current.ClientContext.GetSetting("/Channels/LinkedIn/AuthToken") as string; }
        }

        public static string TokenSecret
        {
            get { return ChannelContext.Current.ClientContext.GetSetting("/Channels/LinkedIn/AuthSecret") as string; }
        }
    }
}
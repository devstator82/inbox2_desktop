/**************************************************************************\
    Copyright Microsoft Corporation. All Rights Reserved.
\**************************************************************************/

// This file contains general utilities to aid in development.
// Classes here generally shouldn't be exposed publicly since
// they're not particular to any library functionality.
// Because the classes here are internal, it's likely this file
// might be included in multiple assemblies.
namespace Standard
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    internal static class Utility
    {
        public static int GET_X_LPARAM(IntPtr lParam)
        {
            return LOWORD(lParam.ToInt32());
        }

        public static int GET_Y_LPARAM(IntPtr lParam)
        {
            return HIWORD(lParam.ToInt32());
        }

        public static int HIWORD(int i)
        {
            return (short)(i >> 16);
        }

        public static int LOWORD(int i)
        {
            return (short)(i & 0xFFFF);
        }

        public static bool IsFlagSet(int value, int mask)
        {
            return 0 != (value & mask);
        }
    }
}

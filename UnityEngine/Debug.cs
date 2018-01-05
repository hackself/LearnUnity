// -----------------------------------------------------------------
// File:    Debug
// Author:  ruanban
// Date:    1/4/2018 4:26:14 PM
// Description:
//      
// -----------------------------------------------------------------

using System;
using System.Text;

namespace UnityEngine
{
    public static class Debug
    {
        public static void Init()
        {
        }

        public static void LogColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
        public static void Log(object obj)
        {
            var str = obj.ToString();
            Console.WriteLine(str);
        }

        public static void Error(object obj)
        {
            Log(obj, ConsoleColor.Red);
        }

        public static void Log(object obj, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            LogColor(color);
            Console.WriteLine(obj.ToString());
            LogColor(oldColor);
        }
    }
}

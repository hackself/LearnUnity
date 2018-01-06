// -----------------------------------------------------------------
// File:    Math
// Author:  ruanban
// Date:    1/6/2018 2:27:09 PM
// Description:
//      
// -----------------------------------------------------------------
using System.Runtime.InteropServices;
namespace NativeDllTest
{
    public static class Math
    {
        const string MathDll = "NativeDllTest.dll";
        [DllImport(MathDll,EntryPoint ="add")]
        extern public static double add(double a, double b);
        [DllImport(MathDll,EntryPoint ="sub")]
        extern public static double sub(double a, double b);
        [DllImport(MathDll,EntryPoint ="mul")]
        extern public static double mul(double a, double b);
        [DllImport(MathDll,EntryPoint ="div")]
        extern public static double div(double a, double b);
    }
}

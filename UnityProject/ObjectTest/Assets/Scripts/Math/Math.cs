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
    public static class NativeMath
    {
        const string MathDll = "NativeDllTest";
        [DllImport(MathDll,EntryPoint ="add")]
        extern public static double Add(double a, double b);
        [DllImport(MathDll,EntryPoint ="sub")]
        extern public static double Sub(double a, double b);
        [DllImport(MathDll,EntryPoint ="mul")]
        extern public static double Mul(double a, double b);
        [DllImport(MathDll,EntryPoint ="div")]
        extern public static double Div(double a, double b);
    }
}

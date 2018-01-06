
using UnityEngine;

namespace NativeDllTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Log(NativeMath.Add(1.1, 1.1));
            Debug.Log(NativeMath.Div(1, 1));
            System.Console.ReadKey();
        }
    }
}

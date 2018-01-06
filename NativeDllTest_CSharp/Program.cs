
using UnityEngine;

namespace NativeDllTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Log(Math.add(1.1, 1.1));
            Debug.Log(Math.div(1, 1));
            System.Console.ReadKey();
        }
    }
}

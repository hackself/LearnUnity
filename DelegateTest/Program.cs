using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelegateTest
{
    class Program
    {
        /* 
         *  class TestDelegate : MutilDelegate
            {
            }
         
         */
        delegate void TestDelegate();
        delegate void TestDelegate<T>(T a);

        static event TestDelegate myEvent;

        static TestDelegate myDelegate;
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; ++i)
            {
                myDelegate += () =>
                {
                    Console.WriteLine("new delegatel 1");
                };
            }

            int a = 10;
            for (int i = 0; i < 5; ++i)
            {
                myDelegate += delegate ()
                {
                    Console.WriteLine("new delegatel 2:" + a);
                };
            }

            myDelegate = () =>
            {
                Console.WriteLine("new delegatel 2");
            };

            myDelegate();

            Console.ReadKey();
        }
    }
}

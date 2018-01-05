//模拟实现Unity的协程
//by ruanban 2017/12/29

using System;
using System.Collections;
using UnityEngine;

namespace CoroutineTest
{

    public class CoroutineTest : MonoBehaviour
    {
        IEnumerator Test_1()
        {
            for (int i = 1; i < 6; ++i)
            {
                yield return new WaitForSeconds(1);
                Debug.Log("Test_1 i:" + i);
                yield return StartCoroutine(Test_2(i));
            }
        }

        IEnumerator Test_2(int value)
        {
            for (int i = 1; i < 6; ++i)
            {
                yield return new WaitForSeconds(2);
                Debug.Log("Test_2:" + (10 * value + i));
            }
        }

        public void CoroutineRun()
        {
            StartCoroutine(Test_1());
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Application.Instance.Run(false,() =>
            {
                var obj = new GameObject();
                var test = obj.AddComponent<CoroutineTest>();
                test.CoroutineRun();
            });
            Application.Instance.Run();
        }
    }
}


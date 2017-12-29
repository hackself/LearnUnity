//模拟实现Unity的协程
//by ruanban 2017/12/29

using System;
using System.Collections;
using System.Collections.Generic;

namespace LearnUnity
{

    abstract public class YieldInstruct
    {
        abstract public bool IsWait
        {
            get;
        }
    }

    public class WaitForSecond : YieldInstruct
    {
        private DateTime _startTime;
        private float _waitTime;
        public override bool IsWait
        {
            get
            {
                var dt = DateTime.Now - this._startTime;
                return dt.TotalSeconds < this._waitTime;
            }
        }

        public WaitForSecond(float time)
        {
            this._startTime = DateTime.Now;
            this._waitTime = time;
        }
    }

    public class Coroutine : YieldInstruct, IEnumerator
    {
        private bool _isDie = false;
        private IEnumerator _em;
        public Coroutine(IEnumerator em)
        {
            this._em = em;
        }

        private YieldInstruct _cur = null;

        public override bool IsWait
        {
            get
            {
                return !this._isDie;
            }
        }

        public object Current
        {
            get
            {
                return _cur;
            }
        }

        public bool MoveNext()
        {
            if (_cur == null || !_cur.IsWait)
            {
                if (!this._em.MoveNext())
                {
                    this._isDie = true;
                    return false;
                }
                this._cur = this._em.Current as YieldInstruct;
            }
            return true;
        }

        public void Reset()
        {
            _cur = null;
            _em.Reset();
        }
    }

    public class CoroutineManager
    {
        private List<Coroutine> _co_add = new List<Coroutine>();
        private List<Coroutine> _co = new List<Coroutine>();
        private List<Coroutine> _co_rm = new List<Coroutine>();

        private static CoroutineManager _manager;
        public static CoroutineManager Instance
        {
            get
            {
                if (_manager == null)
                {
                    _manager = new CoroutineManager();
                }
                return _manager;
            }
        }
        public Coroutine StartCoroutine(IEnumerator em)
        {
            if (em != null)
            {
                var co = new Coroutine(em);
                _co_add.Add(co);
                return co;
            }
            return null;
        }

        public void StopCoroutine(Coroutine co)
        {
            if (co != null)
            {
                _co_rm.Remove(co);
            }
        }

        public void Update()
        {
            if (_co_add.Count > 0)
            {
                _co.AddRange(_co_add);
                _co_add.Clear();
            }
            foreach (var co in _co)
            {
                if (!co.MoveNext())
                {
                    StopCoroutine(co);
                }
            }

            if (_co_rm.Count > 0)
            {
                _co_rm.ForEach((co_rm) =>
                {
                    StopCoroutine(co_rm);
                });
                _co_rm.Clear();
            }
        }
    }


    public class MonoBehaviour
    {

        public Coroutine StartCoroutine(IEnumerator em)
        {
            return CoroutineManager.Instance.StartCoroutine(em);
        }

        public void StopCoroutine(Coroutine co)
        {
            CoroutineManager.Instance.StopCoroutine(co);
        }
    }

    public class CoroutineTest : MonoBehaviour
    {
        IEnumerator Test_1()
        {
            for (int i = 1; i < 6; ++i)
            {
                yield return new WaitForSecond(1);
                Console.WriteLine("Test_1 i:" + i);
                yield return StartCoroutine(Test_2(i));
            }
        }

        IEnumerator Test_2(int value)
        {
            for (int i = 1; i < 6; ++i)
            {
                yield return new WaitForSecond(2);
                Console.WriteLine("Test_2:" + (10 * value + i));
            }
        }

        public void Test()
        {
            StartCoroutine(Test_1());
        }
    }

    public class Program : MonoBehaviour
    {
        static void FrameUpdate()
        {
            //模拟游戏帧循环
            while (true)
            {
                CoroutineManager.Instance.Update();
            }
        }
        static void Main(string[] args)
        {
            var coTest = new CoroutineTest();
            coTest.Test();
            FrameUpdate();
            Console.ReadKey();
        }
    }
}


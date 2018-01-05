// -----------------------------------------------------------------
// File:    CoroutineManager
// Author:  ruanban
// Date:    1/5/2018 1:29:53 PM
// Description:
//      
// -----------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public class CoroutineManager : Singleton<CoroutineManager>
    {
        private List<Coroutine> _co_add = new List<Coroutine>();
        private List<Coroutine> _co = new List<Coroutine>();
        private List<Coroutine> _co_rm = new List<Coroutine>();
    
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
}


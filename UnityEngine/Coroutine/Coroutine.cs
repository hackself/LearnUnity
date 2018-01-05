// -----------------------------------------------------------------
// File:    Coroutine
// Author:  ruanban
// Date:    1/5/2018 1:30:48 PM
// Description:
//      
// -----------------------------------------------------------------
using System.Collections;
namespace UnityEngine
{
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

}

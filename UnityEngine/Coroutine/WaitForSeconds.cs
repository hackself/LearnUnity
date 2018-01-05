// -----------------------------------------------------------------
// File:    WaitForSecond
// Author:  ruanban
// Date:    1/5/2018 1:33:01 PM
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace UnityEngine
{
    public class WaitForSeconds : YieldInstruct
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

        public WaitForSeconds(float time)
        {
            this._startTime = DateTime.Now;
            this._waitTime = time;
        }
    }
}

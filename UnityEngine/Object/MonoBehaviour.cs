// -----------------------------------------------------------------
// File:    MonoBehaviour
// Author:  ruanban
// Date:    1/4/2018 4:58:58 PM
// Description:
//      
// -----------------------------------------------------------------

using System.Collections;

using System.Reflection;

namespace UnityEngine
{
    public class MonoBehaviour : Behaviour
    {
        internal override void SendMessage(string methodName)
        {
            if (methodName == UnityMessage.Awake)
            {
                if (this.componentState > CompnentState.UnAwake) return;
                this.componentState = CompnentState.Awake;
            }
            else if (methodName == UnityMessage.Start)
            {
                if (this.componentState > CompnentState.Awake) return;
                this.componentState = CompnentState.Start;
            }
            base.SendMessage(methodName);
            if (methodName == UnityMessage.OnDestroy || this.enabled)
            {
                var method = this.GetType().GetMethod(methodName,
                    BindingFlags.Instance |
                    BindingFlags.InvokeMethod |
                    BindingFlags.NonPublic |
                    BindingFlags.Public);
                if (method != null)
                    method.Invoke(this, null);
            }
        }

        public Coroutine StartCoroutine(IEnumerator em)
        {
            return CoroutineManager.Instance.StartCoroutine(em);
        }

        public void StopCoroutine(Coroutine co)
        {
            CoroutineManager.Instance.StopCoroutine(co);
        }
    }
}

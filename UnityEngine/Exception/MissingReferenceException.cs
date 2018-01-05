// -----------------------------------------------------------------
// File:    MissingReferenceException
// Author:  ruanban
// Date:    1/5/2018 3:22:07 PM
// Description:
//      
// -----------------------------------------------------------------

namespace UnityEngine
{
    internal class MissingReferenceException : System.Exception
    {
        private System.WeakReference _object;
        public MissingReferenceException(Object obj)
        {
            this._object = new System.WeakReference(obj);
        }
        public override string ToString()
        {
            return string.Format("MissingReferenceException: The object of type '{0}' has been destroyed but you are still trying to access it.\nYour script should either check if it is null or you should not destroy the object.{0}", _object.Target.GetType().Name);
        }
    }
}

// -----------------------------------------------------------------
// File:    Object
// Author:  ruanban
// Date:    1/4/2018 3:55:14 PM
// Description:
//      
// -----------------------------------------------------------------

using UnityEngine;
using System.Diagnostics;

namespace UnityEngine
{
    public class Object
    {
        protected enum ObjectState
        {
            Active,
            WaitForDestroy,
            Destroy,
        }
              
        private static int instance_id_counter = 0;
        private int _instance_id = 0;

        public Object()
        {
            this.state = ObjectState.Active;
            _instance_id = ++instance_id_counter;
        }

        private string _name="GameObject";
        virtual public string name
        {
            set
            {
                this.CheckDestroy();
                this._name = value;
            }
            get { this.CheckDestroy(); return _name; }
        }

        private ObjectState _state;
        protected ObjectState state
        {
            set
            {
                this.CheckDestroy();
                this._state = value;
            }
            get
            {
                this.CheckDestroy();
                return this._state;
            }
        }

        protected Scene scene
        {
            get {  this.CheckDestroy(); return SceneManager.Instance.currentScene; }
        }

        [Conditional("DEBUG")]
        protected void CheckDestroy()
        {
            if (this._state == ObjectState.Destroy)
            {
                throw new MissingReferenceException(this);
            }
        }

        virtual protected void __OnDestroy__()
        {

        }

        virtual protected void __OnWaitDestroy__()
        {

        }

        private static bool IsDestroyTransform(Object obj)
        {
            if (obj is Transform)
            {
                Debug.Error("Can't destroy Transform component of 'Transform'. If you want to destroy the game object, please call 'Destroy' on the game object instead. Destroying the transform component is not allowed.");
                return true;
            }
            return false;
        }

        public static void Destroy(Object obj)
        {
            if (!IsDestroyTransform(obj))
            {
                __Destroy__(obj);
            }
        }
        public static void DestroyImmediate(Object obj)
        {
            if (!IsDestroyTransform(obj))
            {
                __DestroyImmediate__(obj);
            }
        }
        

        internal static void __Destroy__(Object obj)
        {
            if (obj != null)
            {
                obj.__OnWaitDestroy__();
                obj.state = ObjectState.WaitForDestroy;
            }
        }

        internal static void __DestroyImmediate__(Object obj)
        {
            if (obj != null)
            {
                obj.__OnDestroy__();
                obj.state = ObjectState.Destroy;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null &&
                this.state == ObjectState.Destroy)
            {
                return true;
            }
            return base.Equals(obj);
        }

        public int GetInstanceID()
        {
            this.CheckDestroy();
            return _instance_id;
        }

        public override int GetHashCode()
        {
            this.CheckDestroy();
            return base.GetHashCode();
        }
    }
}

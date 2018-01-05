// -----------------------------------------------------------------
// File:    Component
// Author:  ruanban
// Date:    1/4/2018 3:57:49 PM
// Description:
//      
// -----------------------------------------------------------------

namespace UnityEngine
{
    public class Component : Object
    {

        internal enum CompnentState
        {
            UnAwake,
            Awake,
            Start
        }

        internal CompnentState componentState = CompnentState.UnAwake;
        private GameObject _gameObject;
        public GameObject gameObject
        {
            get { this.CheckDestroy(); return this._gameObject; }
            private set
            {
                this.CheckDestroy();
                this._gameObject = value;
            }
        }

        override public string name
        {
            set
            {
                this.CheckDestroy();
                this.gameObject.name = value;
            }

            get
            {
                this.CheckDestroy();
                return this.gameObject.name;
            }
        }

        internal virtual void SendMessage(string methodName)
        {
        }

        internal virtual void __OnAddToGameObject__(GameObject obj)
        {
            this.gameObject = obj;
        }

        protected override void __OnWaitDestroy__()
        {
            this.scene.DestroyObject(this);
            base.__OnWaitDestroy__();
        }

        protected override void __OnDestroy__()
        {
            this.gameObject.DestroyComponentImmediate(this);
            base.__OnDestroy__();
        }
    }
}
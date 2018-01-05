// -----------------------------------------------------------------
// File:    Behaviour
// Author:  ruanban
// Date:    1/4/2018 4:59:12 PM
// Description:
//      
// -----------------------------------------------------------------
using System.Reflection;

namespace UnityEngine
{
    public class Behaviour : Component
    {
        private bool _enabled = false;
        public bool enabled
        {
            get
            {
                this.CheckDestroy();
                return this._enabled;
            }
            set
            {
                this.CheckDestroy();
                if (this._enabled == value) return;
                if (value)
                {
                    this._enabled = value;
                    this.SendEnableMessage();
                }
                else
                {
                    this.SendDisableMessage();
                    this._enabled = value;
                }
            }
        }


        public bool isActiveAndEnabled
        {
            get
            {
                this.CheckDestroy();
                return this.enabled && this.gameObject.activeInHierarchy;
            }
        }

        internal override void __OnAddToGameObject__(GameObject obj)
        {
            base.__OnAddToGameObject__(obj);
            this.enabled = true;
        }

        internal void SendEnableMessage()
        {
            this.SendMessage(UnityMessage.Awake);
            this.SendMessage(UnityMessage.OnEnable);
        }

        internal void SendDisableMessage()
        {
            this.SendMessage(UnityMessage.OnDisable);
        }
    }
}

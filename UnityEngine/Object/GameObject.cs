// -----------------------------------------------------------------
// File:    GameObject
// Author:  ruanban
// Date:    1/4/2018 3:59:09 PM
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Reflection;
using System.Collections.Generic;

namespace UnityEngine
{
    public sealed class GameObject : Object
    {
        private Transform _transform;
        public Transform transform
        {
            get { this.CheckDestroy();return this._transform; }
            internal set
            {
                this.CheckDestroy();
                this._transform = value;
            }
        }

        private bool _activeSelf = false;
        public bool activeSelf
        {
            get { this.CheckDestroy(); return this._activeSelf; }   
        }

        public void SetActive(bool value)
        {
            this.CheckDestroy();
            if (this.activeSelf == value) return;
            if (value)
            {
                this._activeSelf = value;
                this.BroadcastMessage(UnityMessage.Awake);
                this.BroadcastMessage(UnityMessage.OnEnable);
            }
            else
            {
                this.BroadcastMessage(UnityMessage.OnDisable);
                this._activeSelf = value;
            }
        }

        public bool activeInHierarchy
        {
            get
            { 
                this.CheckDestroy();
                var active = this.activeSelf;
                var parent = transform.parent;
                while (active && parent != null)
                {
                    active = parent.gameObject.activeSelf;
                    parent = parent.parent;
                }
                return active;
            }
        }

        private Dictionary<Type, Dictionary<int, Component>> _all_comps = new Dictionary<Type, Dictionary<int, Component>>();
        private List<Component> all_comps
        {
            get
            {
                List<Component> list = new List<Component>();
                foreach (var pair in this._all_comps)
                {
                    list.AddRange(pair.Value.Values);
                }
                return list;
            }
        }

        public GameObject()
            :base()
        {
            this.transform = this.AddComponent<Transform>();
            this.scene.AddGameObject(this);
            this.SetActive(true);
        }

        public GameObject(string name)
            :this()
        {
            this.name = name;
        }

        public GameObject(string name,params Type[] components)
            :this(name)
        {
            if (components != null)
            {
                foreach (var comp in components)
                {
                    this.AddComponent(comp);
                }
            }
        }

        public T AddComponent<T>() where T : Component
        {
            this.CheckDestroy();
            return this.AddComponent(typeof(T)) as T;
        }

        public Component AddComponent(Type type)
        {
            this.CheckDestroy();
            if (type != null && type.IsSubclassOf(typeof(Component)))
            {
                if (Attribute.IsDefined(type,typeof(DisallowMultipleComponentAttribute)))
                {
                    if (this.GetComponent(type) != null)
                    {
                        throw new DisallowMultipleException(type);
                    }
                }
                Dictionary<int, Component> comps = null;
                if (!this._all_comps.TryGetValue(type, out comps))
                {
                    comps = new Dictionary<int, Component>();
                    this._all_comps.Add(type, comps);
                }

                if (comps != null)
                {
                    var comp = Activator.CreateInstance(type) as Component;
                    if (comp != null)
                    {
                        comps.Add(comp.GetInstanceID(), comp);
                        comp.__OnAddToGameObject__(this);
                    }
                    return comp;
                }
            }
            return null;
        }

        public Component GetComponent(Type type)
        {
            this.CheckDestroy();
            Dictionary<int,Component> ret = null;
            if (this._all_comps.TryGetValue(type, out ret))
            {
                var e = ret.GetEnumerator();
                if (e.MoveNext()) return e.Current.Value;
            }
            return null;
        }

        public T GetComponent<T>() where T : Component
        {
            return this.GetComponent(typeof(T)) as T;
        }

        public List<Component> GetAllComponents()
        {
            List<Component> ret = new List<Component>();
            foreach (var comps in this._all_comps.Values)
            {
                ret.AddRange(comps.Values);
            }
            return ret;
        }

        internal void DestroyComponentImmediate(Component comp)
        {
            if (comp != null)
            {
                Dictionary<int, Component> comps;
                if (this._all_comps.TryGetValue(comp.GetType(), out comps) &&
                    comps.ContainsKey(comp.GetInstanceID()))
                {
                    comp.SendMessage(UnityMessage.OnDestroy);
                    comps.Remove(comp.GetInstanceID());
                }
            }
        }

        public void SendMessage(string methodName)
        {
            this.CheckDestroy();
            foreach(var comp in this.all_comps)
            {
                comp.SendMessage(methodName);
            }
        }

        public void BroadcastMessage(string methodName)
        {
            this.CheckDestroy();
            if (methodName == UnityMessage.OnDestroy || this.activeInHierarchy)
            {
                this.SendMessage(methodName);
                var count = this.transform.GetChildCount();
                for (int i = 0; i < count; ++i)
                {
                    var t = this.transform.GetChild(i);
                    if (t != null)
                    {
                        t.gameObject.BroadcastMessage(methodName);
                    }
                }
            }
        }

    
        protected override void __OnWaitDestroy__()
        {
            this.scene.DestroyObject(this);
            base.__OnWaitDestroy__();
        }

        protected override void __OnDestroy__()
        {
            this.SetActive(false);

            while (this.transform.GetChildCount() > 0)
            {
                Object.DestroyImmediate(this.transform.GetChild(0).gameObject);
            }

            var comps = this.all_comps;
            foreach (var comp in comps)
                Object.__DestroyImmediate__(comp);

            this.scene.DestroyGameObjectImmediate(this);
            base.__OnDestroy__();
        }
    }
}

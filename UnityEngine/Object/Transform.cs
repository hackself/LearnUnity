// -----------------------------------------------------------------
// File:    Transform
// Author:  ruanban
// Date:    1/4/2018 4:54:43 PM
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    public class Transform : Component
    {
        private Transform _root;
        public Transform root
        {
            get { this.CheckDestroy(); return this._root; }

            private set
            {
                this.CheckDestroy();
                this._root = value;
            }
        }

        private Transform _parent;
        public Transform parent
        {
            get { this.CheckDestroy();return this._parent; }

            set
            {
                this.CheckDestroy();
                if (value == this._parent) return;
                this.Detach();
                this._parent = value;
                this.Attach();
            }
        }

        internal override void __OnAddToGameObject__(GameObject obj)
        {
            obj.transform = this;
            base.__OnAddToGameObject__(obj);
        }


        protected override void __OnDestroy__()
        {
            this.Detach();
            this.gameObject.transform = null;
            base.__OnDestroy__();
        }


        private List<Transform> _children = new List<Transform>();

        public void Detach()
        {
            if (this.parent != null)
            {
                this.parent._children.Remove(this);
                this.scene.AddRootGameObject(this.gameObject);
            }
            this.root = this;
        }

        protected void Attach()
        {
            if (this.parent != null)
            {
                this.scene.RemoveRootGameObject(this.gameObject);
                this.parent._children.Add(this);
                this.root = this.parent.root;
            }
        }

        public void SetSiblingIndex(int index)
        {
            if (this.parent == null)
            {
                this.scene.SetRootIndex(this.gameObject, index);
            }
            else
            {
                if (this.parent._children.Remove(this))
                {
                    if (index >= 0 && index <= this.parent._children.Count)
                    {
                        this.parent._children.Insert(index, this);
                    }
                    else
                    {
                        this.parent._children.Add(this);
                    }
                }
            }
        }

        public int GetSiblingIndex()
        {
            if (this.parent == null)
            {
                return this.scene.GetRootIndex(this.gameObject);
            }
            else
            {
                return this.parent._children.IndexOf(this);
            }
        }


        public void SetAsFirstSibling()
        {
            this.SetSiblingIndex(0);
        }

        public void SetAsLastSibling()
        {
            this.SetSiblingIndex(-1);
        }

        private Transform _Find(Transform target, string name)
        {
            foreach (var t in target._children)
            {
                if (t.name == name)
                {
                    return t;
                }
            }
            return null;
        }

        public Transform Find(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var strs = name.Split('/');
                var target = this;
                for (int i = 0; i < strs.Length; ++i)
                {
                    if (target != null)
                    {
                        target = _Find(target, strs[i]);
                    }
                    else
                        break;
                }
                return target;
            }
            return null;
        }

        public Transform GetChild(int index)
        {
            if (index >= 0 && index < this._children.Count)
                return this._children[index];
            return null;
        }

        public int GetChildCount()
        {
            return this._children.Count;
        }
    }
}

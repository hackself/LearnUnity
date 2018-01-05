// -----------------------------------------------------------------
// File:    Scene
// Author:  ruanban
// Date:    1/4/2018 4:11:51 PM
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine
{
    public sealed class Scene
    {
        private Dictionary<int, GameObject> _all_gameObjects = new Dictionary<int, GameObject>();
        private List<Object> _destroy_objects = new List<Object>();
        private List<GameObject> _root = new List<GameObject>();

        public string name
        {
            get;
            private set;
        }

        public Scene(string name)
        {
            this.name = name;
        }

        internal void AddGameObject(GameObject obj)
        {
            if (obj != null)
            {
                var id = obj.GetInstanceID();
                if (!this._all_gameObjects.ContainsKey(id))
                {
                    this._all_gameObjects.Add(id, obj);
                }
                this.AddRootGameObject(obj);
            }
        }

        internal void AddRootGameObject(GameObject obj)
        {
            if (obj.transform.parent == null)
                this._root.Add(obj);
        }

        internal void SetRootIndex(GameObject obj,int index)
        {
            if (this._root.Remove(obj))
            {
                if (index >= 0 && index <= this._root.Count)
                {
                    this._root.Insert(index, obj);
                }
                else
                {
                    this._root.Add(obj);
                }
            }
        }

        internal int GetRootIndex(GameObject obj)
        {
            return this._root.IndexOf(obj);
        }

        internal void RemoveRootGameObject(GameObject obj)
        {
            this._root.Remove(obj);
        }

        internal void DestroyObject(Object obj)
        {
            this._destroy_objects.Add(obj);
        }

        internal void DestroyGameObjectImmediate(GameObject obj)
        {
            if (obj != null)
            {
                var id = obj.GetInstanceID();
                if (this._all_gameObjects.ContainsKey(id))
                    this._all_gameObjects.Remove(id);
                this._root.Remove(obj);
            }
        }

        private void DestroyObjects()
        {
            foreach (var obj in this._destroy_objects)
            {
                if (obj != null)
                {
                    Object.__DestroyImmediate__(obj);
                    if (obj is GameObject)
                        _root.Remove(obj as GameObject);
                }
            }
            this._destroy_objects.Clear();
        }


        public void OnEnter()
        {
        }

        public void OnExit()
        {

        }

        private void StartComponent()
        {
            foreach (var obj in this._all_gameObjects.Values)
            {
                if (obj.activeSelf)
                    obj.SendMessage(UnityMessage.Start);
            }
        }

        private void Renderer()
        {

        }

        private void BroadcastMessage(string methodName)
        {
            foreach (var obj in this._root)
            {
                if (obj != null)
                {
                    obj.BroadcastMessage(methodName);
                }
            }
        }

        internal void FrameLoop()
        {

            if (Time.inFixedTimeStep)
            {
                this.BroadcastMessage(UnityMessage.FixedUpdate);
            }

            /*
             * TODO:UnityEngine 其他系统Udpate
             */

            this.BroadcastMessage(UnityMessage.Update);

            /*
             * TODO:UnityEngine 其他系统Udpate
             */
             //协程更新
            CoroutineManager.Instance.Update();

            this.StartComponent();

            this.BroadcastMessage(UnityMessage.LateUpdate);

            //销毁需要销毁的GameObject
            this.DestroyObjects();

            //渲染
            this.Renderer();

            this.BroadcastMessage(UnityMessage.OnGui);
        }

        public void PrintHierarchy()
        {
            Debug.Log("================================================");
            Debug.Log(this.name + "[Scene]");
            foreach (var obj in this._root)
            {
                if (obj != null)
                {
                    this.PrintGameObject(obj, 0);
                }
            }
            Debug.Log("================================================");
        }

        private void PrintGameObject(GameObject obj,int depth)
        {
            var str = new System.Text.StringBuilder();
            str.Append(' ', depth * 2);
            str.Append(obj.name);
            var comps = obj.GetAllComponents();
            if (comps.Count > 0)
            {
                str.Append("[");
                for (var i = 0; i < comps.Count; ++i)
                {
                    if (i != 0)
                        str.Append(',');
                    str.Append(comps[i].GetType().Name);
                }
                str.Append(']');
            }
            if (obj.activeInHierarchy)
            {
                Debug.Log(str);
            }
            else
            {
                Debug.Log(str, System.ConsoleColor.DarkGray);
            }

            var count = obj.transform.GetChildCount();
            for (int i = 0; i < count; ++i)
            {
                var child = obj.transform.GetChild(i);
                if (child != null)
                {
                    this.PrintGameObject(child.gameObject, depth + 1);
                }
            }
        }
    }
}

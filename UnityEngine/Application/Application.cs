// -----------------------------------------------------------------
// File:    Application
// Author:  ruanban
// Date:    1/4/2018 5:36:54 PM
// Description:
//      
// -----------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Application:Singleton<Application>
    {
        private enum AppState
        {
            Run,
            //Pause,
            Quit,
        }

        private AppState _state = AppState.Run;

        public bool IsRunning
        {
            get
            {
                return _state == AppState.Run;
            }
        }

        private bool IsQuit
        {
            get { return this._state == AppState.Quit; }
        }

        private bool showMenu;

        private void Init(bool showMenu,Action initAction)
        {
            this.showMenu = showMenu;
            Debug.Init();
            this.InitMenu();
            SceneManager.Instance.LoadScene("Main");
            if (initAction != null)
            {
                initAction();
            }
        }

        public void Run(bool showMenu = true,Action init = null)
        {
            try
            {
                this.Init(showMenu,init);
                new Time();
                while (!this.IsQuit)
                {
                    Time.BeginFrame();
                    var scene = SceneManager.Instance.currentScene;
                    if (scene != null)
                    {
                        scene.FrameLoop();
                    }
                    Time.EndFrame();

                    if (showMenu)
                        this.ShowMenu();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Debug.Log("Press Any Key To Exit!");
                Debug.WaitKey();
            }
        }

        private SortedDictionary<ConsoleKey,KeyValuePair<string,Action>> _menus = new SortedDictionary<ConsoleKey, KeyValuePair<string, Action>>();
        private void AddMenuItem(ConsoleKey key,string menu, Action action)
        {
            this._menus.Add(key, new KeyValuePair<string, Action>(menu,action));
        }

        private void ShowMenu()
        {
            Debug.Log(string.Format("RenderFrameCount {0} FrameCount {1}, Please Select:",Time.renderedFrameCount, Time.frameCount));
            
            foreach (var item in _menus)
            {
                Debug.Log(string.Format("[{0,-7}]\t{1}", item.Key, item.Value.Key));
            }

            var key = Debug.WaitKey();
            while (!_menus.ContainsKey(key.Key))
            {
                Debug.Log("Invalid,Input again:");
                key = Debug.WaitKey();
            }
            Debug.Log("");
            if (_menus[key.Key].Value != null)
                _menus[key.Key].Value();
        }

        public void Quit()
        {
            this._state = AppState.Quit;
        }

        public void PrintHierarchy()
        {
            SceneManager.Instance.currentScene.PrintHierarchy();
        }

        private void InitMenu()
        {
            this.AddMenuItem(ConsoleKey.Enter, "Continue.", null);
            this.AddMenuItem(ConsoleKey.H, "Print Hierarchy", this.PrintHierarchy);
            this.AddMenuItem(ConsoleKey.Escape, "Quit.", this.Quit);
            this.AddMenuItem(ConsoleKey.R, "Run", ()=> { this.showMenu = false;});
        }
    }
}

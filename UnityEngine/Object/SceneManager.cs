// -----------------------------------------------------------------
// File:    SceneManager
// Author:  ruanban
// Date:    1/4/2018 5:15:29 PM
// Description:
//      
// -----------------------------------------------------------------

using UnityEngine;

namespace UnityEngine
{
    public class SceneManager : Singleton<SceneManager> 
    {
        public Scene currentScene
        {
            get;
            private set;
        }

        public void UnloadScene()
        {
            if (currentScene != null)
            {
                this.currentScene.OnExit();
                this.currentScene = null;
            }
        }

        public void LoadScene(string name)
        {
            this.UnloadScene();
            this.currentScene = new Scene(name);
            this.currentScene.OnEnter();
        }
    }
}

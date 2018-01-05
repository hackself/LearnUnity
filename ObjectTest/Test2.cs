// -----------------------------------------------------------------
// File:    Test2
// Author:  ruanban
// Date:    1/5/2018 1:38:06 PM
// Description:
//      
// -----------------------------------------------------------------

using UnityEngine;
namespace ObjectTest
{
    public class Test_2 : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("Test_2 Awake");
        }

        void Start()
        {
            Debug.Log("Test_2 Start");
        }

        void OnEnable()
        {
            Debug.Log("Test_2 OnEnable");
        }

        void OnDisable()
        {
            Debug.Log("Test_2 OnDisable");
        }

        void OnDestroy()
        {
            Debug.Log("Test_2 OnDestroy");
        }
    }
}

// -----------------------------------------------------------------
// File:    Test1
// Author:  ruanban
// Date:    1/5/2018 1:37:58 PM
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace ObjectTest
{
    public class Test_1 : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("Test_1 Awake");
        }

        void Start()
        {
            Debug.Log("Test_1 Start");
        }

        void OnEnable()
        {
            Debug.Log("Test_1 OnEnable");
        }

        void OnDisable()
        {
            Debug.Log("Test_1 OnDisable");
        }

        void OnDestroy()
        {
            Debug.Log("Test_1 OnDestroy");
        }
    }
}

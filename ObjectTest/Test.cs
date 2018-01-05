// -----------------------------------------------------------------
// File:    Test
// Author:  ruanban
// Date:    1/5/2018 1:37:07 PM
// Description:
//      
// -----------------------------------------------------------------

using UnityEngine;
namespace ObjectTest
{
    class Test : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("Test Awake");
        }

        void Start()
        {
            Debug.Log("Test Start");
        }

        void OnEnable()
        {
            Debug.Log("Test OnEnable");
        }

        //void Update()
        //{
        //    Debug.Log("Update");
        //}

        //void FixedUpdate()
        //{
        //    Debug.Log("FixedUpdate");
        //}

        //void LateUpdate()
        //{
        //    Debug.Log("LateUpdate");
        //}

        //void OnGui()
        //{
        //    Debug.Log("OnGui");
        //}

        void OnDisable()
        {
            Debug.Log("Test OnDisable");
        }

        void OnDestroy()
        {
            Debug.Log("Test OnDestroy");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Test3 : Behaviour {
public class Test3 : MonoBehaviour
{
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TestMessage()
    {
        Debug.Log("Test3::TestMessage");
    }

    void TestMessageWithParam(int i)
    {
        Debug.Log("Test3::TestMessageWithParam(int i):" + i);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestAsyncFunc : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        testc();
    }

    async void testc()
    {
        List<Func<Task>> testFunc = new List<Func<Task>>();
        testFunc.Add(async () =>
        {
            Debug.Log("In");
            await Task.Delay(1000);
            Debug.Log("First delay");
            await Task.Delay(1000);
            Debug.Log("Second delay");
        });

        testFunc.Add(async () =>
        {
            Debug.Log("In");
            await Task.Delay(1000);
            Debug.Log("First delay");
            await Task.Delay(1000);
            Debug.Log("Second delay");
        });
        foreach (var item in testFunc)
        {
            await item();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

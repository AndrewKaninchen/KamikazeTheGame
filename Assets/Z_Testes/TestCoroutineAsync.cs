using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineAsync;

public class TestCoroutineAsync : MonoBehaviour {

	
	void Start ()
    {
        //StartCoroutine(Coroutine(15));
        CoroutineAsync();		
	}

    IEnumerator Coroutine(int count)
    {
        while (count > 0)
        {
            Debug.Log("Coroutine");
            yield return new WaitForSeconds(1f);
            count--;
        }
    }

    async void CoroutineAsync()
    {
        Debug.Log("Waiting For regular Coroutine");
        await this.StartCoroutineAsync(Coroutine(4));
        Debug.Log("Regular Coroutine is done.");

        var count = 15;
        while (count > 0)
        {
            Debug.Log("CoroutineAsync");
            await this.WaitForSecondsAsync(1f);
            count--;
        }        
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PopupExample : MonoBehaviour
{
    public GameObject buttonTemplate;


    public Task<int> InputInteger()
    {
        var completionSource = new TaskCompletionSource<int>();

        var b = Instantiate(buttonTemplate, transform).GetComponent<Button>();
        b.gameObject.SetActive(true);
        b.onClick.AddListener(
            () => { completionSource.SetResult(35); Destroy(b.gameObject); }
        );

        return completionSource.Task;
    }

    public async Task Test()
    {
        var i = await InputInteger();

        Debug.Log(i);
    }

    public async void TestEncaps()
    {
        await Test();
    }

    public void Start()
    {
        TestEncaps();
    }
}

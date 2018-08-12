using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace CoroutineAsync
{
    public static class CoroutineUtilities
    {
        public static Task<object> StartCoroutineAsync(this MonoBehaviour behaviour, IEnumerator coroutine)
        {
            IEnumerator CoroutineWrapper(IEnumerator c, TaskCompletionSource<object> cS)
            {
                yield return c;
                cS.SetResult(null);
            }

            var completionSource = new TaskCompletionSource<object>();
            behaviour.StartCoroutine(CoroutineWrapper(coroutine, completionSource));
            var task = completionSource.Task;
            return task;
        }

        //A ideia é isso aqui servir pra encapsular CoroutineAsync em Coroutine normal, mas não acho que isso vá ser útil ainda e tô com preguiça de testar se isso funciona
        //public static Coroutine StartCoroutine(this MonoBehaviour behaviour, Action coroutineAsync)
        //{
        //    IEnumerator Coroutine(Task t) { while (!t.IsCompleted) yield return new WaitForEndOfFrame(); }

        //    Task task = new Task(coroutineAsync);
        //    task.Start();
        //    var coroutine = behaviour.StartCoroutine(Coroutine(task));
        //    return coroutine;
        //}

        public static Task WaitForSecondsAsync(this MonoBehaviour behaviour, float seconds)
        {
            IEnumerator Coroutine() { yield return new WaitForSeconds(seconds); }
            return behaviour.StartCoroutineAsync(Coroutine());
        }

        public static Task WaitForEndOfFrameAsync(this MonoBehaviour behaviour)
        {
            IEnumerator Coroutine() { yield return new WaitForEndOfFrame(); }
            return behaviour.StartCoroutineAsync(Coroutine());
        }

        public static Task WaitForFixedUpdateAsync(this MonoBehaviour behaviour)
        {
            IEnumerator Coroutine() { yield return new WaitForFixedUpdate(); }
            return behaviour.StartCoroutineAsync(Coroutine());
        }        
    }
}
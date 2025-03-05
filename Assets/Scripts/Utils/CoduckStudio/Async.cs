using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoduckStudio.Utils
{
    public class Async : Singleton<Async>
    {
        public void WaitForEndOfFrame(System.Action callback)
        {
            StartCoroutine(WaitForEndOfFrameCoroutine(callback));
        }

        private IEnumerator WaitForEndOfFrameCoroutine(System.Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }

        public void WaitForSeconds(float seconds, System.Action callback)
        {
            StartCoroutine(WaitForSecondsCoroutine(seconds, callback));
        }

        private IEnumerator WaitForSecondsCoroutine(float seconds, System.Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}

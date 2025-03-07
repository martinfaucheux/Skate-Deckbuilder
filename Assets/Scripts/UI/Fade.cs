using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CoduckStudio
{
    public class Fade : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public float duration = 2;
        public bool showGameOnStart = true;

        private void Start() {
            if (showGameOnStart) {
                ShowGame();
            }
        }

        public void ShowGame(bool instant = false, Action callback = null)
        {
            DOTween.Kill(canvasGroup);
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, duration).OnComplete(() => callback?.Invoke());

        }

        public void HideGame(bool instant = false, Action callback = null)
        {
            DOTween.Kill(canvasGroup);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, duration).OnComplete(() => callback?.Invoke());
        }
    }
}

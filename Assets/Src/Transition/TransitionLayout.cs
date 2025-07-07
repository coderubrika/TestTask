using System;
using DG.Tweening;
using Suburb.Utils;
using UniRx;
using UnityEngine;

namespace TestTask.Transition
{
    public class TransitionLayout : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Ease ease;
        [SerializeField] private float duration;

        public void SetFadeIn()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        public void SetFadeOut()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
        
        public IObservable<Unit> PlayFadeIn()
        {
            return canvasGroup.DOFade(1, duration)
                .SetEase(ease)
                .ToObservableOnKill()
                .ObserveOnMainThread()
                .Do(_ => SetFadeIn());
        }
        
        public IObservable<Unit> PlayFadeOut()
        {
            return canvasGroup.DOFade(0, duration)
                .SetEase(ease)
                .ToObservableOnKill()
                .ObserveOnMainThread()
                .Do(_ => SetFadeOut());
        }
    }
}
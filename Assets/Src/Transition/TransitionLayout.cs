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

        private Tween tween;
        
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
            DOTween.Kill(tween);
            
            tween = canvasGroup.DOFade(1, duration).SetEase(ease);
            
            return tween
                .ToObservableOnKill()
                .ObserveOnMainThread()
                .Do(_ => SetFadeIn());
        }
        
        public IObservable<Unit> PlayFadeOut()
        {
            DOTween.Kill(tween);
            tween = canvasGroup.DOFade(0, duration).SetEase(ease);
            return tween
                .ToObservableOnKill()
                .ObserveOnMainThread()
                .Do(_ => SetFadeOut());
        }
    }
}
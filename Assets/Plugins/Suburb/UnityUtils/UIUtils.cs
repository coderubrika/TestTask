﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Suburb.Utils.Serialization;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Suburb.Utils
{
    public static class UIUtils
    {
        public static IDisposable UpdateContent(RectTransform content)
        {
            IDisposable disposable = null;
            disposable = Observable.NextFrame()
                .Subscribe(_ =>
                {
                    disposable.Dispose();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                });

            return disposable;
        }

        public static IDisposable UpdateContents(IEnumerable<RectTransform> contents)
        {
            IDisposable disposable = null;

            disposable = Observable.NextFrame()
                .Subscribe(_ =>
                {
                    disposable.Dispose();
                    foreach (var content in contents)
                        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                });

            return disposable;
        }

        public static Color GetNewAlpha(Color source, float alpha)
        {
            return new Color(source.r, source.g, source.b, alpha);
        }

        public static Tween FadeCanvas(CanvasGroup canvasGroup, ValueStartEndAnimationData<float> config)
        {
            canvasGroup.alpha = config.Start;
            return canvasGroup.DOFade(config.End, config.AnimationSettings.Duration)
                .SetEase(config.AnimationSettings.Easing)
                .OnKill(() => canvasGroup.alpha = config.End);
        }
        
        public static float GetDurationForPercentage0(float current, float duration)
        {
            return current * duration;
        }

        public static float GetDurationForPercentage1(float current, float duration)
        {
            return (1 - current) * duration;
        }
        
        public static IEnumerator Rebuild(this LayoutGroup layoutGroup)
        {
            yield return new WaitForEndOfFrame();
            layoutGroup.enabled = false;
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.transform as RectTransform);
            layoutGroup.enabled = true;
        }
        
        public static Vector3 TransformCoords(RectTransform rectTransform, Camera camera, Vector2 screenPoint)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out Vector2 localPoint);
            Vector2 pivot = rectTransform.pivot;
            Vector2 size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            return camera.ViewportToWorldPoint(new Vector3(localPoint.x / size.x + pivot.x, localPoint.y / size.y + pivot.y));
        }
    }
}

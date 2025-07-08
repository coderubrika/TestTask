using DG.Tweening;
using Suburb.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.UI
{
    public class WaveUpAnim
    {
        private readonly RectTransform rectTransform;
        private readonly Graphic graphic;
        private readonly float duration;
        private readonly float waveHeight;
        private readonly float waveWidth;
        private readonly Vector2 startPosition;

        public bool IsPlaying { get; private set; }
        public TMP_Text TMPText { get; }
        public WaveUpAnim(
            TMP_Text tmpText,
            Vector3 startAnchoredPosition,
            float duration = 0.5f,
            float waveHeight = 100f,
            float waveWidth = 50f)
        {
            TMPText = tmpText;
            rectTransform = tmpText.rectTransform;
            graphic = tmpText;
            this.duration = duration;
            this.waveHeight = waveHeight;
            this.waveWidth = waveWidth;
            startPosition = startAnchoredPosition;
        }

        public void Play()
        {
            DOTween.Kill(rectTransform);
            DOTween.Kill(graphic);
            IsPlaying = true;
            rectTransform.DOAnchorPosY(startPosition.y + waveHeight, duration)
                .SetEase(Ease.OutSine)
                .OnUpdate(() =>
                {
                    float progress = (rectTransform.anchoredPosition.y - startPosition.y) / waveHeight;
                    float waveOffset = Mathf.Sin(progress * Mathf.PI * 2) * waveWidth;
                    rectTransform.anchoredPosition = new Vector2(
                        startPosition.x + waveOffset,
                        rectTransform.anchoredPosition.y
                    );
                })
                .OnKill(() => IsPlaying = false);
            
            graphic.DOFade(0, duration)
                .SetEase(Ease.Linear);
        }

        public void Clear()
        {
            DOTween.Kill(rectTransform);
            DOTween.Kill(graphic);
            rectTransform.localScale = Vector3.one;
            rectTransform.rotation = Quaternion.identity;
            graphic.color = UIUtils.GetNewAlpha(graphic.color, 1);
        }
    }
}
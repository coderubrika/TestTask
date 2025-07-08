using DG.Tweening;
using UnityEngine;

namespace TestTask.UI
{
    public class RectTransformAnim
    {
        private readonly RectTransform rectTransform;
        private readonly Vector3 originalPosition;
        private readonly Quaternion originalRotation;
        private readonly Vector3 originalScale;
        private readonly float jumpHeight;
        private readonly float rotationAngle;

        public RectTransformAnim(
            RectTransform rectTransform,
            Vector3 originalPosition,
            Quaternion originalRotation,
            Vector3 originalScale,
            float jumpHeight,
            float rotationAngle)
        {
            this.rectTransform = rectTransform;
            this.originalPosition = originalPosition;
            this.originalRotation = originalRotation;
            this.originalScale = originalScale;
            this.jumpHeight = jumpHeight;
            this.rotationAngle = rotationAngle;
        }
        
        public void PlayAnimation()
        {
            DOTween.Kill(rectTransform);
            
            rectTransform.DOAnchorPosY(originalPosition.y + jumpHeight, 0.25f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    rectTransform.DOAnchorPosY(originalPosition.y, 0.25f)
                        .SetEase(Ease.OutBounce);
                });
            
            rectTransform.DORotate(new Vector3(0, 0, rotationAngle), 0.15f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => 
                {
                    rectTransform.DORotate(originalRotation.eulerAngles, 0.15f);
                });
            
            rectTransform.DOScale(originalScale * 0.9f, 0.1f)
                .OnComplete(() =>
                {
                    rectTransform.DOScale(originalScale, 0.4f)
                        .SetEase(Ease.OutElastic);
                });
        }

        public void Clear()
        {
            DOTween.Kill(rectTransform);
            rectTransform.anchoredPosition = originalPosition;
            rectTransform.rotation = originalRotation;
            rectTransform.localScale = originalScale;
        }
    }
}
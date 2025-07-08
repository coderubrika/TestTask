using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Dogs
{
    public class DogInfoLayout : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text dogName;
        [SerializeField] private GameObject hypoallergenicYes;
        [SerializeField] private GameObject hypoallergenicNo;
        [SerializeField] private TMP_Text femaleValue;
        [SerializeField] private TMP_Text maleValue;
        [SerializeField] private LayoutGroup layoutGroup;

        public LayoutGroup LayoutGroup => layoutGroup;
        
        public ReactiveCommand OnCloseFinished { get; } = new();
        
        public Button CloseButton => closeButton;
        
        private Tween tween;

        public void SetData(BreedData data)
        {
            description.text = data.Attributes.Description;
            dogName.text = data.Attributes.Name;
            hypoallergenicYes.SetActive(data.Attributes.Hypoallergenic);
            hypoallergenicNo.SetActive(!data.Attributes.Hypoallergenic);
            femaleValue.text = $"{data.Attributes.FemaleWeight.Min} - {data.Attributes.FemaleWeight.Max}";
            maleValue.text = $"{data.Attributes.MaleWeight.Min} - {data.Attributes.MaleWeight.Max}";
        }
        
        public void SetFadeOut()
        {
            tween?.Kill();
            tween = null;
            canvasGroup.alpha = 0;
            OnCloseFinished.Execute();
        }
        
        public void SetFadeIn()
        {
            tween?.Kill();
            tween = null;
            canvasGroup.alpha = 1;
        }

        public void FadeIn()
        {
            tween = canvasGroup
                .DOFade(1, duration)
                .SetEase(ease)
                .OnComplete(SetFadeIn);
        }
        
        public void FadeOut()
        {
            tween = canvasGroup
                .DOFade(0, duration)
                .SetEase(ease)
                .OnComplete(SetFadeOut);
        }
    }
}
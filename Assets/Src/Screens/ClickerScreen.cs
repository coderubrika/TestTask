using Suburb.Screens;
using Suburb.Utils;
using TestTask.Clicker;
using TestTask.FX;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestTask.Screens
{
    public class ClickerScreen : BaseScreen
    {
        private ClickerService clickerService;
        private FXService fxService;
        
        [SerializeField] private TMP_Text money;
        [SerializeField] private TMP_Text energy;
        [SerializeField] private Button tapButton;
        [SerializeField] private RectTransform fxRectTransform;
        
        private readonly CompositeDisposable disposables = new();

        [Inject]
        private void Construct(
            ClickerService clickerService,
            FXService fxService)
        {
            this.fxService = fxService;
            this.clickerService = clickerService;
            clickerService.Enable();
        }
        
        protected override void Show()
        {
            fxService.Enable();
            tapButton.OnPointerClickAsObservable()
                .Subscribe(data =>
                {
                    Vector3 position = TransformCoords(fxRectTransform, fxService.Camera, data.position);
                    clickerService.Click(position);
                })
                .AddTo(disposables);

            Observable.EveryUpdate()
                .Subscribe(_ => UpdateValues())
                .AddTo(disposables);
            
            clickerService.OnClick
                .Subscribe(position =>
                {
                    fxService.EmitParticle(position);
                })
                .AddTo(disposables);
            
            base.Show();
        }

        public static Vector3 TransformCoords(RectTransform rectTransform, Camera camera, Vector2 screenPoint)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out Vector2 localPoint);
            Vector2 pivot = rectTransform.pivot;
            Vector2 size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            return camera.ViewportToWorldPoint(new Vector3(localPoint.x / size.x + pivot.x, localPoint.y / size.y + pivot.y));
        }
        
        private void UpdateValues()
        {
            money.text = clickerService.Money.ToString();
            energy.text = $"{clickerService.Energy}/{clickerService.MaxEnergy}";
        }
        
        protected override void Hide()
        {
            fxService.Disable();
            base.Hide();
            disposables.Clear();
        }
    }
}
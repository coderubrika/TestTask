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
                    Vector3 position = UIUtils.TransformCoords(fxRectTransform, fxService.Camera, data.position);
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
                    fxService.PlayAudio();
                })
                .AddTo(disposables);
            
            base.Show();
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
using Suburb.Screens;
using Suburb.Utils;
using TestTask.Clicker;
using TestTask.FX;
using TestTask.UI;
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

        private RectTransformAnim buttonAnimation;
        
        [Inject]
        private void Construct(
            ClickerService clickerService,
            FXService fxService)
        {
            this.fxService = fxService;
            this.clickerService = clickerService;
            clickerService.Enable();
            buttonAnimation = new(
                tapButton.transform as RectTransform,
                Vector3.zero,
                Quaternion.identity,
                Vector3.one,
                10f, 10f);
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
                    buttonAnimation.PlayAnimation();
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
            buttonAnimation.Clear();
            fxService.Disable();
            base.Hide();
            disposables.Clear();
        }
    }
}
using Suburb.Screens;
using TestTask.Clicker;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestTask.Screens
{
    public class ClickerScreen : BaseScreen
    {
        private ClickerService clickerService;
        
        [SerializeField] private TMP_Text money;
        [SerializeField] private TMP_Text energy;
        [SerializeField] private Button tapButton;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        private void Construct(ClickerService clickerService)
        {
            this.clickerService = clickerService;
            clickerService.Enable();
        }
        
        protected override void Show()
        {
            tapButton.OnClickAsObservable()
                .Subscribe(_ => clickerService.Click())
                .AddTo(disposables);

            Observable.EveryUpdate()
                .Subscribe(_ => UpdateValues())
                .AddTo(disposables);

            clickerService.OnClick
                .Subscribe(_ =>
                {
                    // анимации и эффекты
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
            base.Hide();
            disposables.Clear();
        }
    }
}
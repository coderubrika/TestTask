using System.Linq;
using Suburb.Screens;
using TestTask.Weather;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace TestTask.Screens
{
    public class WeatherScreen : BaseScreen
    {
        private WeatherService weatherService;
        
        [SerializeField] private TMP_Text temperatureLabel;
        private readonly CompositeDisposable disposables = new();
        
        [Inject]
        private void Construct(WeatherService weatherService)
        {
            this.weatherService = weatherService;
        }
        
        protected override void Show()
        {
            temperatureLabel.text = string.Empty;
            
            weatherService.OnUpdate
                .Subscribe(data =>
                {
                    int temperature = data.Properties.Periods.First().Temperature;
                    temperatureLabel.text = $"Сегодня {temperature}F";
                })
                .AddTo(disposables);
                
            weatherService.Enable();
            base.Show();
        }

        protected override void Hide()
        {
            base.Hide();
            weatherService.Disable();
            disposables.Clear();
        }
    }
}
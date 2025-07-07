using System;
using TestTask.RestClientQueue;
using UniRx;

namespace TestTask.Weather
{
    public class WeatherService
    {
        private readonly RestClientService restClientService;
        
        private IDisposable requestDisposable;
        
        private readonly CompositeDisposable disposables = new();
        
        private readonly string address = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public ReactiveCommand<WeatherApiResponse> OnUpdate { get; } = new();
        
        public WeatherService(RestClientService restClientService)
        {
            this.restClientService = restClientService;
        }

        public void Enable()
        {
            RequestWeather();
            Observable.Interval(TimeSpan.FromSeconds(5))
                .Subscribe(_ => RequestWeather())
                .AddTo(disposables);
        }

        private void RequestWeather()
        {
            requestDisposable?.Dispose();
            requestDisposable = restClientService.Get<WeatherApiResponse>(
                    address,
                    response => OnUpdate.Execute(response),
                    null)
                .AddTo(disposables);
        }
        
        public void Disable()
        {
            disposables.Clear();
        }
    }
}
using System;
using UniRx;
using UnityEngine;

namespace TestTask.Clicker
{
    public class ClickerService
    {
        private readonly CompositeDisposable disposables = new();
        
        public int Energy { get; private set; }
        public int MaxEnergy { get; private set; }
        public int Money { get; private set; }
        public int Transit { get; private set; }
        public int EnergyUpInterval { get; private set; }
        public int EnergyUpTransit { get; private set; }
        public int AutoClickInterval { get; private set; }

        public ReactiveCommand<Vector3> OnClick { get; } = new();
        
        public ClickerService(SettingsRepository settingsRepository)
        {
            MaxEnergy = settingsRepository.StartMaxEnergy;
            Energy = Mathf.Clamp(settingsRepository.StartEnergy, 0, MaxEnergy);
            Money = 0;
            Transit = settingsRepository.StartTransit;
            EnergyUpTransit = settingsRepository.StartEnergyUpTransit;
            EnergyUpInterval = settingsRepository.StartEnergyUpInterval;
            AutoClickInterval = settingsRepository.StartAutoClickInterval;
        }
        
        public void Click(Vector3 position)
        {
            if (Energy == 0)
                return;
            
            Money += Transit;
            Energy -= Transit;

            OnClick.Execute(position);
        }

        public void Enable()
        {
            Observable.Interval(TimeSpan.FromSeconds(AutoClickInterval))
                .Subscribe(_ => Click(Vector3.zero))
                .AddTo(disposables);
            
            Observable.Interval(TimeSpan.FromSeconds(EnergyUpInterval))
                .Subscribe(_ => Energy = Mathf.Clamp(Energy + EnergyUpTransit, 0, MaxEnergy))
                .AddTo(disposables);
        }

        public void Disable()
        {
            disposables.Clear();
        }
    }
}
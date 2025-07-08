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

        public ReactiveCommand<Vector3> OnClick { get; } = new();
        
        public ClickerService()
        {
            MaxEnergy = 1000;
            Energy = Mathf.Clamp(1000, 0, MaxEnergy);
            Money = 0;
        }
        
        public void Click(Vector3 position)
        {
            if (Energy == 0)
                return;
            
            Money += 1;
            Energy -= 1;

            OnClick.Execute(position);
        }

        public void Enable()
        {
            Observable.Interval(TimeSpan.FromSeconds(3))
                .Subscribe(_ => Click(Vector3.zero))
                .AddTo(disposables);
            
            Observable.Interval(TimeSpan.FromSeconds(10))
                .Subscribe(_ => Energy = Mathf.Clamp(Energy + 10, 0, MaxEnergy))
                .AddTo(disposables);
        }

        public void Disable()
        {
            disposables.Clear();
        }
    }
}
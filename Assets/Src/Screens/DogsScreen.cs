using System.Collections.Generic;
using Suburb.Screens;
using Suburb.Utils;
using TestTask.Dogs;
using TestTask.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestTask.Screens
{
    public class DogsScreen : BaseScreen
    {
        private DogListItem.Pool pool;
        private DogsService dogsService;

        private readonly CompositeDisposable disposables = new();
        private readonly List<DogListItem> items = new();
        
        [SerializeField] private GameObject loading;
        [SerializeField] private ScrollRect scrollRect;
        
        [Inject]
        private void Construct(
            DogsService dogsService,
            DogListItem.Pool pool)
        {
            this.dogsService = dogsService;
            this.pool = pool;
        }

        protected override void Show()
        {
            dogsService.IsLoading
                .Subscribe(isOn => loading.SetActive(isOn))
                .AddTo(disposables);
            
            dogsService.GetDogs()
                .Subscribe(FillDogs)
                .AddTo(disposables);
            base.Show();
        }

        private void FillDogs(BreedData[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                var dataItem = datas[i];
                var dogListItem = pool.Spawn(dataItem, i+1);
                items.Add(dogListItem);
                dogListItem.transform.SetParent(scrollRect.content);
                dogListItem.transform.localScale = Vector3.one;

                dogListItem.OpenDogInfoButton
                    .OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        dogListItem.Loading.SetActive(true);
                        dogsService.Clear();
                        dogsService.ShowFullInfo(dataItem.Id)
                            .Subscribe(_ => dogListItem.Loading.SetActive(false))
                            .AddTo(disposables);
                    })
                    .AddTo(disposables);
            }
        }
        
        protected override void Hide()
        {
            base.Hide();

            foreach (var item in items)
                pool.Despawn(item);
            
            items.Clear();
            
            dogsService.Clear();
            disposables.Clear();
        }
    }
}
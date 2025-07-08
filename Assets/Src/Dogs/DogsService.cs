using System;
using Suburb.Utils;
using TestTask.RestClientQueue;
using UniRx;
using Zenject;

namespace TestTask.Dogs
{
    public class DogsService : IInitializable, IDisposable
    {
        private readonly RestClientService restClientService;
        private readonly DogInfoLayout dogInfoLayoutPrefab;
        private readonly InjectCreator injectCreator;
        private readonly LayoutsRoot layoutsRoot;
        
        private DogInfoLayout dogInfoLayout;
        
        private BreedData[] datas;
        private Subject<DogBreedsResponse> dogsSubject;
        private IDisposable dogsDisposable;
        private IDisposable dogDisposable;
        private Subject<DogBreedResponse> dogSubject;
        
        private readonly CompositeDisposable disposables = new();
        
        private readonly string address = "https://dogapi.dog/api/v2/breeds/";

        public ReactiveProperty<bool> IsLoading { get; } = new();
        
        public DogsService(
            RestClientService restClientService,
            InjectCreator injectCreator,
            LayoutsRoot layoutsRoot,
            DogInfoLayout dogInfoLayoutPrefab)
        {
            this.restClientService = restClientService;
            this.layoutsRoot = layoutsRoot;
            this.injectCreator = injectCreator;
            this.dogInfoLayoutPrefab = dogInfoLayoutPrefab;
        }
        
        public IObservable<BreedData[]> GetDogs()
        {
            if (datas != null)
                return Observable.Return(datas);

            IsLoading.Value = true;
            
            if (dogsSubject != null)
                return dogsSubject.Select(data => data.Data);

            dogsSubject = new Subject<DogBreedsResponse>();
            
            dogsDisposable = restClientService.Get<DogBreedsResponse>(
                address,
                data =>
                {
                    dogsSubject.OnNext(data);
                    dogsSubject.OnCompleted();
                }, exception => dogsSubject.OnError(exception));
            
            return dogsSubject.Select(data => data.Data)
                .ObserveOnMainThread()
                .Do(data =>
                {
                    datas = data;
                    IsLoading.Value = false;
                });
        }

        private IObservable<DogBreedResponse> GetDog(string id)
        {
            IsLoading.Value = true;
            dogSubject?.Dispose();
            dogSubject = new();
            dogDisposable?.Dispose();
            dogDisposable = restClientService.Get<DogBreedResponse>(
                address + id,
                data =>
                {
                    IsLoading.Value = false;
                    dogSubject.OnNext(data);
                }, 
                dogSubject.OnError);
            
            return dogSubject;
        }
        
        public IObservable<Unit> ShowFullInfo(string id)
        {
            dogInfoLayout.SetFadeOut();

            return GetDog(id)
                .ObserveOnMainThread()
                .SelectMany(data =>
                {
                    dogInfoLayout.SetData(data.Data);
                    dogInfoLayout.gameObject.SetActive(true);
                    return dogInfoLayout.LayoutGroup.Rebuild().ToObservable();
                })
                .ObserveOnMainThread()
                .Do(_ => dogInfoLayout.FadeIn());
        }
        
        public void Clear()
        {
            dogSubject?.Dispose();
            dogDisposable?.Dispose();
            dogsDisposable?.Dispose();
            dogsSubject?.Dispose();
            dogsSubject = null;
            dogInfoLayout.SetFadeOut();
            IsLoading.Value = false;
        }

        public void Dispose()
        {
            Clear();
            disposables.Dispose();
        }

        public void Initialize()
        {
            dogInfoLayout = injectCreator.Create(dogInfoLayoutPrefab, layoutsRoot.Root);
            dogInfoLayout.gameObject.SetActive(false);

            dogInfoLayout.CloseButton
                .OnClickAsObservable()
                .Subscribe(_ => dogInfoLayout.FadeOut())
                .AddTo(disposables);

            dogInfoLayout.OnCloseFinished
                .Subscribe(_ =>
                {
                    if (dogInfoLayout != null)
                        dogInfoLayout.gameObject.SetActive(false);
                })
                .AddTo(disposables);
        }
    }
}
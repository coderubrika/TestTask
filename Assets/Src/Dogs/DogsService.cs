using System;
using TestTask.RestClientQueue;
using UniRx;

namespace TestTask.Dogs
{
    public class DogsService : IDisposable
    {
        private readonly RestClientService restClientService;
        
        private BreedData[] datas;
        private Subject<DogBreedResponse> dogsSubject;
        private IDisposable dogsDisposable;
        
        private readonly string address = "https://dogapi.dog/api/v2/breeds/";
        
        public DogsService(RestClientService restClientService)
        {
            this.restClientService = restClientService;
        }
        
        public IObservable<BreedData[]> GetDogs()
        {
            if (datas != null)
                return Observable.Return(datas);

            if (dogsSubject != null)
                return dogsSubject.Select(data => data.Data);

            dogsSubject = new Subject<DogBreedResponse>();
            
            dogsDisposable = restClientService.Get<DogBreedResponse>(
                address,
                data =>
                {
                    dogsSubject.OnNext(data);
                    dogsSubject.OnCompleted();
                }, exception => dogsSubject.OnError(exception));
            
            return dogsSubject.Select(data => data.Data);
        }

        public void Dispose()
        {
            dogsDisposable?.Dispose();
            dogsSubject.Dispose();
            dogsSubject = null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace TestTask.RestClientQueue
{
    public class RestClientService : IDisposable
    {
        private readonly LinkedList<int> requestIndexes = new();
        private readonly Dictionary<int, Request> requests = new(); 
        private int requestIndex;
        
        public IDisposable Get<TResponse>(string url, Action<TResponse> callback, Action<Exception> onError)
        {
            int currentIndex = requestIndex++;
            var requestNode = requestIndexes.AddLast(currentIndex);
            
            Request request = new Request(
                UnityWebRequest.Get(url),
                Disposable.Create(() =>
                {
                    requestIndexes.Remove(requestNode);
                    requests.Remove(currentIndex);
                }),
                ProcessNextRequest,
                payload => callback.Invoke(JsonConvert.DeserializeObject<TResponse>(payload)),
                onError);
            
            requests.Add(currentIndex, request);
            ProcessNextRequest();
            return request;
        }

        private void ProcessNextRequest()
        {
            if (requests.Count == 0)
                return;

            var requestIndexNode = requestIndexes.First;
            int index = requestIndexNode.Value;
            Request request = requests[index];
            request.DisposeLinks();
            

            void HandleOperation(AsyncOperation operation)
            {
                if (request.UnityWebRequest.result == UnityWebRequest.Result.Success)
                    request.SetPayload(request.UnityWebRequest.downloadHandler.text);
                else
                {
                    request.SetError(new Exception($"Request failed: {request.UnityWebRequest.error}"));
                }
                
                request.DisposeOperations();
            }
            request.Send(HandleOperation);
        }
        
        public void Dispose()
        {
            foreach (var request in requests.Values.ToArray())
                request.Dispose();
            
            requests.Clear();
            requestIndexes.Clear();
            requestIndex = 0;
        }
    }
}
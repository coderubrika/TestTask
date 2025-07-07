using System;
using UnityEngine;
using UnityEngine.Networking;

namespace TestTask.RestClientQueue
{
    public class Request : IDisposable
    {
        private readonly Action<string> onSuccess;
        private readonly Action<Exception> onError;
        private readonly Action onDisposeOperation;
        
        private IDisposable disposableLinks;
        private Action<AsyncOperation> operationHandler;
        private UnityWebRequestAsyncOperation operation;
        
        public UnityWebRequest UnityWebRequest { get; }


        public Request(
            UnityWebRequest unityWebRequest, 
            IDisposable disposableLinks,
            Action onDisposeOperation,
            Action<string> onSuccess,
            Action<Exception> onError = null)
        {
            this.onDisposeOperation = onDisposeOperation;
            this.onSuccess = onSuccess;
            this.onError = onError;
            UnityWebRequest = unityWebRequest;
            this.disposableLinks = disposableLinks;
        }
        
        public void Dispose()
        {
            DisposeLinks();
            DisposeOperations();
            UnityWebRequest.Abort();
        }

        public void SetPayload(string payloadText)
        {
            onSuccess?.Invoke(payloadText);
        }

        public void SetError(Exception error)
        {
            onError?.Invoke(error);
        }
        
        public void DisposeLinks()
        {
            disposableLinks?.Dispose();
            disposableLinks = null;
        }

        public void DisposeOperations()
        {
            if (operation == null)
                return;
            
            operation.completed -= operationHandler;
            operation = null;
            onDisposeOperation?.Invoke();
        }
        
        public void Send(Action<AsyncOperation> operationHandler)
        {
            this.operationHandler = operationHandler;
            operation = UnityWebRequest.SendWebRequest();
            operation.completed += this.operationHandler;
        }
    }
}
using System;
using Suburb.Utils;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace TestTask.FX
{
    public class FXService : IInitializable, IDisposable
    {
        private readonly Camera cameraPrefab;
        private readonly ParticlePool particlePool;
        private readonly InjectCreator injectCreator;
        private readonly Store<ParticleSystem> particles = new();

        private readonly CompositeDisposable disposables = new();
        
        private bool isEnable;
        
        public Camera Camera { get; private set; }
        
        public FXService(
            Camera cameraPrefab,
            ParticlePool particlePool,
            InjectCreator injectCreator)
        {
            this.particlePool = particlePool;
            this.cameraPrefab = cameraPrefab;
            this.injectCreator = injectCreator;
        }

        public void Initialize()
        {
            Camera = injectCreator.Create(cameraPrefab, null);
            Object.DontDestroyOnLoad(Camera.gameObject);
        }

        public void EmitParticle(Vector3 position)
        {
            if (!isEnable)
                return;
            
            var particle = particlePool.Spawn();
            particle.transform.SetParent(Camera.transform);
            particle.transform.position = position;
            particle.Play();
            particles.Push(particle);
        }
        
        public void Enable()
        {
            if (isEnable)
                return;

            isEnable = true;
            
            Camera.gameObject.SetActive(true);
            
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    foreach (var particle in particles)
                    {
                        if (!particle.isStopped) 
                            continue;
                        
                        particlePool.Despawn(particle);
                        particles.Remove(particle);
                    }
                })
                .AddTo(disposables);
        }

        public void Disable()
        {
            if (!isEnable)
                return;

            isEnable = false;
            
            Camera.gameObject.SetActive(false);
            disposables.Clear();
            foreach (var particle in particles)
                particlePool.Despawn(particle);
            
            particles.Clear();
        }

        public void Dispose()
        {
            Disable();
            Object.Destroy(Camera.gameObject);
        }
    }
}
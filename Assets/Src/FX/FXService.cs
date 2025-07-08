using System;
using Suburb.Utils;
using TestTask.UI;
using TMPro;
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
        private readonly AudioClip clickClip;
        private readonly AudioSourcePool audioSourcePool;
        private readonly TMPTextPool tmpTextPool;
        
        private readonly CompositeDisposable disposables = new();
        private readonly Store<ParticleSystem> particles = new();
        private readonly Store<AudioSource> audioSources = new();
        private readonly Store<WaveUpAnim> textAnims = new();
        
        private bool isEnable;
        
        public Camera Camera { get; private set; }
        
        public FXService(
            Camera cameraPrefab,
            ParticlePool particlePool,
            InjectCreator injectCreator,
            AudioSourcePool audioSourcePool,
            AudioClip clickClip,
            TMPTextPool tmpTextPool)
        {
            this.audioSourcePool = audioSourcePool;
            this.clickClip = clickClip;
            this.particlePool = particlePool;
            this.cameraPrefab = cameraPrefab;
            this.injectCreator = injectCreator;
            this.tmpTextPool = tmpTextPool;
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

        public void PlayAudio()
        {
            if (!isEnable)
                return;
            
            var audioSource = audioSourcePool.Spawn();
            audioSource.transform.SetParent(Camera.transform);
            audioSource.clip = clickClip;
            audioSource.Play();
            audioSources.Push(audioSource);
        }

        public void EmitText(string text, RectTransform root, Vector2 screenPosition)
        {
            if (!isEnable)
                return;
            
            var tmpText = tmpTextPool.Spawn();
            tmpText.transform.SetParent(root);
            tmpText.text = text;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                root, screenPosition, null, out Vector2 localPoint);

            tmpText.rectTransform.localScale = Vector3.one;
            tmpText.rectTransform.rotation = Quaternion.identity;
            tmpText.rectTransform.anchoredPosition = localPoint;
            
            var anim = new WaveUpAnim(tmpText, localPoint, 1f, 300f, 25f);
            anim.Play();
            textAnims.Push(anim);
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
                    
                    foreach (var audioSource in audioSources)
                    {
                        if (audioSource.isPlaying) 
                            continue;
                        
                        audioSourcePool.Despawn(audioSource);
                        audioSources.Remove(audioSource);
                    }
                    
                    foreach (var textAnim in textAnims)
                    {
                        if (textAnim.IsPlaying) 
                            continue;
                        
                        textAnim.Clear();
                        tmpTextPool.Despawn(textAnim.TMPText);
                        textAnims.Remove(textAnim);
                    }
                })
                .AddTo(disposables);
        }

        public void Disable()
        {
            if (!isEnable)
                return;

            isEnable = false;
            
            if (Camera != null)
                Camera.gameObject.SetActive(false);
            
            disposables.Clear();
            foreach (var particle in particles)
            {
                particle.Stop();
                particlePool.Despawn(particle);
            }
            
            particles.Clear();
            
            foreach (var textAnim in textAnims)
            {
                textAnim.Clear();
                tmpTextPool.Despawn(textAnim.TMPText);
            }
            
            textAnims.Clear();
        }

        public void Dispose()
        {
            Disable();
            
            if (Camera != null)
                Object.Destroy(Camera.gameObject);
        }
    }
}
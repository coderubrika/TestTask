using Suburb.Screens;
using Suburb.Utils;
using TestTask.Clicker;
using TestTask.Dogs;
using TestTask.FX;
using TestTask.Navigation;
using TestTask.RestClientQueue;
using TestTask.Transition;
using TestTask.UI;
using TestTask.Weather;
using UnityEngine;
using Zenject;

namespace TestTask.MainScence
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private string screensRoot;
        [SerializeField] private NavigationLayout navigationLayoutPrefab;
        [SerializeField] private TransitionLayout transitionLayoutPrefab;
        [SerializeField] private DogListItem dogListItemPrefab;
        [SerializeField] private DogInfoLayout dogInfoLayoutPrefab;
        [SerializeField] private ParticleSystem particleSystemPrefab;
        [SerializeField] private AudioSource audioSourcePrefab;
        [SerializeField] private AudioClip clickClip;
        [SerializeField] private Camera fxCameraPrefab;
        
        private static string DOG_LIST_ITEM_GROUP = "DOG_LIST_ITEM_GROUP";
        private static string PARTICLE_SYSTEM_GROUP = "PARTICLE_SYSTEM_GROUP";
        private static string AUDIO_SOURCE_GROUP = "AUDIO_SOURCE_GROUP";
        
        public override void InstallBindings()
        {
            Container.Bind<InjectCreator>().AsSingle();
            Container.Bind<ClickerService>().AsSingle();
            Container.Bind<RestClientService>().AsSingle();
            Container.Bind<WeatherService>().AsSingle();
            
            Container.Bind<ScreensFactory>()
                .AsSingle()
                .WithArguments(screensRoot)
                .NonLazy();
            
            Container.Bind<ScreensService>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<LayoutsRoot>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<TransitionService>()
                .AsSingle()
                .WithArguments(transitionLayoutPrefab)
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<NavigationService>()
                .AsSingle()
                .WithArguments(navigationLayoutPrefab)
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<DogsService>()
                .AsSingle()
                .WithArguments(dogInfoLayoutPrefab)
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<FXService>()
                .AsSingle()
                .WithArguments(fxCameraPrefab, clickClip)
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<MainSceneStartup>()
                .AsSingle()
                .NonLazy();
            
            Container.BindMemoryPool<DogListItem, DogListItem.Pool>()
                .WithInitialSize(0)
                .WithMaxSize(10)
                .FromComponentInNewPrefab(dogListItemPrefab)
                .UnderTransformGroup(DOG_LIST_ITEM_GROUP);

            Container.BindMemoryPool<ParticleSystem, ParticlePool>()
                .WithInitialSize(0)
                .WithMaxSize(5)
                .FromComponentInNewPrefab(particleSystemPrefab)
                .UnderTransformGroup(PARTICLE_SYSTEM_GROUP);
            
            Container.BindMemoryPool<AudioSource, AudioSourcePool>()
                .WithInitialSize(0)
                .WithMaxSize(5)
                .FromComponentInNewPrefab(audioSourcePrefab)
                .UnderTransformGroup(AUDIO_SOURCE_GROUP);
        }
    }
}
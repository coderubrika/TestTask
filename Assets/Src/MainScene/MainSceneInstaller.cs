using Suburb.Screens;
using Suburb.Utils;
using TestTask.Clicker;
using TestTask.Navigation;
using TestTask.RestClientQueue;
using TestTask.Transition;
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
            
            Container.BindInterfacesAndSelfTo<MainSceneStartup>()
                .AsSingle()
                .NonLazy();
        }
    }
}
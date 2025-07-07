using Suburb.Screens;
using Suburb.Utils;
using TestTask.Navigation;
using TestTask.Transition;
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
using System;
using Suburb.ExpressRouter;
using Suburb.Screens;
using Suburb.Utils;
using TestTask.Screens;
using TestTask.Transition;
using UniRx;
using Zenject;

namespace TestTask.Navigation
{
    public class NavigationService : IInitializable, IDisposable
    {
        private readonly LayoutsRoot layoutsRoot;
        private readonly NavigationLayout navigationLayoutPrefab;
        private readonly InjectCreator injectCreator;
        private readonly ScreensService screensService;
        private readonly TransitionService transitionService;
        
        private NavigationLayout navigationLayout;

        private readonly CompositeDisposable disposables = new();
        
        public NavigationService(
            LayoutsRoot layoutsRoot,
            InjectCreator injectCreator,
            ScreensService screensService,
            TransitionService transitionService,
            NavigationLayout navigationLayoutPrefab)
        {
            this.layoutsRoot = layoutsRoot;
            this.injectCreator = injectCreator;
            this.screensService = screensService;
            this.transitionService = transitionService;
            this.navigationLayoutPrefab = navigationLayoutPrefab;
        }
        
        public void Initialize()
        {
            navigationLayout = injectCreator.Create(navigationLayoutPrefab, layoutsRoot.Root);
            SetupTransitions();
            SetupButtons();
        }
        
        public void Dispose()
        {
            disposables.Dispose();
        }

        private void SetupTransitions()
        {
            Rule fromAllToAll = new Rule(Selector.All(), Selector.All());

            ActItem<FromTo> actItemFrom = new ActItem<FromTo>((points, next) =>
            {
                transitionService.FadeIn()
                    .Subscribe(_ => next?.Invoke(points))
                    .AddTo(disposables);
            });
            
            ActItem<FromTo> actItemTo = new ActItem<FromTo>((points, next) =>
            {
                transitionService.FadeOut()
                    .Subscribe(_ => next?.Invoke(points))
                    .AddTo(disposables);
            });
            
            screensService.UseTransition(actItemFrom, fromAllToAll, MiddlewareOrder.From)
                .AddTo(disposables);
            
            screensService.UseTransition(actItemTo, fromAllToAll, MiddlewareOrder.To)
                .AddTo(disposables);
        }

        private void SetupButtons()
        {
            navigationLayout.ClickerButton
                .OnClickAsObservable()
                .Subscribe(_ => screensService.GoTo<ClickerScreen>())
                .AddTo(disposables);
            
            navigationLayout.WeatherButton
                .OnClickAsObservable()
                .Subscribe(_ => screensService.GoTo<WeatherScreen>())
                .AddTo(disposables);
            
            navigationLayout.DogsButton
                .OnClickAsObservable()
                .Subscribe(_ => screensService.GoTo<DogsScreen>())
                .AddTo(disposables);
        }
    }
}
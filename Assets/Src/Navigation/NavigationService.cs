using System;
using Suburb.ExpressRouter;
using Suburb.Screens;
using Suburb.Utils;
using TestTask.Screens;
using TestTask.Transition;
using UniRx;
using UnityEngine.UI;
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
        private Button currentButton;
        
        private readonly CompositeDisposable disposables = new();

        public Button ClickerButton => navigationLayout.ClickerButton;
        public Button WeatherButton => navigationLayout.WeatherButton;
        public Button DogsButton => navigationLayout.DogsButton;
        
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

        public void SetButton(Button button)
        {
            currentButton = button;
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
                navigationLayout.SetButton(null);
                transitionService.FadeIn()
                    .Subscribe(_ =>
                    {
                        navigationLayout.SetEnableButtons(false);
                        next?.Invoke(points);
                    })
                    .AddTo(disposables);
            });
            
            ActItem<FromTo> actItemTo = new ActItem<FromTo>((points, next) =>
            {
                transitionService.FadeOut()
                    .Subscribe(_ =>
                    {
                        next?.Invoke(points);
                        navigationLayout.SetButton(currentButton);
                        navigationLayout.SetEnableButtons(true);
                    })
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
                .Subscribe(_ =>
                {
                    currentButton = ClickerButton;
                    screensService.GoTo<ClickerScreen>();
                })
                .AddTo(disposables);
            
            navigationLayout.WeatherButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    currentButton = WeatherButton;
                    screensService.GoTo<WeatherScreen>();
                })
                .AddTo(disposables);
            
            navigationLayout.DogsButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    currentButton = DogsButton;
                    screensService.GoTo<DogsScreen>();
                })
                .AddTo(disposables);
        }
    }
}
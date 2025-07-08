using Suburb.Screens;
using TestTask.Navigation;
using TestTask.Screens;
using TestTask.Transition;
using UnityEngine;
using Zenject;

namespace TestTask.MainScence
{
    public class MainSceneStartup : IInitializable
    {
        private readonly ScreensService screensService;
        private readonly TransitionService transitionService;
        private readonly NavigationService navigationService;
        
        public MainSceneStartup(
            ScreensService screensService,
            TransitionService transitionService,
            NavigationService navigationService)
        {
            this.transitionService = transitionService;
            this.screensService = screensService;
            this.navigationService = navigationService;
        }
        
        public void Initialize()
        {
            transitionService.SetFadeIn();
            screensService.GoTo<ClickerScreen>();
            navigationService.SetButton(navigationService.ClickerButton);
            Application.targetFrameRate = 90;
        }
    }
}
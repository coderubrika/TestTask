using Suburb.Screens;
using TestTask.Navigation;
using TestTask.Screens;
using TestTask.Transition;
using Zenject;

namespace TestTask.MainScence
{
    public class MainSceneStartup : IInitializable
    {
        private readonly ScreensService screensService;
        private readonly TransitionService transitionService;
        
        public MainSceneStartup(
            ScreensService screensService,
            TransitionService transitionService)
        {
            this.transitionService = transitionService;
            this.screensService = screensService;
        }
        
        public void Initialize()
        {
            transitionService.SetFadeIn();
            screensService.GoTo<ClickerScreen>();
        }
    }
}
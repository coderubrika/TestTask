using System;
using Suburb.Utils;
using UniRx;
using Zenject;

namespace TestTask.Transition
{
    public class TransitionService : IInitializable
    {
        private readonly TransitionLayout transitionLayoutPrefab;
        private readonly LayoutsRoot layoutsRoot;
        private readonly InjectCreator injectCreator;
        
        private TransitionLayout transitionLayout;

        public TransitionService(
            InjectCreator injectCreator,
            LayoutsRoot layoutsRoot,
            TransitionLayout transitionLayoutPrefab)
        {
            this.layoutsRoot = layoutsRoot;
            this.injectCreator = injectCreator;
            this.transitionLayoutPrefab = transitionLayoutPrefab;
        }

        public void Initialize()
        {
            transitionLayout = injectCreator.Create(transitionLayoutPrefab, layoutsRoot.Root);
        }

        public void SetFadeIn()
        {
            transitionLayout.SetFadeIn();
        }

        public IObservable<Unit> FadeIn()
        {
            return transitionLayout.PlayFadeIn();
        }

        public IObservable<Unit> FadeOut()
        {
            return transitionLayout.PlayFadeOut();
        }
    }
}
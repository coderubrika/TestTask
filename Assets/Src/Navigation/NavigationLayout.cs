using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestTask.Navigation
{
    public class NavigationLayout : MonoBehaviour
    {
        [SerializeField] private Button clickerButton;
        [SerializeField] private Button weatherButton;
        [SerializeField] private Button dogsButton;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private Button[] buttons;
        
        public Button ClickerButton => clickerButton;

        public Button WeatherButton => weatherButton;

        public Button DogsButton => dogsButton;

        [Inject]
        private void Construct()
        {
            buttons = new[]
            {
                clickerButton,
                weatherButton,
                dogsButton
            };
        }
        
        public void SetButton(Button button)
        {
            foreach (var btn in buttons)
                btn.interactable = true;

            if (button != null)
                button.interactable = false;
        }

        public void SetEnableButtons(bool isOn)
        {
            canvasGroup.blocksRaycasts = isOn;
        }
    }
}
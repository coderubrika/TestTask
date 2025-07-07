using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Navigation
{
    public class NavigationLayout : MonoBehaviour
    {
        [SerializeField] private Button clickerButton;
        [SerializeField] private Button weatherButton;
        [SerializeField] private Button dogsButton;

        public Button ClickerButton => clickerButton;

        public Button WeatherButton => weatherButton;

        public Button DogsButton => dogsButton;
    }
}
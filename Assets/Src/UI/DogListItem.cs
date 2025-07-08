using TestTask.Dogs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestTask.UI
{
    public class DogListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text idx;
        [SerializeField] private TMP_Text dogName;
        [SerializeField] private Button openDogInfoButton;
        [SerializeField] private GameObject loading;
        
        public string Id { get; private set; }
        
        public Button OpenDogInfoButton => openDogInfoButton;
        public GameObject Loading => loading;
        
        public class Pool : MonoMemoryPool<BreedData, int, DogListItem>
        {
            protected override void Reinitialize(BreedData data, int index, DogListItem item)
            {
                item.Id = data.Id;
                item.loading.SetActive(false);
                item.idx.text = index.ToString();
                item.dogName.text = data.Attributes.Name;
            }
        }
    }
}
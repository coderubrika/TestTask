using UnityEngine;
using Zenject;

namespace TestTask
{
    [CreateAssetMenu(fileName = "RepositoryInstaller", menuName = "Installers/RepositoryInstaller")]
    public class RepositoryInstaller : ScriptableObjectInstaller<RepositoryInstaller>
    {
        [SerializeField] private SettingsRepository settingsRepository;
        
        public override void InstallBindings()
        {
            Container.BindInstance(settingsRepository);
        }
    }
}
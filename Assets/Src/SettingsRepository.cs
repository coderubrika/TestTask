using UnityEngine;

namespace TestTask
{
    [CreateAssetMenu(fileName = "SettingsRepository", menuName = "Repositories/SettingsRepository")]
    public class SettingsRepository : ScriptableObject
    {
        [SerializeField] private int startMaxEnergy;
        [SerializeField] private int startEnergy;
        [SerializeField] private int startTransit;
        [SerializeField] private int startEnergyUpInterval;
        [SerializeField] private int startEnergyUpTransit;
        [SerializeField] private int startAutoClickInterval;

        public int StartAutoClickInterval => startAutoClickInterval;
        public int StartMaxEnergy => startMaxEnergy;
        public int StartEnergy => startEnergy;
        public int StartTransit => startTransit;
        
        public int StartEnergyUpInterval => startEnergyUpInterval;

        public int StartEnergyUpTransit => startEnergyUpTransit;
    }
}
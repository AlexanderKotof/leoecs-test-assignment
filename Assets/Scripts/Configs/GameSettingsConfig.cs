using TestAsssignment.UI;
using UnityEngine;

namespace TestAsssignment.Configs
{
    [CreateAssetMenu(menuName = "Configs/Game Settings")]
    public class GameSettingsConfig : ScriptableObject
    {
        [SerializeField]
        private double _startBalance;

        [SerializeField]
        private BusinessConfig[] _businessConfigs;

        [SerializeField]
        private GameScreenComponent _gameScreenPrefab;

        public double StartBalance => _startBalance;
        public BusinessConfig[] BusinessConfigs => _businessConfigs;
        public GameScreenComponent GameScreenPrefab => _gameScreenPrefab;
    }
}
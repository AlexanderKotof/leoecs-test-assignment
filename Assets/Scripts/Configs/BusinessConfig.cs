using System;
using UnityEngine;

namespace TestAsssignment.Configs
{
    [CreateAssetMenu(menuName = "Configs/Business")]
    public class BusinessConfig : ScriptableObject
    {
        [SerializeField]
        private string _businessName;
        [SerializeField]
        private int _profitDelay;
        [SerializeField]
        private double _basePrice;
        [SerializeField]
        private double _baseProfit;

        [Serializable]
        public struct BusinessUpgrade
        {
            public string name;
            public double upgradePrice;
            public double profitMultiplier;
        }

        [SerializeField]
        private BusinessUpgrade[] _upgrades = new BusinessUpgrade[upgradesCount];

        [SerializeField]
        private bool _activeOnStart;

        public string BusinesName => _businessName;
        public int ProfitDelay => _profitDelay;
        public double BasePrice => _basePrice;
        public double BaseProfit => _baseProfit;

        public BusinessUpgrade[] Upgrades => _upgrades;

        public bool ActiveOnStart => _activeOnStart;

        public const int upgradesCount = 2;
    }
}
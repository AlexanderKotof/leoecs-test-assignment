using TestAsssignment.Utils;
using UnityEngine;
using UnityEngine.UI;
using static TestAsssignment.Systems.GameUISystem;

namespace TestAsssignment.UI
{
    public class BusinessItemComponent : MonoBehaviour
    {
        public TMPro.TMP_Text businessNameText;
        public TMPro.TMP_Text lvlValueText;
        public TMPro.TMP_Text profitValueText;
        public TMPro.TMP_Text lvlUpPriceValueText;

        public Slider profitPogressbar;

        public Button lvlUpButton;

        public UpgradeButtonComponent upgrade1Button;
        public UpgradeButtonComponent upgrade2Button;

        private BusinessData _data;

        public void SetInfo(BusinessData data)
        {
            _data = data;

            businessNameText.SetText(_data.config.BusinesName);
            profitPogressbar.value = 0;

            lvlUpButton.onClick.AddListener(() => _data.onLvlUpPressed.Invoke(_data));

            upgrade1Button.SetInfo(_data, 0);
            upgrade2Button.SetInfo(_data, 1);

            UpdateInfo();
        }

        public void UpdateInfo()
        {
            if (_data.IsActiveBusiness)
            {
                var activeBusiness = _data.GetActiveBusiness;
                SetInfo(activeBusiness.businessLevel.ToString(),
                    $"${GameMathUtils.GetBusinessLvlUpPrice(_data.config, activeBusiness.businessLevel)}",
                    $"${GameMathUtils.CalculateBusinessProfit(_data.config, activeBusiness.businessLevel, activeBusiness.purchasedUpgrades)}"
                    );

                UpdateProgress();

                upgrade1Button.UpdateInfo(_data, 0);
                upgrade2Button.UpdateInfo(_data, 1);
            }
            else
            {
                SetInfo("0",
                    $"${GameMathUtils.GetBusinessLvlUpPrice(_data.config, 0)}",
                    $"${_data.config.BaseProfit}"
                    );
            }
        }

        private void SetInfo(string level, string lvlUpPrice, string profit)
        {
            lvlValueText.SetText(level);
            profitValueText.SetText(profit);
            lvlUpPriceValueText.SetText(lvlUpPrice);
        }

        private void UpdateProgress()
        {
            var percent = _data.GetActiveBusiness.profitProgress / _data.config.ProfitDelay;
            profitPogressbar.value = percent;
        }
    }
}
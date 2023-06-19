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

        public BusinessData Data { get; private set; }

        public void SetInfo(BusinessData data)
        {
            Data = data;

            businessNameText.SetText(data.config.BusinesName);
            lvlValueText.SetText(data.lvlValue.ToString());

            profitValueText.SetText($"${GameMathUtils.CalculateBusinessProfit(data.config, data.lvlValue, data.purchasedUpgrades)}");
            lvlUpPriceValueText.SetText($"${GameMathUtils.GetBusinessLvlUpPrice(data.config, data.lvlValue)}");

            profitPogressbar.value = data.profitProgress / Data.config.ProfitDelay;

            lvlUpButton.onClick.AddListener(() => data.onLvlUpPressed.Invoke(data));

            upgrade1Button.SetInfo(data, 0);
            upgrade2Button.SetInfo(data, 1);
        }

        public void UpdateItem(float progress)
        {
            var percent = progress / Data.config.ProfitDelay;
            profitPogressbar.value = percent;
        }
    }
}
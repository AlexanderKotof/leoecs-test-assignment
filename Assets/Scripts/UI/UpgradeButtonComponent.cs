using UnityEngine;
using UnityEngine.UI;
using static TestAsssignment.Systems.GameUISystem;

public class UpgradeButtonComponent : MonoBehaviour
{
    public Button button;

    public TMPro.TMP_Text upgradeNameText;
    public TMPro.TMP_Text upgradeProfitValueText;
    public TMPro.TMP_Text upgradePriceValueText;

    public GameObject priceContainer;
    public GameObject upgradedContainer;

    public void SetInfo(BusinessData data, int upgradeIndex)
    {
        var upgrade = data.config.Upgrades[upgradeIndex];

        upgradeNameText.SetText(upgrade.name);
        upgradeProfitValueText.SetText($"+{upgrade.profitMultiplier * 100}%");
        upgradePriceValueText.SetText($"${upgrade.upgradePrice}");

        priceContainer.SetActive(!data.purchasedUpgrades[upgradeIndex]);
        upgradedContainer.SetActive(data.purchasedUpgrades[upgradeIndex]);

        button.onClick.AddListener(() => data.onPurchaseUpgradePressed.Invoke(data, upgradeIndex));
    }
}

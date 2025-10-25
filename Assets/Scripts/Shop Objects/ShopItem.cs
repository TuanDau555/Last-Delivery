using TMPro;
using UnityEngine;

public class ShopItem : BaseInteract
{
    [Space(10)]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private ShopItemSO _shopItemSO;
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        if (_shopItemSO == null)
        {
            Debug.LogWarning("No item in this slot");
            return;
        }

        if (!WorldManager.Instance.SpendMoney(_shopItemSO.price)) return;

        if (_shopItemSO.isCatItem)
            ApplyCatBuff(_shopItemSO);
        else
            ApplyPlayerBuff(_shopItemSO, playerController);

        Destroy(gameObject);

    }

    public void InitializedItem(ShopItemSO itemSO)
    {
        _shopItemSO = itemSO;
        priceText.text = $"${itemSO.price}";
        descriptionText.text = itemSO.description;
    }

    #region Buy
    private void ApplyCatBuff(ShopItemSO itemSO)
    {
        CatAgent cat = FindObjectOfType<CatAgent>();
        if (cat == null)
        {
            Debug.LogWarning("No cat found to apply buff");
            return;
        }
        
        if (!itemSO.isUpgradeItem)
        {
            // TODO cat mood value increase
            cat.ApplyBuff(itemSO.catMoodBuff);
        }
    }
    
    private void ApplyPlayerBuff(ShopItemSO itemSO, PlayerController player)
    {
        if (!itemSO.isUpgradeItem)
        {
            player.ApplyBuff(itemSO.buffTime, itemSO.playerSpeedBuff, itemSO.playerHPBuff);
        }
    }
    #endregion
}

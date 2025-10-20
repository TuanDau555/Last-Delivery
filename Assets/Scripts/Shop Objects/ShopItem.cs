using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : BaseInteract
{
    [Space(10)]
    [SerializeField] private Transform displayPoint;
    private GameObject visualDisplay;
    private ShopItemSO _itemSO;

    public override void Interact(PlayerController playerController)
    {
        if (_itemSO == null) return;
    }

    public void SetItem(ShopItemSO itemSO)
    {
        _itemSO = itemSO;

        if (visualDisplay != null)
        {
            Destroy(visualDisplay);
        }

        if (itemSO.displayVisual != null)
        {
            visualDisplay = Instantiate(itemSO.displayVisual, displayPoint.position, displayPoint.rotation, displayPoint);
        }
        else
        {
            Debug.LogWarning($"{itemSO.name} doesn't have anything to display");
        }
    }

    public ShopItemSO GetItem() => _itemSO;
}

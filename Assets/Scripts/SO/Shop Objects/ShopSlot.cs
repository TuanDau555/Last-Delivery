using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private Transform displayPoint;
    private GameObject visualDisplay;
    private ShopItemSO _itemSO;
    
    public void SetItem(ShopItemSO itemSO)
    {
        _itemSO = itemSO;

        if (visualDisplay != null)
        {
            Destroy(visualDisplay);
            visualDisplay = null;
        }

        if (itemSO == null) return;

        if (itemSO.displayVisual != null)
        {
            visualDisplay = Instantiate(itemSO.displayVisual, displayPoint.position, displayPoint.rotation, displayPoint);

            var shopItem = visualDisplay.GetComponent<ShopItem>();
            if(shopItem != null)
            {
                shopItem.InitializedItem(itemSO);
            }
        }
        else
        {
            Debug.LogWarning($"{itemSO.name} doesn't have anything to display");
        }
    }

    public ShopItemSO GetItem() => _itemSO;
}
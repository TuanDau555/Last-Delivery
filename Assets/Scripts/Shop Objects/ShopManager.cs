using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private float chanceToHaveItem = 0.7f;
    [SerializeField] private List<ShopItemSO> allShopItems;
    [SerializeField] private List<ShopSlot> shopSlots;
    private List<ShopItemSO> _currentItems = new List<ShopItemSO>();

    void Start()
    {
        GenerateTodayShop();
        DisplayOnWall();
    }

    private void DisplayOnWall()
    {
        for(int _shopSlot = 0; _shopSlot < shopSlots.Count; _shopSlot++)
        {
            var item = _currentItems[_shopSlot];

            if (item != null)
            {
                shopSlots[_shopSlot].SetItem(item);
            }
            else
            {
                shopSlots[_shopSlot].SetItem(null);
            }
        }
    }

    private void GenerateTodayShop()
    {
        _currentItems.Clear();

        // Choose random items 
        var randomItemPool = allShopItems.OrderBy(x => Random.value).ToList();

        for(int _shopSlot = 0; _shopSlot < shopSlots.Count; _shopSlot++)
        {
            bool hasItem = Random.value <= chanceToHaveItem;

            if (hasItem && randomItemPool.Count > 0)
            {
                _currentItems.Add(randomItemPool[0]);
                randomItemPool.RemoveAt(0);
            }
            else
            {
                _currentItems.Add(null);
            }
        }
    }
}
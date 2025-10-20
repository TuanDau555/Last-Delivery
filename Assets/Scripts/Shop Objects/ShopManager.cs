using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private List<ShopItemSO> allShopItems;
    [SerializeField] private List<ShopItem> shopItems;

    private List<ShopItemSO> _currentItems = new List<ShopItemSO>();

    void Start()
    {
        GenerateTodayShop();
        DisplayOnWall();
    }

    private void DisplayOnWall()
    {
        for(int _shopItem = 0; _shopItem < shopItems.Count; _shopItem++)
        {
            shopItems[_shopItem].SetItem(_currentItems[_shopItem]);
        }
    }

    private void GenerateTodayShop()
    {
        _currentItems.Clear();

        // Choose random items 
        var randomItem = allShopItems.OrderBy(x => Random.value).ToList();

        for(int _shopItem = 0; _shopItem < shopItems.Count; _shopItem++)
        {
            _currentItems.Add(randomItem[_shopItem]);
        }
    }
}
using Godot;

namespace dimghost;

public class ShopItemType
{
    public string Name { get; set; }
    public int MaxQuantity { get; set; }
    public int cost { get; set; }
    public string ItemSprite { get; set; }

}

public class BoughtShopItem
{
    public ShopItem Shopitem { get; set; }
    public int AmountBought { get; set; }
}

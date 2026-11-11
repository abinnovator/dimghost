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
public class ChatMessage
{
    public string text { get; set; }
    public string username { set; get; }

}
public class LocationType
{
    public string Name { get; set; }
    public string displayName { get; set; }
    public string scenePath { get; set; }
    public string image { get; set; }
    public int levelMin { get; set; }
    public string size { get; set; }
}

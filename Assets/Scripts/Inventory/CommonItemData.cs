using UnityEngine;

[System.Serializable]
public class CommonItemData
{


    //this class is used to store the persistent data of the common items in the player inventory. It is saved along the unique items list. in this 
    //way, I dont need to save each instance of the common items, but only the quantity of each item.

    public CommonItem commonItem;
    public int quantity;

    public CommonItemData(CommonItem data, int qty)
    {
        commonItem = data;
        quantity = qty;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//this class holds the inventory of the player, with the functions to control it. It is accessed through the GameData class, through DataManager.
//inventory is updated in real time but it's saved to gameData only when the player reaches a checkpoint (and thus when the game is loaded with the
//new intentory).

//I have a scriptable object called commonItem, listing a series of stats for the item. I'm passing this item type in another script called 
//commonItemData, which includes the quantity of the item, since I dont want it to be part of the scriptable object. Then the object commonItemData 
//is passed in the inventory in the commonItems list. Every time we pickup an item, we check whether any of the items already present in the inventory
// (the list commonItems) has the same itemID of the item passed in the function AddCommonItem. 
public class Inventory 
{
    public static Inventory Instance { get; private set; }

    public List<CommonItemData> commonItems;
    public List<UniqueItem> uniqueItems;

    //public Transform inventoryUI; // Reference to the UI container
    //public GameObject inventorySlotPrefab; // Prefab for each inventory slot

    private Dictionary<string, GameObject> itemSlots = new Dictionary<string, GameObject>();





    // Constructor
    public Inventory()
    {
        commonItems = new List<CommonItemData>();
        uniqueItems = new List<UniqueItem>();
    }

    // Add a common item to the inventory
    public void AddCommonItem(CommonItem item)
    {
        // Check if the item is already in the inventory
        CommonItemData existingItem = commonItems.Find(i => i.commonItem.itemID == item.itemID);

        if (existingItem != null)
        {
            // If the item exists, increase the quantity in commonItemData, with the quantityToAdd from commonItem.
            existingItem.quantity += item.quantityToAdd;
            //InventoryUIManager.Instance.UpdateUI(existingItem);
        }
        else
        {
            // If the item does not exist, add it to the list;

            CommonItemData newItemData = new CommonItemData(item, item.quantityToAdd);
            commonItems.Add(newItemData);
            //Debug.Log(InventoryUIManager.Instance);
            //InventoryUIManager.Instance.CreateUIElement(newItemData);
        }

    }



    // Add a unique item to the inventory, checking that this items doesnt exist already.
    public void AddUniqueItem(UniqueItem item)
    {
        if (!uniqueItems.Exists(i => i.itemID == item.itemID))
        {
            uniqueItems.Add(item); // Add the unique item only if it's not already in the inventory
        }

    }

    // Methos to remove inventory must be revised, since player may decide how much to remove, or we remove everything on death. Anyway, it shouldn't
    //be specified by the scriptableObject.
    //public void RemoveCommonItem(string itemID, int quantity)
    //{
    //    var item = commonItems.Find(i => i.itemID == itemID);
    //    if (item != null)
    //    {
    //        item.RemoveQuantity(quantity);
    //        if (item.quantityToAdd == 0)
    //        {
    //            commonItems.Remove(item); // Remove item if quantity is zero
    //        }
    //    }
    //}

    public void RemoveUniqueItem(string itemID)
    {
        var item = uniqueItems.Find(i => i.itemID == itemID);
        if (item != null)
        {
            uniqueItems.Remove(item); // Remove the unique item
        }
    }

    //rivedere questa parte
    //public Inventory GetInventory()
    //{
    //    Inventory updatedInventory = new Inventory();
    //    updatedInventory.commonItems = new List<CommonItem>(this.commonItems);
    //    updatedInventory.uniqueItems = new List<UniqueItem>(this.uniqueItems);
    //    return updatedInventory;
    //}

}

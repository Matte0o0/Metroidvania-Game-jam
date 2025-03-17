using UnityEngine;

[System.Serializable]

//apparently not used, dont remember
public class Item
{
    public string itemName; // The name of the item
    public string itemID; // A unique identifier for the item
    public string description; // A description of the item
    public Sprite icon; // An icon to represent the item in the UI
    public bool isStackable; // Can the item be stacked?
    public int maxStack; // Maximum number of items that can be stacked (only relevant if isStackable is true)

    // Constructor
    public Item(string name, string id, string desc, Sprite icon, bool stackable, int maxStack = 1)
    {
        this.itemName = name;
        this.itemID = id;
        this.description = desc;
        this.icon = icon;
        this.isStackable = stackable;
        this.maxStack = maxStack;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "New Common Item", menuName = "Inventory/Common Item")]
[System.Serializable]
public class CommonItem : ScriptableObject
{
    public string itemName; 
    public string itemID; 
    public string description; 
    public Sprite icon; // An icon to represent the item in the UI
    public int quantityToAdd;
    public int maxStack = 99;




    // Method to remove items from the inventory
    //public void RemoveQuantity(int _quantityToSubtract)
    //{
    //    quantity -= amount;
    //    if (quantity < 0)
    //    {
    //        quantity = 0; // Prevent negative quantities
    //    }
    //}
}
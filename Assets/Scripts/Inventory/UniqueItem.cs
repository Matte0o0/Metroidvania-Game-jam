using UnityEngine;

[CreateAssetMenu(fileName = "New Unique Item", menuName = "Inventory/Unique Item")]
[System.Serializable]
public class UniqueItem : ScriptableObject
{
    public string itemName; 
    public string itemID; 
    public string description; 
    public Sprite icon; // An icon to represent the item in the UI
    public bool isQuestItem; 

    //not sure this is needed.
    public int healthBonus; // The amount of health this item gives the player
    public int damageBonus; // The amount of damage this item gives the player
    public int armorBonus; // The amount of armor this item gives the player

}
using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public CommonItem item;
    public UniqueItem uniqueItem;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (item != null)
            {
                CollectCommonItem();
            }
            else if (uniqueItem != null)
            {
                CollectUniqueItem();
            }
        }
    }

    private void CollectUniqueItem()
    {
        Debug.Log(uniqueItem + " collected");
        DataManager.Instance.gameData.playerInventory.AddUniqueItem(uniqueItem);
        Destroy(gameObject);
    }

    //updates the player's inventory with the item collected
    private void CollectCommonItem()
    {
        Debug.Log(item + " collected");
        DataManager.Instance.gameData.playerInventory.AddCommonItem(item);
        Destroy(gameObject);
    }
}
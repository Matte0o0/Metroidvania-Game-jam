using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private SaveLoadSystem saveLoadSystem;
    private float timePassed;

    private void Start()
    {
        saveLoadSystem = FindObjectOfType<SaveLoadSystem>();
    }

    private void Update()
    {
        //DataManager.Instance.UpdateTimePassed(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //this saves the current scene name, so that when we load the game, we know the last scene and thus the spawn point. This may need
            //to be separated in case we do hollow knight style and we save after picking up items and such.
            DataManager.Instance.SaveCheckpoint();

            //this saves the player's inventory, in the gameData object
            //Inventory updatedInventory = DataManager.Instance.gameData.playerInventory.GetInventory();
            //DataManager.Instance.gameData.playerInventory = updatedInventory;

            saveLoadSystem.SaveData();
            Debug.Log("Game Saved");
        }
    }
}
using System.Collections.Generic;

[System.Serializable]

//gameData stores game data. when it's constructed, it initializes the playerInventory. right now we dont pass a custom constructor which initialises
// all the values. Maybe I should do it for clarity. this would require change also in the SaveLoadSystem.cs
public class GameData
{
    //examples
    public float timePassed;
    public int coinsCollected;
    public int playerHealth;

    //name of the last checkpoint the player reached, SINCE save occurs only when reaching a checkpoint. BEWARE of saving on death, this should not
    //be saved.
    public string currentScene;
    public Inventory playerInventory;

    // Constructor to initialize playerInventory. when loading data, we will overwrite this object with the one from the save file, thus updating
    //the inventory. Since inventory isn't a monobehavior, it will be accessed through this variable. The game data references the live inventory
    //instance, which is updated in real time. When the player reaches a checkpoint, the inventory is saved to the gameData object.


    public GameData(Inventory liveInventory)
    {
        playerInventory = liveInventory;
        playerHealth = 150;
    }

    //need part in which items are saved to hubInventory.

}

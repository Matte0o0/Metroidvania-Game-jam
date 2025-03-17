using UnityEngine;
using UnityEngine.SceneManagement;

//this scripts creates and holds the GameData variable, holding the data of the game. This data may be saved and loaded, in that case the gameData 
//variable will be overwritten.
//This also manages the inventory data, which is accessed through the gameData object. To modify anything related to the inventory, you must access
//the dataManager instance and its gameData object, and then the playerInventory object. (probably would also work by modyfying the inventory directly)
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public GameData gameData { get; set; } // The game data containing a reference to the inventory
    public Inventory liveInventory; // The live inventory instance
       

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //RIGHT NOW, inventory is created here, and a reference is passed in the gamedata script. Everytime the inventory is updated, it is
            //updated in the gamedata and gamedata manager.
            liveInventory = new Inventory();
            gameData = new GameData(liveInventory);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    //general game data. if I wanted to be coherent I would add another script to have these functions and keep them separate from the manager,
    //since that is what I'm doing with the inventory.
    public void UpdateTimePassed(float deltaTime)
    {
        gameData.timePassed += deltaTime;
    }

    public void AddCoins(int amount)
    {
        gameData.coinsCollected += amount;
    }

    //this changes the data, and calls for the UI to update the health bar.
    public void UpdatePlayerHealth(int health)
    {
        gameData.playerHealth += health;
        UIManager.Instance.UpdatePlayerHealthBar();
    }
    // New method to update the current scene name when a checkpoint is reached
    public void SaveCheckpoint()
    {
        gameData.currentScene = SceneManager.GetActiveScene().name;
    }
    public string GetCurrentScene()
    {
        return gameData.currentScene;
    }

    //check again later, maybe needs to change
    public void ResetCurrentData()
    {
        gameData = new GameData(liveInventory);
    }


}
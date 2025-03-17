using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//CARE: right now not all instances are directly referenced, some are called directly, and this may have a performance hit. problem is, by
//initialising them, order mey get screwed and that's bad. check lazy initialisation for this.
//This script manages the scene loading. Everytime a scene is loaded, a function is called from this script.
public enum GameState
{
    MainMenu,
    LoadingGame,
    Playing,
    InGameMenu,
    CharacterMenu,
    GameOver
}

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public static string PreviousSceneName { get; private set; }

    private GameState currentState;
    private string nextGameSceneName;
    public Spawner spawner;
    public SaveLoadSystem saveLoadSystem;
    private float timePassed;

    UIManager uiManager;
    //private GameObject pauseMenuUI; // Instance of the pause menu UI
    //private GameObject characterMenuUI; // Instance of the character menu UI

    public GameState CurrentState => currentState; // Public property to expose the current state

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //should not load in here, since start will occur only once. If player dies and retries, this wont be called and data wont be loaded. maybe there
    //is better place where to put it, for now I have duplicate code in the retry function.
    void Start()
    {
        saveLoadSystem = FindObjectOfType<SaveLoadSystem>();
        GameData data = saveLoadSystem.LoadData();
        //List<CommonItemData> commonItems = data.playerInventory.commonItems;
        //List<UniqueItem> uniqueItems = data.playerInventory.uniqueItems;
        //foreach (var item in commonItems)
        //{
        //    Debug.Log($"Item ID: {item.commonItem.itemID}, Quantity: {item.quantity}");
        //}
        //foreach (var item in uniqueItems)
        //{
        //    Debug.Log($"Item ID: {item.itemID}");
        //}


        ChangeState(GameState.MainMenu);
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
    }

    public void ChangeState(GameState newState, string sceneName = null)
    {
        currentState = newState;
        nextGameSceneName = sceneName;
        HandleStateChange();
    }

    private void HandleStateChange()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                LoadMainMenu();
                break;
            case GameState.LoadingGame:
                LoadGameScene(nextGameSceneName);
                break;
            case GameState.Playing:
                GameResume();
                break;
            case GameState.InGameMenu:
                InGameMenuPause();
                break;
            case GameState.CharacterMenu:
                CharacterMenuPause();
                break;
            case GameState.GameOver:
                LoadGameOver();
                break;
        }
    }

    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SceneManager.sceneLoaded += OnMainMenuLoaded;

        UIManager.Instance.menuPanel.SetActive(true);

        UIManager.Instance.pausePanel.SetActive(false);
        UIManager.Instance.inventoryPanel.SetActive(false);
        UIManager.Instance.gameOverPanel.SetActive(false);
        UIManager.Instance.inGamePanel.SetActive(false);
    }

    public void LoadGameScene(string sceneName)
    {
        //previous scene name is needed in the player spawner, to go back and forth between scenes, so that the spawner can find the right 
        //playerSpawner corresponding to the previous scene name.
        PreviousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnGameSceneLoaded;
    }

    //pause and resume game do not change the scene (for now), so they implement the state switch inside them directly. Other functions which 
    //change the scene will be split between the scene change and what happens after the scene is loaded. (e.g. LoadGameScene and OnGameSceneLoaded)
    private void InGameMenuPause()
    {
        UIManager.Instance.pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    private void CharacterMenuPause()
    {
        UIManager.Instance.inventoryPanel.SetActive(true);
        Time.timeScale = 0;
    }
    //this will be called both from the inGamePause state, CharacterPause state and the loading state.
    private void GameResume()
    {
        UIManager.Instance.pausePanel.SetActive(false);
        UIManager.Instance.inventoryPanel.SetActive(false);
        Time.timeScale = 1;
    }



    //private void CharacterMenuResume()
    //{
    //    if (characterMenuUI != null)
    //    {
    //        characterMenuUI.SetActive(false);
    //    }
    //    Time.timeScale = 1;
    //}

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
        UIManager.Instance.gameOverPanel.SetActive(true);
        SceneManager.sceneLoaded += OnGameOverLoaded;
    }

    private void OnMainMenuLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // Needed when coming from the game scene.
                Destroy(player);
            }
            SceneManager.sceneLoaded -= OnMainMenuLoaded;
        }
    }

    //triggers when the start button is pressed in the main menu. It will load the first scene, or the saved scene if there is one. it will also
    //deactivate the main menu UI for the game scenes and activate the in game UI.
    //CARE. start game, along with retry game, are the only functions that directly load the game scene, so both need to consider the UI changes.
    //I should add a function that changes the UI for the game scenes, so that it's called in both cases.
    public void StartGame()
    {
        // Get the saved scene name from DataManager, if it's null, then load the tutorial/first scene, this should not be needed as, for now
        //ther is an instant checkpoint so that there will always be a saved scene, once the game is started for the first time.
        string savedScene = DataManager.Instance.GetCurrentScene();
        string sceneToLoad = string.IsNullOrEmpty(savedScene) ? "First Scene" : savedScene;

        //care, UI gets loaded before the scene, so this is kind of ugly. Find alternative.
        UIManager.Instance.menuPanel.SetActive(false);
        UIManager.Instance.inGamePanel.SetActive(true);
        ChangeState(GameState.LoadingGame, sceneToLoad);
    }

    //spawns everything in the proper order, then finds the player and sets the camera to follow it, then instantiates the pause menu which will be
    //inactive until the player presses the pause button. At last switched to the playing state, that resumes the game (even if it wasn't paused).
    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        spawner = FindObjectOfType<Spawner>();
        spawner.SpawnPlayers();
        //spawner.SpawnNPC();
        //spawner.SpawnEnemy();

        CameraFollow.Instance.FindPlayer();
        SceneManager.sceneLoaded -= OnGameSceneLoaded;

        ChangeState(GameState.Playing);
    }

    //maybe I will need to destroy the player game object, or maybe not since it will be destroyed when it's killed. In that case maybe there is a 
    //death animation, so we would wait for the animation to finish, trigger the destroy function and then load the game over scene.
    private void OnGameOverLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameOver")
        {
            SceneManager.sceneLoaded -= OnGameOverLoaded;

            // Assuming you have buttons for retry and quit in the GameOver scene
            //Button retryButton = GameObject.Find("RetryButton").GetComponent<Button>();
            //Button quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

            //retryButton.onClick.AddListener(() => RetryGame());
            //quitButton.onClick.AddListener(() => QuitGame());
        }
    }

    //BEWARE. right now this directly loads the game scene, without passing from the main menu. If we add any initialisation logic on the main menu
    //loading, we should remember this. for now I'm resetting the data, so that it doesnt get saved on death. This should be expanded upon, since
    //there may be different mechanics for how we handle inventory, and maybe not all data should be reset (like hollow knight, if you pick a 
    //unique item this will stay even if you die right after).
    public void RetryGame()
    {
        string savedScene = DataManager.Instance.GetCurrentScene();
        string sceneToLoad = string.IsNullOrEmpty(savedScene) ? "First Scene" : savedScene;

        DataManager.Instance.ResetCurrentData();

        //needed since the start function is called only once, and if the player dies and retries, the data wont be loaded. 
        GameData data = saveLoadSystem.LoadData();
        List<CommonItemData> commonItems = data.playerInventory.commonItems;
        List<UniqueItem> uniqueItems = data.playerInventory.uniqueItems;
        //foreach (var item in commonItems)
        //{
        //    Debug.Log($"Item ID: {item.commonItem.itemID}, Quantity: {item.quantity}");
        //}
        //foreach (var item in uniqueItems)
        //{
        //    Debug.Log($"Item ID: {item.itemID}");           
        //}

        UIManager.Instance.gameOverPanel.SetActive(false);
        UIManager.Instance.inGamePanel.SetActive(true);
        ChangeState(GameState.LoadingGame, sceneToLoad);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }



}
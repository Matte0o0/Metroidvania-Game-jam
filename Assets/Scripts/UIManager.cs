using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//This will manage all the UI elements in the game, including the menu and the inventory. It's initialised in the main menu scene and persists
// in the game scenes. It holds all the UI references and the buttons.
//CARE: right now not all instances are directly referenced, some are called directly, and this may have a performance hit. problem is, by
//initialising them, order mey get screwed and that's bad. check lazy initialisation for this.
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    DataManager dataManager;
    //SceneLoader sceneLoader;
    

    private GameData data;
    public Canvas canvas;

    [Header("Main Menu UI")]
    public GameObject menuPanel; //Main menu UI
    public Button startButton; //Start button in the main menu
    public TextMeshProUGUI mainMenuText; //Text element to display the main menu

    [Header("Pause Menu UI")]
    public GameObject pausePanel; // in game pause menu UI
    public Button pauseToMenuButton; // go back to main menu button
    public TextMeshProUGUI pauseMenuText; //Text element to display the pause menu

    [Header("Character Menu UI")]
    public GameObject inventoryPanel; // character menu UI/inventory
    public TextMeshProUGUI inventoryText; //Text element to display the inventory
    public TextMeshProUGUI itemText;

    [Header("GameOver Menu UI")]
    public GameObject gameOverPanel; // game over menu UI
    public Button retryButton; // retry button
    public Button mainMenuButton; // go back to main menu button
    public Button quitButton; // quit button
    public TextMeshProUGUI gameOverText; //Text element to display the game over menu

    [Header("In Game UI")]
    public GameObject inGamePanel; // in game UI
    public TextMeshProUGUI healthText; //Text element to display the health

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes

            DontDestroyOnLoad(canvas.gameObject);

        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances

            Destroy(canvas.gameObject);

        }
    }

    private void Start()
    {
        DataManager dataManager = DataManager.Instance;
        //SceneLoader sceneLoader = SceneLoader.Instance;
        data = dataManager.gameData;

        SetUpMainMenu();
        SetUpInventoryMenu();
        SetUpPauseMenu();
        SetUpGameOverMenu();
        SetUpGameUI();
        // Update UI with the current inventory
        //DisplayInventory();
    }

    private void Update()
    {
        // Update the inventory display every frame (for demonstration purposes)
        //DisplayInventory();
    }
    public void DisplayInventory()
    {
        // Start building the display string
        string displayText = "Inventory:\n";

        // List for common items
        List<CommonItemData> commonItems = data.playerInventory.commonItems;
        List<UniqueItem> uniqueItems = data.playerInventory.uniqueItems;

        // Display common items
        displayText += "Common Items:\n";
        foreach (var item in commonItems)
        {
            displayText += $"Item ID: {item.commonItem.itemID}, Quantity: {item.quantity}\n";
        }

        // Display unique items
        displayText += "\nUnique Items:\n";
        foreach (var item in uniqueItems)
        {
            displayText += $"Item: {item.itemName}\n";
        }

        // Update the UI text element with the new inventory string
        itemText.text = displayText;
    }

    private void SetUpPauseMenu()
    {
        if (pausePanel != null)
        {
            pauseToMenuButton.onClick.AddListener(() => SceneLoader.Instance.LoadMainMenu());           
        }
    }

    private void SetUpMainMenu()
    {
        if (menuPanel != null)
        {
            if (startButton != null)
            {
                startButton.onClick.AddListener(() => SceneLoader.Instance.StartGame());
            }
        }
    }

    //since there are no buttons in the inventory, we don't need to set up any listeners for now and function is empty.
    private void SetUpInventoryMenu()
    {

    }

    private void SetUpGameOverMenu()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            retryButton.onClick.AddListener(() => SceneLoader.Instance.RetryGame());
            mainMenuButton.onClick.AddListener(() => SceneLoader.Instance.LoadMainMenu());
            quitButton.onClick.AddListener(() => SceneLoader.Instance.QuitGame());
        }
    }


    private void SetUpGameUI()
    {
        UpdatePlayerHealthBar();
    }

    //this occurs at game start and when health data changes thorugh the manager.
    public void UpdatePlayerHealthBar()
    {
        healthText.text = "Health: " + data.playerHealth;
    }
}


    


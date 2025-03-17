using UnityEngine;

//Singleton, initiated in first game scene, but can also be initiated in main menu.
//Holds reference of spawned player characters, which will be passed back to the spawner on game scene change. (since spawner script is not a singleton,
//and will lose reference to existing players)
//Maanages switch between characters, their activation and deactivation, both on spawn and on switch.

//WARNING. switch resets player state to idle. FIND A FIX. ROBA IMPORTANTE
//Problema1 quando cambio personaggio, a volte guarda in direzione opposta (ovvero quella in cui ho lasciato il personaggio inizialmente), chiamare 
//funzione per flippare personaggio quando si cambia.
public class CharactersManager : MonoBehaviour
{
    public static CharactersManager Instance { get; private set; }
    public GameObject[] characters;
    public int currentCharacterIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize(GameObject[] spawnedCharacters)
    {
        characters = spawnedCharacters;

        // Set DontDestroyOnLoad for each character
        foreach (GameObject character in characters)
        {
            DontDestroyOnLoad(character);
        }

        // Deactivate all but the first character
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == currentCharacterIndex);
        }
    }

    public Transform GetCurrentCharacterTransform()
    {
        return characters[currentCharacterIndex].transform;
    }

    public void SwitchCharacter(int direction)
    {
        // Get the current transform data
        Transform currentTransform = characters[currentCharacterIndex].transform;

        // Deactivate the current character
        characters[currentCharacterIndex].SetActive(false);

        // Calculate the new character index
        currentCharacterIndex = (currentCharacterIndex + direction + characters.Length) % characters.Length;

        // Set the new character's transform to match the old character's transform
        characters[currentCharacterIndex].transform.position = currentTransform.position;
        characters[currentCharacterIndex].transform.rotation = currentTransform.rotation;
        //characters[currentCharacterIndex].transform.localScale = currentTransform.localScale;

        // Activate the new character
        characters[currentCharacterIndex].SetActive(true);
    }
}

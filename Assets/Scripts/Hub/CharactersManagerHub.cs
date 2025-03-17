using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//WARNING. UNUSED SCRIPT. ITS FUNCIONALITY MAY BE USEFUL FOR THE HUB.
//allows to switch between player and AI characters. Also initialises the characters so that we get a hold of the characters present in the scene,
//and we know which one is controlled by the player.
public class CharactersManagerHub : MonoBehaviour
{
    private static CharactersManager instance;

    public GameObject playerCharacter;
    public GameObject aiCharacter;
    public GameObject currentPlayerCharacter;


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //StartCoroutine(InitializeCharacters());
    }

    private void Start()
    {
        //InitialiseCharacters();
    }


    //Initialise spawned functions work only when the characters are spawned in the scene. NPCs wont be carried through, and the player is 
    //initialised also when he is carried through scenes with dontdestroyonload
    public void InitialiseSpawnedPlayer(GameObject _playerCharacter)
    {
        playerCharacter = _playerCharacter;
        currentPlayerCharacter = _playerCharacter;
        SetControl(_playerCharacter, true);
        _playerCharacter.tag = "Player";

    }
    public void InitialiseSpawnedAI(GameObject _aiCharacter)
    {
        aiCharacter = _aiCharacter;
        SetControl(aiCharacter, false);
        aiCharacter.tag = "NPC";

    }

    public bool SwitchCharacter()
    {    

        if (aiCharacter == null)
        {
            Debug.LogWarning("AI character not found. Cannot switch characters.");
            return false;
        }

        if (currentPlayerCharacter == playerCharacter)
        {
            SetControl(playerCharacter, false);
            SetControl(aiCharacter, true);
            currentPlayerCharacter = aiCharacter;
        }
        else
        {
            SetControl(aiCharacter, false);
            SetControl(playerCharacter, true);
            currentPlayerCharacter = playerCharacter;
        }

        SwapTags(playerCharacter, aiCharacter);
        ReferenceManager.Instance.Register("PlayerTransform", currentPlayerCharacter.transform);
        return true;
    }

    void SetControl(GameObject character, bool isPlayerControlled)
    {

        var playerInput = character.GetComponent<PlayerInput>();


        if (playerInput != null)
        {
            playerInput.enabled = isPlayerControlled;
        }


    }

    void SwapTags(GameObject character1, GameObject character2)
    {
        string tempTag = character1.tag;
        character1.tag = character2.tag;
        character2.tag = tempTag;

    }
}
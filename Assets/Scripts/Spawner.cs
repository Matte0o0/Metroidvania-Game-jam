using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

//inizializzato in caricamento di game scene
public class Spawner : MonoBehaviour
{
    //public GameObject playerPrefab;
    //public GameObject npcPrefab;
    //public GameObject enemyPrefab;
    //private GameObject playerInstance;
    //private GameObject npcInstance;
    //private GameObject enemyInstance;

    public CharactersManager charactersManager;


    public GameObject character1Prefab;
    public GameObject character2Prefab;
    public GameObject character3Prefab;

    private GameObject[] playerInstances;


    //WARNING. I DONT UNDERSTAND NEXT FUNCTIONS. BOH
    //passes character manager to spawner. Needs to be done through a function since  
    public void GetCharactersManager()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
    }



    //this function spawns the player in the scene where the saved checkpoint is located. If the player is already in the scene, at scene change it
    //will be moved to the spawn point. The player is also initialised in the characters manager script, so that we know which character is
    //controlled by the player, needed for the switch. There is an initial checkpoint in the first scene/tutorial, which may need to be removed

    public void SpawnPlayers()
    {
        // Array to store player instances
        
        GameObject[] playerInstancesFound = GameObject.FindGameObjectsWithTag("Player");

        if (playerInstancesFound.Length == 0)
        {
            // No players found, spawn all at the checkpoint
            playerInstances = new GameObject[3];
            
            GameObject checkpointSpawner = GameObject.Find("CheckpointSpawner");
            if (checkpointSpawner != null)
            {
                playerInstances[0] = Instantiate(character1Prefab, checkpointSpawner.transform.position, checkpointSpawner.transform.rotation);
                playerInstances[1] = Instantiate(character2Prefab, checkpointSpawner.transform.position, checkpointSpawner.transform.rotation);
                playerInstances[2] = Instantiate(character3Prefab, checkpointSpawner.transform.position, checkpointSpawner.transform.rotation);

                charactersManager = FindObjectOfType<CharactersManager>();
                if (charactersManager != null)
                {
                    charactersManager.Initialize(playerInstances);
                }
                else
                {
                    Debug.LogError("CharactersManager not found in the scene");
                }

                // Register the players' transforms in the ReferenceManager
                for (int i = 0; i < playerInstances.Length; i++)
                {
                    ReferenceManager.Instance.Register("PlayerTransform" + (i + 1), playerInstances[i].transform);
                }
            }
            else
            {
                Debug.LogError("CheckpointSpawner not found in the scene");
            }
        }
        else
        {
            // Players found, reposition them based on the spawner connected to the previous scene
            playerInstances = CharactersManager.Instance.characters;
            string previousSceneName = SceneLoader.PreviousSceneName;
            PlayerSpawner[] spawners = GameObject.FindObjectsOfType<PlayerSpawner>();

            foreach (PlayerSpawner spawner in spawners)
            {
                //maybe we dont need all of this, but since we are checking all spawners, but there arent many anyway.
                if (spawner.connectedSceneName == previousSceneName)
                {
                    playerInstances[CharactersManager.Instance.currentCharacterIndex].transform.position = spawner.transform.position;
                    playerInstances[CharactersManager.Instance.currentCharacterIndex].transform.rotation = spawner.transform.rotation;

                }
            }
        }

        // Ensure each player has the CharactersManager reference
        foreach (var player in playerInstances)
        {
            player.GetComponent<PlayerInput>().GetCharactersManager();
        }
    }


    //public void SpawnPlayer()
    //{
    //    playerInstance = GameObject.FindWithTag("Player");

    //    if (playerInstance == null)
    //    {
    //        GameObject spawner = GameObject.Find("CheckpointSpawner");
    //        if (spawner != null)
    //        {
    //            playerInstance = Instantiate(playerPrefab, spawner.transform.position, spawner.transform.rotation);
    //            DontDestroyOnLoad(playerInstance);
    //            charactersManager.Initialize(playerInstance);
    //            ReferenceManager.Instance.Register("PlayerTransform", playerInstance.transform);

    //        }
    //    }
    //    else
    //    {
    //        // Get the name of the previous scene
    //        string previousSceneName = SceneLoader.PreviousSceneName;

    //        // Find all objects with the PlayerSpawner component
    //        PlayerSpawner[] spawners = GameObject.FindObjectsOfType<PlayerSpawner>();

    //        // Iterate through all spawners to find the correct one
    //        foreach (PlayerSpawner spawner in spawners)
    //        {
    //            if (spawner.connectedSceneName == previousSceneName)
    //            {
    //                // Set player position and rotation to the spawner's position and rotation
    //                playerInstance.transform.position = spawner.transform.position;
    //                playerInstance.transform.rotation = spawner.transform.rotation;

    //                // Reinitialize the player in the characters manager
    //                charactersManager.Initialize(playerInstance);
    //                break;
    //            }
    //        }
    //    }
    //    playerInstance.GetComponent<PlayerInput>().GetCharactersManager();
    //}

    //public void SpawnNPC()
    //{
    //    npcInstance = GameObject.FindWithTag("NPC");

    //    if (npcInstance == null)
    //    {
    //        GameObject NpcSpawner = GameObject.Find("NPCSpawner");
    //        if (NpcSpawner != null)
    //        {
    //            npcInstance = Instantiate(npcPrefab, NpcSpawner.transform.position, NpcSpawner.transform.rotation);
    //        }
    //    }
    //    else
    //    {
    //        GameObject spawner = GameObject.Find("NPCSpawner");
    //        if (spawner != null)
    //        {
    //            npcInstance.transform.position = spawner.transform.position;
    //            npcInstance.transform.rotation = spawner.transform.rotation;
    //        }
    //    }
    //}

    ////we need a proper script to spawn the enemies
    //public void SpawnEnemy()
    //{
    //    GameObject EnemySpawner = GameObject.Find("EnemySpawner");
    //    if (EnemySpawner != null)
    //    {
    //        enemyInstance = Instantiate(enemyPrefab, EnemySpawner.transform.position, EnemySpawner.transform.rotation);
    //    }
    //}

    //this works is there is only one NPC in the map. We will need fix
    public void SetPlayerDontDestroyOnLoad()
    {
        GameObject currentPlayer = GameObject.FindWithTag("Player");
        GameObject nonCurrentPlayer = GameObject.FindWithTag("NPC");


        if (currentPlayer != nonCurrentPlayer)
        {

            SceneManager.MoveGameObjectToScene(nonCurrentPlayer, SceneManager.GetActiveScene());
            DontDestroyOnLoad(currentPlayer);
            nonCurrentPlayer = currentPlayer;

        }
    }


}

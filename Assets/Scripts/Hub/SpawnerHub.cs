//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Numerics;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using static UnityEditor.Experimental.GraphView.GraphView;

////WARNING. THIS IS NOT CURRENTLY USED, BUT MAY BE USEFUL IN THE HUB SCENE.
//public class SpawnerHub : MonoBehaviour
//{
//    public GameObject playerPrefab;
//    public GameObject npcPrefab;
//    public GameObject enemyPrefab;
//    private GameObject playerInstance;
//    private GameObject npcInstance;
//    private GameObject enemyInstance;

//    public CharactersManager charactersManager;
//    //public PlayerInput playerInput;


//    void Start()
//    {
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    //WARNING. I DONT UNDERSTAND NEXT FUNCTIONS. BOH
//    //passes character manager to spawner. Needs to be done through a function since  
//    public void GetCharactersManager()
//    {
//        charactersManager = FindObjectOfType<CharactersManager>();
//    }



//    //this function spawns the player in the scene where the saved checkpoint is located. If the player is already in the scene, at scene change it
//    //will be moved to the spawn point. The player is also initialised in the characters manager script, so that we know which character is
//    //controlled by the player, needed for the switch. There is an initial checkpoint in the first scene/tutorial, which may need to be removed
//    public void SpawnPlayer()
//    {
//        playerInstance = GameObject.FindWithTag("Player");

//        if (playerInstance == null)
//        {
//            GameObject spawner = GameObject.Find("CheckpointSpawner");
//            if (spawner != null)
//            {
//                playerInstance = Instantiate(playerPrefab, spawner.transform.position, spawner.transform.rotation);
//                DontDestroyOnLoad(playerInstance);
//                charactersManager.InitialiseSpawnedPlayer(playerInstance);
//                ReferenceManager.Instance.Register("PlayerTransform", playerInstance.transform);
                
//            }
//        }
//        else
//        {
//            // Get the name of the previous scene
//            string previousSceneName = SceneLoader.PreviousSceneName;

//            // Find all objects with the PlayerSpawner component
//            PlayerSpawner[] spawners = GameObject.FindObjectsOfType<PlayerSpawner>();

//            // Iterate through all spawners to find the correct one
//            foreach (PlayerSpawner spawner in spawners)
//            {
//                if (spawner.connectedSceneName == previousSceneName)
//                {
//                    // Set player position and rotation to the spawner's position and rotation
//                    playerInstance.transform.position = spawner.transform.position;
//                    playerInstance.transform.rotation = spawner.transform.rotation;

//                    // Reinitialize the player in the characters manager
//                    charactersManager.InitialiseSpawnedPlayer(playerInstance);
//                    break;
//                }
//            }
//        }
//        playerInstance.GetComponent<PlayerInput>().GetCharactersManager();
//    }

//    public void SpawnNPC()
//    {
//        npcInstance = GameObject.FindWithTag("NPC");

//        if (npcInstance == null)
//        {
//            GameObject NpcSpawner = GameObject.Find("NPCSpawner");
//            if (NpcSpawner != null)
//            {
//                npcInstance = Instantiate(npcPrefab, NpcSpawner.transform.position, NpcSpawner.transform.rotation);
//                charactersManager.InitialiseSpawnedAI(npcInstance);
//            }
//        }
//        else
//        {
//            GameObject spawner = GameObject.Find("NPCSpawner");
//            if (spawner != null)
//            {
//                npcInstance.transform.position = spawner.transform.position;
//                npcInstance.transform.rotation = spawner.transform.rotation;
//            }
//        }
//    }

//    public void SpawnEnemy()
//    {
//        GameObject EnemySpawner = GameObject.Find("EnemySpawner");
//        if (EnemySpawner != null)
//        {
//            enemyInstance = Instantiate(enemyPrefab, EnemySpawner.transform.position, EnemySpawner.transform.rotation);
//        }
//    }

//    //this works is there is only one NPC in the map. We will need fix
//    public void SetPlayerDontDestroyOnLoad()
//    {
//        GameObject currentPlayer = GameObject.FindWithTag("Player");
//        GameObject nonCurrentPlayer = GameObject.FindWithTag("NPC");


//        if (currentPlayer != nonCurrentPlayer)
//        {

//            SceneManager.MoveGameObjectToScene(nonCurrentPlayer, SceneManager.GetActiveScene());
//            DontDestroyOnLoad(currentPlayer);
//            nonCurrentPlayer = currentPlayer;

//        }
//    }


//}

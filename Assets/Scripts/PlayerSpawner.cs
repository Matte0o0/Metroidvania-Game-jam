using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is connected to a game object representing the player spawn point. When the player enters the scene, the Spawner.cs script will look
//for the spawner script holding the connectedSceneName (set manually) that matches the previous scene name, which is stored in the Spawner.cs.
public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] public string connectedSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

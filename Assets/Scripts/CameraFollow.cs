using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton, initiated in main menu.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 0.1f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 groundedOffset; // Offset when the player is grounded
    bool isGrounded;

    [SerializeField] private Vector3 currentOffset;
    private Transform playerTransform;

    CharacterBehavior characterBehavior;
    private static CameraFollow instance;


    public static CameraFollow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraFollow>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(CameraFollow).ToString());
                    instance = singleton.AddComponent<CameraFollow>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {        
        FindPlayer();
        currentOffset = offset;
    }



    // Update is called once per frame
    void LateUpdate()
    {
        if (playerTransform != null)
        {          
            //AdjustCameraOffset();
            transform.position = Vector3.Lerp(transform.position, playerTransform.position + currentOffset, followSpeed);
        }
    }

    public void OnTabPressed()
    {
        FindPlayer();
    }

    // Method to find the player by tag
    public void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

    }
    private void AdjustCameraOffset()
    {
        isGrounded = characterBehavior.Grounded();
        currentOffset = isGrounded ? groundedOffset : offset;
    }
}

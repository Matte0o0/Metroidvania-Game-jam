using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

//script uses new unity input system. Handles input and it's connected to the two state machines, the player state machine and the game state machine.
//all UI input/game input is managed through this script, while for the player some of it is managed through the player state machine. Maybe change
//in the future.
//one thing is not clear. Right now this script is in each character the player may control. Maybe we need only one playerInput, as it's the state
//machine manager.
public class PlayerInput : MonoBehaviour
{
    public ICharacterState currentState;

    private CameraFollow camerafollow;
    public CharactersManager charactersManager;
    public CharacterBehavior characterBehavior;
    private Spawner spawner;



    //unity class storing all the inputs

    public PlayerInputsList controls { get; private set; }

    GameObject startPoint;

    void Awake()
    {
        controls = new PlayerInputsList();
    }

    //beware that everything playing after SetState won't be played.
    void Start()
    {
          
    }

    //maybe not needed
    void OnEnable()
    {
        // Enable the controls and subscribe to actions
        controls.Enable();
        controls.UI.Pause.performed += OnPause;
        controls.UI.CharacterMenu.performed += OnCharacterMenu;
        //care. Switch character is a player mechanic, it should be handled in the various player states, not here.
        controls.Player.SwitchCharacterRight.performed += ctx => SwitchCharacter(1);
        controls.Player.SwitchCharacterLeft.performed += ctx => SwitchCharacter(-1);
        controls.Player.Death.performed += ctx => OnDeath();

        controls.Debug.Test.performed += ctx => DebugTest();


        //this makes sure characherManager is found eveerythime player switches character, since the new player character had a deactivatd playerInput
        //and so the reference wont be obtained on load.
        characterBehavior = GetComponent<CharacterBehavior>();
        charactersManager = FindObjectOfType<CharactersManager>();
        camerafollow = FindObjectOfType<CameraFollow>();
        spawner = FindObjectOfType<Spawner>();
        SetState(new IdleState(this));        
    }

    void OnDisable()
    {
        // Disable the controls and unsubscribe from actions
        controls.Disable();
        controls.UI.Pause.performed -= OnPause;
        controls.UI.CharacterMenu.performed -= OnCharacterMenu;
        controls.Player.SwitchCharacterRight.performed -= ctx => SwitchCharacter(1);
        controls.Player.SwitchCharacterLeft.performed -= ctx => SwitchCharacter(-1);
        controls.Player.Death.performed -= ctx => OnDeath();


        controls.Debug.Test.performed -= ctx => DebugTest();
    }

    void Update()
    {

        //this is the player state only, not the game state.
        currentState.Execute();
    }

    private void FixedUpdate()
    {
        currentState.FixedExecute();
    }
    //this is needed since playerInput is set notdestroyonload and we need to get the characters manager script from the new scene, when it's
    //loaded. so every time SpawnPlayer is called, we need to get the a new reference to the characters manager.
    public void GetCharactersManager()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        Debug.Log("Characters Manager found" + charactersManager);
    }

    public void SetState(ICharacterState newState)
    {
        //string currentStateName = currentState != null ? currentState.GetType().Name : "None";
        //Debug.Log($"Transitioning from {currentStateName} to {newState.GetType().Name}");
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();

    }
    //public void SetState(ICharacterState newState)
    //{
    //    if (currentState != null && currentState.GetType() == newState.GetType())
    //    {
    //        Debug.Log($"Already in state: {newState.GetType().Name}");
    //        return;
    //    }

    //    string currentStateName = currentState != null ? currentState.GetType().Name : "None";
    //    Debug.Log($"Transitioning from {currentStateName} to {newState.GetType().Name}");
    //    currentState?.Exit();
    //    currentState = newState;
    //    currentState.Enter();
    //}



    //pause menu, should be accessed by pressing "esc". Also disables player controls when game is paused and viceversa.
    private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (SceneLoader.Instance.CurrentState == GameState.Playing)
        {
            SceneLoader.Instance.ChangeState(GameState.InGameMenu);
            DisablePlayerControls();
        }
        else if (SceneLoader.Instance.CurrentState == GameState.InGameMenu || SceneLoader.Instance.CurrentState == GameState.CharacterMenu)
        {
            SceneLoader.Instance.ChangeState(GameState.Playing);
            EnablePlayerControls();
        }
    }

    //this is probably the inventory/character menu, accessed by pressing "I" or something like that, not the pause one. careful
    private void OnCharacterMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (SceneLoader.Instance.CurrentState == GameState.Playing)
        {
            SceneLoader.Instance.ChangeState(GameState.CharacterMenu);
            DisablePlayerControls();
        }
        else if (SceneLoader.Instance.CurrentState == GameState.CharacterMenu)
        {
            SceneLoader.Instance.ChangeState(GameState.Playing);
            EnablePlayerControls();
        }
    }

    //missing case of null character. Also, it doesnt account for left and right (Q and E), check where to put this, maybe here.
    private void SwitchCharacter(int direction)
    {
        CharactersManager.Instance.SwitchCharacter(direction);
        camerafollow.FindPlayer();
    }

    //used to teast death, press P, need to be put on top.
    private void OnDeath()
    {
        characterBehavior.Death();
    }
    public bool IsAttackPressed()
    {
        // Use the Input System's context to check if the attack action is pressed. guardare meglio
        return controls.Player.Attack.triggered;
    }

    private void EnablePlayerControls()
    {
        controls.Player.Enable();
    }

    private void DisablePlayerControls()
    {
        controls.Player.Disable();
    }

    private void DebugTest()
    {
        DataManager.Instance.UpdatePlayerHealth(100);
    }
}













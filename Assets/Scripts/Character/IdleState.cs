using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : ICharacterState
{

    //this is to call the state machine PlayerInput
    private PlayerInput _playerInput;
    private CharacterBehavior _characterBehavior;



    public IdleState(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _characterBehavior = playerInput.GetComponent<CharacterBehavior>();
    }

    public void Enter()
    {
        //Debug.Log("IdleState");

        _characterBehavior.SetCharacterAnimation("idle", true);
        _characterBehavior.ResetVelocity();

        //-- WALK TRANSITION, occurs when walk button has been held since previous states, so that I dont have to time it right and I can 
        //just press the button and it will work.
        //CARE, this occurs after animation of idle, so when we instantly switch to walk, animation may look off. Maybe change
        Vector2 movement = _playerInput.controls.Player.Move.ReadValue<Vector2>();
        if (Mathf.Abs(movement.x) >= 0.01f) // Detect held input
        {
            _playerInput.SetState(new WalkState(_playerInput));
        }
        // Subscribe to input events
        _playerInput.controls.Player.Move.performed += OnMovePerformed;
        _playerInput.controls.Player.Move.canceled += OnMoveCanceled;
        _playerInput.controls.Player.Attack.started += OnAttackPerformed;
        _playerInput.controls.Player.Jump.started += OnJumpPerformed;
    }

    public void Execute()
    {
        // No input polling here, handled reactively through input events
    }

    public void FixedExecute()
    {
        // Handle physics-related behavior if needed
    }

    public void Exit()
    {
        // Unsubscribe from input events to prevent duplicate triggers
        _playerInput.controls.Player.Move.performed -= OnMovePerformed;
        _playerInput.controls.Player.Move.canceled -= OnMoveCanceled;
        _playerInput.controls.Player.Attack.started -= OnAttackPerformed;
        _playerInput.controls.Player.Jump.started -= OnJumpPerformed;
    }

    // Event handlers for input actions. No need for execute anymore as these are events and are called when needed, no need for update.
    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Vector2 movement = context.ReadValue<Vector2>();
        //if (Mathf.Abs(movement.x) >= 0.01f) // Transition to WalkState if horizontal input is detected.
        //{
            _playerInput.SetState(new WalkState(_playerInput));
        //}
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Optional: Handle stopping movement
    }

    private void OnAttackPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _playerInput.SetState(new AttackState(_playerInput)); // Transition to AttackState
    }

    private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _characterBehavior.jump = true;
        _playerInput.SetState(new JumpState(_playerInput)); // Transition to JumpState
    }
}







    //public void Enter()
    //{
    //    //check whether snap is needed. Use constants for the strings
    //    _characterBehavior.SetCharacterAnimation("animation", true);
    //    _characterBehavior.ResetVelocity();
    //}

    //public void Execute()
    //{
    //    //-- WALK TRANSITION
    //    if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f)
    //    {
    //        _playerInput.SetState(new WalkState(_playerInput));
    //    }

    //    //-- ATTACK TRANSITION
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        _playerInput.SetState(new AttackState(_playerInput));
    //    }

    //    //-- JUMP TRANSITION (guarda se è funzione giusta getkeydown)
        
    //    if (Input.GetButtonDown("Jump"))
    //    {

    //        _playerInput.SetState(new JumpState(_playerInput));
    //    }

    //}
    //public void FixedExecute()
    //{

    //}

    //public void Exit()
    //{
    //    // Code to execute when exiting the walk state

    //}


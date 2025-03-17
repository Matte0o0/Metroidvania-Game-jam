using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

//not using a bool variable to check for isWalking, as it's not a very sentitive input.
public class WalkState : ICharacterState
{
    private PlayerInput _playerInput;
    private CharacterBehavior _characterBehavior;
    private float _horizontalInput;


    public WalkState(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _characterBehavior = playerInput.GetComponent<CharacterBehavior>();
    }

    public void Enter()
    {
        //Debug.Log("WalkState");
        // Character starts walking
        _characterBehavior.SetCharacterAnimation("walk", true, true);

        // Subscribe to input events
        //_playerInput.controls.Player.Move.performed += OnMovePerformed;
        _playerInput.controls.Player.Move.canceled += OnMoveCanceled;
        _playerInput.controls.Player.Attack.started += OnAttackPerformed;
        _playerInput.controls.Player.Jump.started += OnJumpPerformed;

        //maybe not needed since it's on update now
        //Vector2 movement = _playerInput.controls.Player.Move.ReadValue<Vector2>();
        //_horizontalInput = movement.x;
    }

    //ATTENZIONE: ora logica è che ad ogni cambio di input nel movimento, si passi sempre per l'idle state. Nel caso destra e sinistra vengano
    //schiacciati assieme non so cosa succederebbe. Può essere che movimento si annulli passando ad idle ma il player stia ancora schiacciando
    //uno dei tasti del movimento. Boh
    public void Execute()
    {
        //_playerInput.controls.Player.Attack.ReadValue<float>();
        //if (_playerInput.controls.Player.Attack.ReadValue<float>() > 0)
        //{
        //    _playerInput.SetState(new AttackState(_playerInput));
        //}
    }

    public void FixedExecute()
    {
        Vector2 movement = _playerInput.controls.Player.Move.ReadValue<Vector2>();
        _horizontalInput = movement.x;
        _characterBehavior.Flip(_horizontalInput);
        _characterBehavior.MoveCharacter(_horizontalInput);
    }

    public void Exit()
    {
        // Unsubscribe from input events
        //_playerInput.controls.Player.Move.performed -= OnMovePerformed;
        _playerInput.controls.Player.Move.canceled -= OnMoveCanceled;
        _playerInput.controls.Player.Attack.started -= OnAttackPerformed;
        _playerInput.controls.Player.Jump.started -= OnJumpPerformed;
    }

    //evento del movimento, per ora è chiamato in idle state, forse serve anche qua
    //private void OnMovePerformed(InputAction.CallbackContext context)
    //{
    //    Debug.Log("move is performed");
    //    Vector2 movement = context.ReadValue<Vector2>();
    //    _horizontalInput = movement.x;
    //}

        
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        //--IDLE TRANSITION
        //_horizontalInput = 0;
        _playerInput.SetState(new IdleState(_playerInput));       
    }

    //-- ATTACK TRANSITION
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        _playerInput.SetState(new AttackState(_playerInput)); 
    }

    //-- JUMP TRANSITION
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("jump pressed from walk state");
        _characterBehavior.jump = true;
        _playerInput.SetState(new JumpState(_playerInput)); 
    }

    //public WalkState(PlayerInput playerInput)
    //{
    //    _playerInput = playerInput;
    //    _characterBehavior = playerInput.GetComponent<CharacterBehavior>();
    //}

    //public void Enter()
    //{
    //    // Character starts walking
    //    _characterBehavior.SetCharacterAnimation("walk 2", true, true);
    //}

    //public void Execute()
    //{

    //    _horizontalInput = Input.GetAxisRaw("Horizontal");

    //    //-- IDLE TRANSITION
    //    if (Mathf.Abs(_horizontalInput) <= 0.01f)
    //    {
    //        _playerInput.SetState(new IdleState(_playerInput));
    //    }
    //    //-- ATTACK TRANSITION
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        _playerInput.SetState(new AttackState(_playerInput));
    //    }
    //    //-- JUMP TRANSITION
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        _playerInput.SetState(new JumpState(_playerInput));
    //    }
    //}

    //public void FixedExecute()
    //{
    //    _characterBehavior.Flip(_horizontalInput);
    //    _characterBehavior.MoveCharacter(_horizontalInput);
    //}

    //public void Exit()
    //{
    //    // Cleanup code if needed
    //}
}

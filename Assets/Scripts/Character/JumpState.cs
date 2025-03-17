using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using System;


//regolare meglio salto con tutte le varie impostazioni e la gravità. manca double jump. vedere se velocità di salto e modo in cui cresce vada
//cambiata, vedi hollow knight.
public class JumpState : ICharacterState
{
    private PlayerInput _playerInput;
    private CharacterBehavior _characterBehavior;

    //jump force and holdjumpforce should probably be equal for everycharacter, so they could stay here or in the characterbehavior script.
    private float jumpForce = 10f;          // Base jump force
    private float holdJumpForce = 15f;      // Maximum force when holding jump (max reach of the jump)
    private float maxJumpTime = 0.2f;       // Time to reach maximum jump height
    private float jumpTimeCounter = 0f;     // Counter to track how long jump is held (at maxJumpTime, we will reach max force of jump)
    
    //private bool isJumping = false;         // Track if the player is currently jumping

    // Coyote time parameters
    private float coyoteTime = 0.1f; //gives you a certain amount of time (0,1secs) to jump after you fall off a platform
    private float coyoteTimeCounter; //counts time since you left platform, to check if you can still jump

    // Jump buffer parameters
    private float jumpBufferTime = 0.1f; //gives you a certain window of time (0,1secs) where the jump still works even if you havent landed yet
                                         //Jump will occur provided you land before the buffer time ends.
    private float jumpBufferCounter;

    private bool isGrounded;

    private float _horizontalInput;

    public JumpState(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _characterBehavior = playerInput.GetComponent<CharacterBehavior>();
    }

    public void Enter()
    {
        //Debug.Log("JumpState");
        jumpTimeCounter = 0f;
        //coyoteTimeCounter = _characterBehavior.Grounded() ? coyoteTime : 0f;
        jumpBufferCounter = jumpBufferTime;
        _playerInput.controls.Player.Jump.started += OnJumpPressed;
        _playerInput.controls.Player.Move.performed += OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 movement = _playerInput.controls.Player.Move.ReadValue<Vector2>();
        _horizontalInput = movement.x;
    }

    public void Execute()
    {
        // Update horizontal input for movement. This is the same as walking, maybe I could use an event for this
        // get horizontal input, check to properly make a specific movement while jumping
        //WARNING. se tengo premuto movimento dopo il salto, il player si muoverà a terra stando in jump state, e l'animazione della camminata non
        //partirà. Ve fatto un check ground o qualcosa di simile.

        //if (isGrounded)
        //{
        //    coyoteTimeCounter = coyoteTime; // Reset coyote time when grounded
        //}
        //else
        //{
        //    coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time counter
        //}
        //jumpBufferCounter -= Time.deltaTime;

        // Check if character is ascending or descending and play appropriate animation. cambiare animazioni. forse jump up si puo mettere in enter
        if (_characterBehavior.rb.velocity.y > 0)
        {
            // Ascending - play jump up animation
            _characterBehavior.SetCharacterAnimation("jump", false);
        }
        else if (_characterBehavior.rb.velocity.y < 0)
        {
            // Descending - play jump down animation
            _characterBehavior.SetCharacterAnimation("fall", true);
        }

        //check if the walk button is still being pressed, so that the player can move while jumping
        Vector2 movement = _playerInput.controls.Player.Move.ReadValue<Vector2>();
        _horizontalInput = movement.x;

        //-- ATTACK TRANSITION (now we are checking for grounded, but we can remove it to attack from the air. for now it's useless and you would
        //need to time the attack exactly on landing.)
        if (isGrounded && _playerInput.controls.Player.Attack.triggered && _characterBehavior.rb.velocity.y <= 0f)
        {
            _playerInput.SetState(new AttackState(_playerInput));
        }

        //-- WALK TRANSITION
        if (isGrounded && Mathf.Abs(_horizontalInput) > 0.01f && _characterBehavior.rb.velocity.y <= 0f)
        {
            _playerInput.SetState(new WalkState(_playerInput)); // Transition to WalkState
            return;
        }

        //-- IDLE TRANSITION
        if (isGrounded && _characterBehavior.rb.velocity.y <= 0f)
        {
            //Debug.Log("from jump to idle state");
            _playerInput.SetState(new IdleState(_playerInput));
        }
    }

    public void FixedExecute()
    {
        isGrounded = _characterBehavior.Grounded();

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time when grounded
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime; // Decrease coyote time counter
        }
        jumpBufferCounter -= Time.fixedDeltaTime;

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && _characterBehavior.jump == true)
        {
            _characterBehavior.jump = false;
            _characterBehavior.Jump();
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }


        _characterBehavior.MoveCharacter(_horizontalInput);
        _characterBehavior.Flip(_horizontalInput);
    }


    public void Exit()
    {
        //Debug.Log("JumpState Exit");
        _playerInput.controls.Player.Jump.started -= OnJumpPressed;
        _playerInput.controls.Player.Move.performed -= OnMovePerformed;
    }

    //this function is called only when the jump button is pressed on the air, not the first time (only to check buffer time)
    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        //UnityEngine.Debug.Log("Jump Pressed");
        jumpBufferCounter = jumpBufferTime;
        _characterBehavior.jump = true;
    }


}






using Spine.Unity;
using System.Collections;
using UnityEngine;

//differing from the other states, this one can't be interrupted by other states (FOR NOW), so there are no inputs to check for the state to chnge.
public class AttackState : ICharacterState
{
    private PlayerInput _playerInput;
    private CharacterBehavior _characterBehavior;
    private bool _isAttacking;

    ~AttackState()
    {
        //Debug.Log("Attackstate instance is being garbage collected.");
    }
    public AttackState(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _characterBehavior = playerInput.GetComponent<CharacterBehavior>();
        _isAttacking = false;

    }

    public void Enter()
    {
        //Debug.Log("AttackState");
        _characterBehavior.ResetVelocity();
        _characterBehavior.SetCharacterAnimation("attack", true, true);
        _characterBehavior.skeletonAnimation.AnimationState.Event += OnAttackAnimationEvent;
        _characterBehavior.skeletonAnimation.AnimationState.Complete += OnAttackAnimationComplete;
        _isAttacking = true;
    }

    public void Execute()
    {
    }

    public void FixedExecute()
    {
        // This is where attack logic happens (e.g., dealing damage)
    }

    public void Exit()
    {
        // Reset state on exit
        //Debug.Log("attack state exit");
        _characterBehavior.skeletonAnimation.AnimationState.Event -= OnAttackAnimationEvent;
        _characterBehavior.skeletonAnimation.AnimationState.Complete -= OnAttackAnimationComplete;

    }

    // Triggered by Spine events during the animation. Per ora non va.
    public void OnAttackAnimationEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        //momento in cui facciamo danni non è subito quando parte attacco, ma quando colpisce. Per cui mettiamo evento "hit" negli eventi di spine
        //cosi da avere il timing giusto. vedere più avanti se ha senso farlo o se attacchi sono troppo veloci.
        //if (e.Data.Name == "hit") // Assuming "hit" is the event name set in Spine for when the attack connects
        //{
        //    _characterBehavior.Attack(); // Execute attack logic (e.g., dealing damage)
        //}
        _characterBehavior.Attack();
    }

    // Triggered when the current attack animation completes
    public void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
    {
        //if (trackEntry.Animation.Name == "attack")
        //{
        //    // Reset to idle state at the end of the attack animation
        //    _playerInput.SetState(new IdleState(_playerInput));
        //    _isAttacking = false;
        //}
        _characterBehavior.Attack();
        Vector2 movement = _playerInput.controls.Player.Move.ReadValue<Vector2>();
        if (Mathf.Abs(movement.x) >= 0.01f) // Detect held input
        {
            _playerInput.SetState(new WalkState(_playerInput));
        }
        else
        {
            _playerInput.SetState(new IdleState(_playerInput)); // Otherwise, return to idle
        }
    }
}

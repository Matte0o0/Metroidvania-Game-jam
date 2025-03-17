using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyDeathState : IAIState
{
    private EnemyController _enemyController;
    private EnemyBehavior _enemyBehavior;
    private SkeletonAnimation _skeletonAnimation;


    public EnemyDeathState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyBehavior = _enemyController.GetComponent<EnemyBehavior>();
        _skeletonAnimation = _enemyController.GetComponent<SkeletonAnimation>();
    }
    // Start is called before the first frame update
    public void Enter()
    {
        _enemyBehavior.ResetVelocity();
        Debug.Log("Enemy has died");

        _enemyBehavior.SetCharacterAnimation("attack", false);
        _enemyBehavior.skeletonAnimation.AnimationState.Event += OnAttackAnimationEvent;
        _enemyBehavior.skeletonAnimation.AnimationState.Complete += OnAttackAnimationComplete;
    }

    // Update is called once per frame
    public void Execute()
    {

    }

    public void FixedExecute()
    {

    }

    public void Exit()
    {
        _enemyBehavior.skeletonAnimation.AnimationState.Event -= OnAttackAnimationEvent;
        _enemyBehavior.skeletonAnimation.AnimationState.Complete -= OnAttackAnimationComplete;
    }

    //empty for now, just in case we need to do something when the death animation plays.
    public void OnAttackAnimationEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {

    }
    public void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
    {
        GameObject.Destroy(_enemyController.gameObject);
    }
}

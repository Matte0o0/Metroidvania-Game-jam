using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//Problema. Playertransform non trovato, fixare con referenceManager.cs. 
public class EnemyIdleState : IAIState
{
    private EnemyController _enemyController;
    private EnemyBehavior _enemyBehavior;
    public float distanceToPlayer;
    private Transform playerTransform;

    public float chaseDistance = 3f;


    public EnemyIdleState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyBehavior = _enemyController.GetComponent<EnemyBehavior>();
    }

    public void Enter()
    {
        //_enemyBehavior.SetCharacterAnimation("idle", true);
        _enemyBehavior.ResetVelocity();

        //this is to fix async between update and fixedupdate when enemy tries to get the reference to the transform of the player, andrebbe cambiato credo.
        playerTransform = CharactersManager.Instance.GetCurrentCharacterTransform();
        distanceToPlayer = Vector2.Distance(playerTransform.position, _enemyController.transform.position);
    }

    public void Execute()
    {

        //--PATROL TRANSITION, TEMPORARY
        //if (Time.time > _enemyController.idleStartTime + _enemyController.idleDuration)
        //{
        //    _enemyController.SetState(new EnemyPatrolState(_enemyController));
        //}

        //--CHASE TRANSITION, VA BENE, RIMETTERE
        //if (distanceToPlayer < chaseDistance)
        //{
        //    _enemyController.SetState(new EnemyChaseState(_enemyController));
        //}

    }

    public void FixedExecute()
    {
        playerTransform = CharactersManager.Instance.GetCurrentCharacterTransform();
        distanceToPlayer = Vector2.Distance(playerTransform.position, _enemyController.transform.position);

    }

    public void Exit()
    {
        // Code to execute when exiting the idle state

    }
}

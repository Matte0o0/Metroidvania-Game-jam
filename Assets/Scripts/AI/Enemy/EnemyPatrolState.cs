using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : IAIState
{
    private EnemyController _enemyController;
    private EnemyBehavior _enemyBehavior;


    public EnemyPatrolState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyBehavior = _enemyController.GetComponent<EnemyBehavior>();
    }

    public void Enter()
    {
        _enemyBehavior.SetCharacterAnimation("walk", true);
        Debug.Log("AI entered patrol");
    }

    public void Execute()
    {

    }

    public void FixedExecute()
    {
        // Move the character towards the current patrol target
        _enemyBehavior.MoveCharacter(_enemyController.currentPatrolTarget);


        // Check if the AI has reached the current patrol target
        if (Vector2.Distance(_enemyController.transform.position, _enemyController.currentPatrolTarget) < 0.1f)
        {
            // Update the patrol target to the next point
            _enemyController.currentPatrolTarget = _enemyController.GetNextPatrolPoint();
        }
    }
    
    public void Exit()
    {

    }
}

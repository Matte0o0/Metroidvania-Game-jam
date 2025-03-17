using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script handles the recoil of the enemy when it gets hit and other effects. For now enemy enters this state anytime player lands his attack.
//I need to decide whether to call here the function reducing the health of the characher or in enemycontroller (for now it's in enemy controller).
public class EnemyGetHitState : IAIState
{
    private EnemyController _enemyController;
    private EnemyBehavior _enemyBehavior;
    private float _timer;

    public EnemyGetHitState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyBehavior = _enemyController.GetComponent<EnemyBehavior>();
    }
    // Start is called before the first frame update
    public void Enter()
    {
        _enemyBehavior.ResetVelocity();
        Debug.Log("enemy is hit");
        _timer = 2f;
    }

    // Update is called once per frame
    public void Execute()
    {
        
        _timer -= Time.deltaTime;

        //-- TRANSITION TO IDLE STATE (TEMPORARY)
        if (_timer <= 0)
        {
            _enemyController.SetState(new EnemyIdleState(_enemyController));
        }
       
    }

    public void FixedExecute()
    {
        
    }

    public void Exit()
    {
        // Code to execute when exiting the idle state

    }

}

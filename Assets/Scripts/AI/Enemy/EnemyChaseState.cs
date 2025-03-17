using UnityEngine;

public class EnemyChaseState : IAIState
{
    private EnemyController _enemyController;
    private EnemyBehavior _enemyBehavior;

    public EnemyChaseState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyBehavior = _enemyController.GetComponent<EnemyBehavior>();
    }

    public void Enter()
    {
        //_enemyBehavior.SetCharacterAnimation("walk", true);
        Debug.Log("AI entered chase");
    }

    public void Execute()
    {

    }

    public void FixedExecute()
    {
        Transform playerTransform = CharactersManager.Instance.GetCurrentCharacterTransform();
        Vector2 playerPosition = playerTransform.position;
        _enemyBehavior.MoveCharacter(playerPosition);

        // Example: Transition back to idle if the player is too far
        //float distanceToPlayer = Vector3.Distance(_enemyController.playerTransform.position, _enemyController.transform.position);

        //if (Time.time > _aiController.idleStartTime + _aiController.idleDuration)
        //{
        //    _aiController.SetState(new AiIdleState(_aiController));
        //}
    }

    public void Exit()
    {
        // Code to execute when exiting the chase state
    }
}

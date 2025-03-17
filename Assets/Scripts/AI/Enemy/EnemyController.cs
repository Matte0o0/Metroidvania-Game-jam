using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


//enemy controller is similar to playerInput. It sets the initial state of any enemy character (idle), so it will be applied to any enemy type.
//This is basically managing the state machine of the enemy. 
public class EnemyController: MonoBehaviour
{
    public Transform playerTransform;
    public IAIState currentState;

    private Transform patrolCenter;
    [SerializeField] public float patrolDistance = 5f;
    private Vector2 leftPatrolPoint;
    private Vector2 rightPatrolPoint;
    public Vector2 currentPatrolTarget;

   
    private EnemyBehavior enemyBehavior;

    [SerializeField] public float idleDuration = 3f; // Duration to stay in idle state
    [HideInInspector] public float idleStartTime; // Time when idle state started

    void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        //playerTransform = ReferenceManager.Instance.Get<Transform>("PlayerTransform");


        GameObject patrolCenterObject = GameObject.Find("Patrol Center");
        patrolCenter = patrolCenterObject.transform;
        leftPatrolPoint = new Vector2(patrolCenter.position.x - patrolDistance, patrolCenter.position.y);
        rightPatrolPoint = new Vector2(patrolCenter.position.x + patrolDistance, patrolCenter.position.y);
        currentPatrolTarget = rightPatrolPoint;

        SetState(new EnemyIdleState(this));
    }

    void OnEnable()
    {
        //IMPORTANTE
        //this gets the player transform every time we switch characters. per ora non serve dato da updatiamo il transform in ogni update negli stati.
        //magari è da cambiare e centralizzare qui, cosi che abbiamo una reference sola senza doverla updatare in ogni stato.
        //ReferenceManager.Instance.OnPlayerTransformChanged += UpdatePlayerTransform;
        //playerTransform = ReferenceManager.Instance.Get<Transform>("PlayerTransform");
        //SetState(new EnemyIdleState(this));
    }
    private void OnDisable()
    {
        ReferenceManager.Instance.OnPlayerTransformChanged -= UpdatePlayerTransform;
    }

    private void UpdatePlayerTransform(Transform newPlayerTransform)
    {
        playerTransform = newPlayerTransform;
    }
    void Update()
    {
        currentState.Execute();
    }

    private void FixedUpdate()
    {
        currentState.FixedExecute();
    }

    public void SetState(IAIState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    //when player attack lands on enemy, this function is called. It changes the state of the enemy to get hit, overriding its current state.
    //CARE: this is the first time we change state from the state machine and not from the actual states. This means this state OVERRIDES ANY OTHER
    public void TakeDamage(float damage)
    {
        enemyBehavior.GetHit(damage); // Let EnemyBehavior handle health reduction

        if (enemyBehavior.health <= 0)
        {
            SetState(new EnemyDeathState(this)); // Enemy dies if health reaches 0
        }
        else
        {
            SetState(new EnemyGetHitState(this)); // Always transition from current state to Hurt
        }
    }

    //maybe should move it??
    public Vector2 GetNextPatrolPoint()
    {
        if (currentPatrolTarget == rightPatrolPoint)
        {
            currentPatrolTarget = leftPatrolPoint;
        }
        else
        {
            currentPatrolTarget = rightPatrolPoint;
        }
        return currentPatrolTarget;
    }


}


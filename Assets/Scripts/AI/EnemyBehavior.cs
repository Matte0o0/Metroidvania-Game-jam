using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//defines general behavior of enemy. Similar to character behavior. 
//ATTENTION: all stats should go in an enemy specific script, so that we may change stats for different enemies.
public class EnemyBehavior : MonoBehaviour
{

    public Rigidbody2D rb;
    public SkeletonAnimation skeletonAnimation;
    private string currentAnimation = "animation"; // Default animation


    public float health = 100;

    [Header("Horizontal Movement Settings")]
    [SerializeField] public float speed = 5f;

    public float smoothFactor = 10f;
    private Vector2 moveVelocity;
    [Space(5)]

    [Header("Vertical Movement Settings")]
    private float jumpForce = 10f;


    [Space(5)]


    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private LayerMask whatIsGround;
    [Space(5)]


    [Header("Attacking")]
    bool isAttacking = false;
    //maybe timebetweenattack should have a value???????
    float timeBetweenAttack = 0.01f, timeSinceAttack;
    [SerializeField] Transform sideAttackTransform;
    [SerializeField] Vector2 sideAttackArea;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] float damage;
    public float attackCooldown;
    public bool isAttackOnCooldown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        //check maybe it's not needed.
        //rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        skeletonAnimation = GetComponent<SkeletonAnimation>();

        var attackAnimation = skeletonAnimation.Skeleton.Data.FindAnimation("attack");
        if (attackAnimation != null)
        {
            attackCooldown = attackAnimation.Duration;
        }

    }

    //to see attack hitbox of character
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
    }

    //movement for player input. It stops when attacking
    //public void MoveCharacter(float moveInput)
    //{
    //    //if (isAttacking)
    //    //{
    //    //    return;
    //    //}
    //    //1. Basic movement
    //    rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

    //    //2. this should be a smoother movement than normal. Maybe use for AI as well?
    //    //moveVelocity = new Vector2(moveInput * speed, rb.velocity.y);
    //    //rb.velocity = Vector2.Lerp(rb.velocity, moveVelocity, Time.deltaTime * smoothFactor);
    //}


    //movement for AI, for patrolling. It doesnt stop when attacking, maybe need to add?
    public void MoveCharacter(Vector2 targetPosition)
    {
        // Calculate the direction towards the target position
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        // Calculate the movement input based on the direction and speed
        float moveInput = direction.x * speed;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        HandleMovementAnimation(moveInput);
    }

    public void HandleMovementAnimation(float moveInput)
    {
        if (Mathf.Abs(moveInput) > 0.01f) // check if character is moving, if it is, set the walk animation
        {
            // If there is movement, set the walk animation
            SetCharacterAnimation("walk 2", true);
            Flip(moveInput);
        }
        else
        {
            // If there is no movement input, set the idle animation, but only if the current animation is not already idle
            Idle();
        }
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }
    public void Flip(float moveInput)
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void Idle()
    {
        if (currentAnimation != "animation")
        {
            SetCharacterAnimation("animation", true);
        }
    }

    public bool Grounded()
    {
        //casts 3 rays, one from center, one from right and one from left of the player object. This means that the player can be partially off the
        //ground and still be considered grounded
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void JumpBlock()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce);
    }

    public void Attack()
    {
        //if (isAttackOnCooldown)
        //{
        //    return; 
        //}

        //isAttacking = true;
        //isAttackOnCooldown = true;
        //SetCharacterAnimation("attack", false);

        //stops movement
        Hit(sideAttackTransform, sideAttackArea);

        // Subscribe to the Complete event to know when the animation finishes
        //skeletonAnimation.AnimationState.Complete += OnAttackAnimationComplete;

        //StartCoroutine(AttackCooldownCoroutine());
    }


    public void Hit(Transform _attackTransfom, Vector2 _attackArea)
    {
        //gets enemies that are inside collider area of the player
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransfom.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {

        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<CharacterBehavior>() != null)
            {
                objectsToHit[i].GetComponent<CharacterBehavior>().GetHit(damage);
            }
        }
    }

    public void GetHit(float _damageDone)
    {
        health -= _damageDone;
        Debug.Log("Health: " + health);    
    }

    //non serve se gestiamo tutto nei vari stati della state machine
    public void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SetCharacterAnimation(string animationName, bool loop, bool snap = false, float speed = 1f)
    {
        // Check if the current animation is different from the new one
        if (currentAnimation != animationName)
        {
            var trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);

            // Set the speed of the animation
            trackEntry.TimeScale = speed;

            // If snap is true, set the MixDuration to 0 to snap the animation instantly. Otherwise SetAnimation is called normally
            if (snap)
            {
                trackEntry.MixDuration = 0;
            }

            currentAnimation = animationName;
        }
    }



}




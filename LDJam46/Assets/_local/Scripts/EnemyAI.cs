using System;
using System.Collections;
using System.Collections.Generic;
using _local.Scripts;
using MyNamespace;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

namespace firewalk
{


    public class EnemyAI : MonoBehaviour
    {

        [Header("Settings")]
        [Tooltip("Red circle. Without torch")]
        public float playerDetectionDistanceWithoutTorch = 8f;
        [Tooltip("White circle.With lit torch")]
        public float playerDetectionDistanceWithTorchLit = 10f;
        public float walkingSpeed = 1.0f;
        
        [Tooltip("Yellow circle. Tune this to actual attack range trigger collider or less")]
        public float attackRange = 1.0f;
        
        // public bool changeVelocity;
        
        public float attackTimeOut = 0.5f;
        public float minPatrolDistance = 5f;
        [Tooltip("Blue circle. Tune this to actual attack range trigger collider or less")]
        public float maxPatrolDistance = 15f;
        public float patrolPointReachAccuracy = 1f;
        public float stuckTimeOutSeconds = 10f;
        
        
        public EnemyState state { get; private set; }

        [Header("Links to scene objects")]
        public Animator characterAnimator;
        
        public SpriteRenderer frontSprite;
        public SpriteRenderer backSprite;
        public SpriteRenderer attackSprite;
        public ShadowCaster2D shadowRight;
        public ShadowCaster2D shadowLeft;

        // private Vector3 shadowOriginalScale;

        private Rigidbody2D rb;

        private Vector2 moveDirection;
        private float timeBeforeEvasion;
        private float timeBeforeAttack;
        private float timeBeforeConsideringStuck;

        private PlayerController player;
        private LivingBeing playerLivingBeing;

        private Vector2 lastPatrolPoint;
        private Vector2 spawnPoint;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            
            state = EnemyState.Idle;
            
            
            player = PlayerController.instance;
            playerLivingBeing = player.GetComponent<LivingBeing>();
            
            //save spawn point, it will be used to return after patrolling
            spawnPoint = transform.position;
            lastPatrolPoint = Vector2.zero;
            
            // shadowOriginalScale = shadow.transform.localScale;
        }

        private void Update()
        {
            AdvanceTimers();
        }

        void AdvanceTimers()
        {

            if (timeBeforeAttack > 0)
            {
                timeBeforeAttack -= Time.deltaTime;
            }
            
            if (timeBeforeConsideringStuck > 0)
            {
                timeBeforeConsideringStuck -= Time.deltaTime;
            }
            
            //TODO: stuck timer for situations when stuck in obstacles?
        }

        void FixedUpdate()
        {
            if (PlayerInAttackRange && playerLivingBeing.IsAlive())
            {
                TryToAttack();
                state = EnemyState.Attacking;
            }
            else if (SeesPlayer  && playerLivingBeing.IsAlive())
            {
                MoveTowardsPoint(player.transform.position);
                state = EnemyState.Chasing;
                
                //reset patrol point to randomize movement whan enemy looses playerr
                lastPatrolPoint = Vector2.zero;
            }
            else if (lastPatrolPoint == Vector2.zero)
            {
                //special case - enemy has not yet moved to any patrol point, so select a new one
                SelectNewPatrolPoint();
                MoveTowardsPoint(lastPatrolPoint);
                state = EnemyState.Patrolling;
            }
            else if (ReachedPatrolPoint)
            {
                //reached patrol point, select new one
                SelectNewPatrolPoint();
                MoveTowardsPoint(lastPatrolPoint);
                state = EnemyState.Patrolling;
            }
            else
            {
                if (timeBeforeConsideringStuck < 0)
                {
                    //consider stuck, find another patrol point
                    SelectNewPatrolPoint();
                }
                
                //still moving towards patrol point
                MoveTowardsPoint(lastPatrolPoint);
                state = EnemyState.Patrolling;
            }

            UpdateAnimationDirection();
;            //TODO: refactor to reduce if-else
                
        }

        /*
         * Checks attack timeout (cooldown) and attacks if it can
         */
        void TryToAttack()
        {
            if (timeBeforeAttack <= 0)
            {
                Attack();
                timeBeforeAttack = attackTimeOut;
            }
            
        }

        void Attack()
        {
            print("Enemy attacks");
            characterAnimator.SetTrigger("attack");
        }
        

        /*
         * Must be called from animation
         */
        public void OnEvasionEnded()
        {
            //just here so the animator won't crash if shared with player
        }
        
        public bool SeesPlayer
        {
            get
            {

                float distanceToDetect;

                if (player.torchIsLit)
                {
                    distanceToDetect = playerDetectionDistanceWithTorchLit;
                }
                else
                {
                    distanceToDetect = playerDetectionDistanceWithoutTorch;
                }
                
                bool withinDistance = Vector2.Distance(transform.position, player.transform.position) <= distanceToDetect;


                if (withinDistance)
                {
                    //cast a ray to player and see if it hits wall or player first
                    // Vector2 dir = (player.transform.position - transform.position).normalized;
                    //
                    // int layerMask = LayerMask.GetMask("Player", "Obstacle");
                    //
                    // if (Physics2D.Raycast(transform.position, dir, Mathf.Infinity, layerMask).collider.tag == "Player")
                    // {
                    //     return true;
                    // }
                    
                    //not casting the ray, we assume player is always seem
                    //TODO: cast the ray if layer does not have lit torch?
                    //TODO: reduce detection range if player does not have lit torch?
                    
                    return true;
                }

                return false;
            }
        }

        /*
         * Sets random patrol point around spawn location
         */
        public void SelectNewPatrolPoint()
        {

            int xSign = Random.value > 0.5 ? 1 : -1;
            int ySign = Random.value > 0.5 ? 1 : -1;
            
            lastPatrolPoint = new Vector2(spawnPoint.x + xSign * Random.Range(minPatrolDistance, maxPatrolDistance),
                spawnPoint.y + ySign*Random.Range(minPatrolDistance, maxPatrolDistance));
            
            //reset stuck timer
            timeBeforeConsideringStuck = stuckTimeOutSeconds;

        }
        
        public bool ReachedPatrolPoint
        {
            get
            {
                bool withinDistance = Vector2.Distance(transform.position, lastPatrolPoint) <= patrolPointReachAccuracy;
                return withinDistance;
            }
        }
        
        public bool PlayerInAttackRange
        {
            get
            {
                return Vector2.Distance(transform.position, player.transform.position) <= attackRange;
            }
        }
        
        public void MoveTowardsPoint(Vector2 targetPoint)
        {
            moveDirection = (targetPoint - (Vector2)transform.position).normalized;
            rb.velocity = moveDirection * walkingSpeed;
            //TODO: change to vector 2?
            
            characterAnimator.SetBool("moving", true);
        }
        
        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            // to show attack range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerDetectionDistanceWithoutTorch);
            
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, playerDetectionDistanceWithTorchLit);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxPatrolDistance);
        }
        
        public void StopMovement()
        {
            rb.velocity=Vector2.zero;
        }
        
        public void UpdateAnimationDirection()
        {
            if (moveDirection.x>0)
            {
                frontSprite.flipX = false;
                backSprite.flipX = false;
                attackSprite.flipX = false;
                
                shadowLeft.castsShadows=false;
                shadowRight.castsShadows=true;
                
            }
            else if (moveDirection.x < 0) //we don;t update it if it's 0 to face the same direction he walked
            {
                frontSprite.flipX = true;
                backSprite.flipX = true;
                attackSprite.flipX = true;

                shadowLeft.castsShadows=true;
                shadowRight.castsShadows=false;
            }

            if (moveDirection.y > 0)
            {
                frontSprite.enabled = false;
                backSprite.enabled = true;
            }
            else if (moveDirection.y < 0)
            {
                frontSprite.enabled = true;
                backSprite.enabled = false;
            }
        }

    }

    public enum EnemyState
    {
        Idle = 1,
        Patrolling = 2,
        Chasing = 3,
        Attacking = 4
    }

}
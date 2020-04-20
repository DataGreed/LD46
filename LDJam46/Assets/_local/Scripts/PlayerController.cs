using System;
using System.Collections;
using System.Collections.Generic;
using _local.Scripts;
using UnityEngine;

namespace MyNamespace
{


    public class PlayerController : MonoBehaviour
    {

        public static PlayerController instance;
        
        private float speed = 1.0f;
        
        public float walkingSpeed = 1.0f;
        public float evasionSpeed = 3.0f;
        
        public bool changeVelocity;
        public float evasionTimeOut = 0.5f;
        public float attackTimeOut = 0.3f;

        public float torchMaxBurnSeconds = 40;
        
        public PlayerState state { get; private set; }

        [Header("Links to scene objects")] 
        public GameObject ambientLight;
        public GameObject torchLight;
        public Animator characterAnimator;
        public ParticleSystem footPrintsEmitter;
        public Fading2DLight torch;

        public SpriteRenderer frontSprite;
        public SpriteRenderer backSprite;
        public SpriteRenderer attackSprite;
        
        private Rigidbody2D rb;
        private Inventory _inventory;

        private Vector2 moveDirection;
        private float timeBeforeEvasion;
        private float timeBeforeAttack;


        private void Awake()
        {
            //save instance so enemies can easily refer to it
            instance = this;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _inventory = GetComponent<Inventory>();

            //from the start the torch is not lit
            ambientLight.SetActive(true);
            torchLight.SetActive(false);

            state = PlayerState.Idle;
            speed = walkingSpeed;

            footPrintsEmitter.enableEmission = false;
        }

        private void Update()
        {
            AdvanceTimers();

            //input processing
            // moveDirection.x = Input.GetAxis("Horizontal");
            // moveDirection.y = Input.GetAxis("Vertical");
            
            moveDirection.x = Input.GetAxisRaw("Horizontal");
            moveDirection.y = Input.GetAxisRaw("Vertical");

            //TODO: normalize?

            //fire if the fire button is pressed
            if (Input.GetButtonDown("Fire1"))
            {
                if (timeBeforeAttack<=0)
                {
                    Attack();
                    timeBeforeAttack = attackTimeOut;
                }
                
            }

            //FIXME: should only react on pressing once, not holding 
            //fire if the fire button is pressed
            if (Input.GetButtonDown("Fire2"))
            {
                if (moveDirection != Vector2.zero)
                {
                    //evasion needs direction

                    if (timeBeforeEvasion <= 0)
                    {
                        Evade();
                        timeBeforeEvasion = evasionTimeOut;
                    }
                }
            }

            if (Input.GetButtonDown("Fire3"))
            {
                LightTorch();
            }
        }

        void AdvanceTimers()
        {
            if (timeBeforeEvasion > 0)
            {
                timeBeforeEvasion -= Time.deltaTime;
            }

            if (timeBeforeAttack > 0)
            {
                timeBeforeAttack -= Time.deltaTime;
            }
            
            //TODO: burn the torch
            //TODO: torch burndown animation
        }

        void FixedUpdate()
        {
            if (state != PlayerState.Evading)
            {
                
                
                // update speed based on movement input
                if (changeVelocity)
                {
                    rb.velocity = moveDirection * speed;
                }
                else
                {
                    rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
                }


                //enable/disable running animation
                // we compare to epsilon, not to zero, so we don't get any floating points errors
                // if (System.Math.Abs(moveDirection.x) > Mathf.Epsilon ||
                //     System.Math.Abs(moveDirection.y) > Mathf.Epsilon)
                if(moveDirection!=Vector2.zero)
                {
                    characterAnimator.SetBool("moving", true);

                    var main = footPrintsEmitter.main;
                    var angle = Mathf.PI/2 + Mathf.Atan2(-moveDirection.normalized.y, moveDirection.normalized.x);// * Mathf.Rad2Deg;
                    main.startRotation = angle;    //this has to be set in radians apparantly

                    footPrintsEmitter.enableEmission = true;
                    
                }
                else
                {
                    characterAnimator.SetBool("moving", false);
                    footPrintsEmitter.enableEmission = false;

                }
            }
            
            UpdateAnimationDirection();
        }

        void Attack()
        {
            print("Player attacks");
            characterAnimator.SetTrigger("attack");
        }

        private void Evade()
        {
            state = PlayerState.Evading;
            print("Player evades");
            // speed = evasionSpeed;
            characterAnimator.SetTrigger("evade");
            
            //stop from walking
            rb.velocity = Vector2.zero;
            
            //add explosive force to evasion
            // rb.AddForce(moveDirection.normalized * evasionSpeed, ForceMode2D.Impulse);
            print(moveDirection.normalized);
            rb.velocity = moveDirection.normalized * evasionSpeed;
            footPrintsEmitter.enableEmission = false;
        }

        /*
         * Must be called from animation
         */
        public void OnEvasionEnded()
        {
            state = PlayerState.Idle;
            speed = walkingSpeed;
            print("Evade Ended; speed reset");
        }

        private void LightTorch()
        {
            // print("Trying to light torch");
            if (_inventory.woodCarrying > 0)
            {
                //TODO: check if torch not burning
                
                _inventory.BurnOneWood();
                ambientLight.SetActive(false);
                torchLight.SetActive(true);
                torch.Reignite();
                print("Made a torch");
                HUDController.Alert("Used 1 piece of wood to make a torch");
            }
            else
            {
                print("Not enough wood to make a torch");
                HUDController.Alert("Not enough wood to make a torch");
            }
        }

        private void ExtinguishTorch()
        {
            print("Extinguishing torch");
            ambientLight.SetActive(true);
            torchLight.SetActive(false);
        }

        public bool torchIsLit
        {
            get
            {
                return torchLight.activeInHierarchy;
            }
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
            }
            else if (moveDirection.x < 0) //we don;t update it if it's 0 to face the same direction he walked
            {
                frontSprite.flipX = true;
                backSprite.flipX = true;
                attackSprite.flipX = true;
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

    public enum PlayerState
    {
        Idle = 1,
        Walking = 2,
        Evading = 3,
        Attacking = 4
    }

}
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{


    public class PlayerController : MonoBehaviour
    {

        private float speed = 1.0f;
        
        public float walkingSpeed = 1.0f;
        public float evasionSpeed = 3.0f;
        
        public bool changeVelocity;
        public float evasionTimeOut = 0.5f;
        public float attackTimeOut = 0.3f;
        
        
        public PlayerState state { get; private set; }

        [Header("Links to scene objects")] public GameObject ambientLight;
        public GameObject torchLight;

        private Rigidbody2D rb;

        private Vector2 moveDirection;
        private float timeBeforeEvasion;
        private float timeBeforeAttack;


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            //from the start the torch is not lit
            ambientLight.SetActive(true);
            torchLight.SetActive(false);

            state = PlayerState.Idle;
        }

        private void Update()
        {
            AdvanceTimers();

            //input processing
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.y = Input.GetAxis("Vertical");

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
                if (timeBeforeEvasion <= 0)
                {
                    Evade();
                    timeBeforeEvasion = evasionTimeOut;
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
                if (System.Math.Abs(moveDirection.x) > Mathf.Epsilon ||
                    System.Math.Abs(moveDirection.y) > Mathf.Epsilon)
                {

                }
                else
                {

                }
            }
        }

        void Attack()
        {
            print("Player attacks");
        }

        private void Evade()
        {
            print("Player evades");
            speed = evasionSpeed;
        }

        /*
         * Must be called from animation
         */
        public void OnEvasionEnded()
        {
            state = PlayerState.Idle;
            speed = walkingSpeed;
        }

        private void LightTorch()
        {
            print("Trying to light torch");
            ambientLight.SetActive(false);
            torchLight.SetActive(true);
        }

        private void ExtinguishTorch()
        {
            print("Extinguishing torch");
            ambientLight.SetActive(true);
            torchLight.SetActive(false);
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
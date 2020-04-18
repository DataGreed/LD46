using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 1.0f;
    public bool changeVelocity;
    
    private Rigidbody2D rb;

    private Vector2 moveDirection;

    [Header("Links to scene objects")]
    public GameObject ambientLight;
    public GameObject torchLight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        
        //from the start the torch is not lit
        ambientLight.SetActive(true);
        torchLight.SetActive(false);
    }

    private void Update()
    {
        //input processing
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");
        
        //TODO: normalize?
        
        //fire if the fire button is pressed
        if(Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
        
        //FIXME: should only react on pressing once, not holding 
        //fire if the fire button is pressed
        if(Input.GetButtonDown("Fire2"))
        {
            Evade();
        }
        
        if(Input.GetButtonDown("Fire3"))
        {
            LightTorch();
        }
    }

    void FixedUpdate()
    {
        // update speed based on movement input
        if (changeVelocity)
        {
            rb.velocity = moveDirection * speed;
        }
        else
        {
            rb.MovePosition(rb.position + moveDirection * speed  * Time.fixedDeltaTime);
        }

        
        //enable/disable running animation
        // we compare to epsilon, not to zero, so we don't get any floating points errors
        if (System.Math.Abs(moveDirection.x) > Mathf.Epsilon || System.Math.Abs(moveDirection.y) > Mathf.Epsilon)
        {
            
        }
        else
        {
            
        }
    }

    void Attack()
    {
        print("Player attacks");
    }
    
    private void Evade()
    {
        print("Player evades");
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

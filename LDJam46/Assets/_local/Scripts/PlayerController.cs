using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 1.0f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void FixedUpdate()
    {
       

        // update speed based on movement input
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rb.velocity = moveDirection * speed;

        
        

        // //fire if the fire button is pressed
        // if(Input.GetButton("Fire1"))
        // {
        //     Attack();
        // }
        

        //enable/disable running animation
        // we compare to epsilon, not to zero, so we don't get any floating points errors
        if (System.Math.Abs(moveDirection.x) > Mathf.Epsilon || System.Math.Abs(moveDirection.y) > Mathf.Epsilon)
        {
            
        }
        else
        {
            
        }
    }
    

}

using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    float xVel = 0;
    float yVel = 0;

    public float sideSpeed = 3;
    public float jumpSpeed = 10;
    public float fastFallSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float velLeft = 0;
        if (Input.GetAxis("Horizontal") < 0)
        {
            velLeft = -sideSpeed;
        }

        float velRight = 0;
        if (Input.GetAxis("Horizontal") > 0)
        {
            velRight = sideSpeed;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            yVel = jumpSpeed;
        }
        else if (rigidbody.velocity.y < 0 && Input.GetAxis("Vertical") < 0)
        {
            yVel = rigidbody.velocity.y - fastFallSpeed;
        }
        else{
            yVel = rigidbody.velocity.y;
        }

        

        xVel = velLeft + velRight;
        
        rigidbody.velocity = new Vector3(xVel, yVel, 0);
    }

    private void GetLeft(){
        //return Input.GetKey(KeyCode.A) || Input.GetAxis()
    }
}


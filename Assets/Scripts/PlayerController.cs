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
    public bool touchingGround = false;
    bool wasTouchingGround = false;
    bool releasedUp = true;

    public int doubleJump = 1;
    public bool fastFell = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        touchingGround = true;
        doubleJump = 1;
    }

    private void OnCollisionExit(Collision other) {
        touchingGround = false;
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


        yVel = rigidbody.velocity.y;
        if (touchingGround)
        {
            fastFell = false;
            if (Input.GetAxis("Jump") > 0 && releasedUp)
            {
                yVel = jumpSpeed;
                releasedUp = false;
            }
            else if (Input.GetAxis("Jump") <= 0){
                releasedUp = true;
            }
        }
        else
        {
            if (Input.GetAxis("Jump") > 0 && releasedUp && doubleJump > 0)
            {
                doubleJump--;
                yVel = jumpSpeed;
                releasedUp = false;
            }
            else if (Input.GetAxis("Jump") <= 0){
                releasedUp = true;
            }

            if (rigidbody.velocity.y < 0 && Input.GetAxis("Vertical") < 0 && !fastFell){
                yVel = -fastFallSpeed;
                fastFell = true;
            }
        }

        xVel = velLeft + velRight;
        
        rigidbody.velocity = new Vector3(xVel, yVel, 0);
    }

    private void GetLeft(){
        //return Input.GetKey(KeyCode.A) || Input.GetAxis()
    }
}


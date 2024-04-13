using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject attack;
    new Rigidbody rigidbody;
    float xVel = 0;
    float yVel = 0;

    public float sideSpeed = 3;
    public float jumpSpeed = 10;
    public float fastFallSpeed = 5;
    public bool touchingGround = false;
    bool releasedUp = true;
    bool attackOnCooldown = false;

    public int doubleJump = 1;
    public bool fastFell = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy"){
            //Invicibility period
            if (!other.gameObject.GetComponent<Enemy>().dead){
                //Debug.Log("Player Hit");
            }
            
        }
        else{
            touchingGround = true;
            doubleJump = 1;
        }
        
    }

    private void OnCollisionExit(Collision other) {
        touchingGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (attackOnCooldown){
            return;
        }

        //UP
        if (Input.GetAxis("VerticalSecondary") > 0)
        {
            Attack(0, 1);
        }
        //RIGHT
        else if (Input.GetAxis("HorizontalSecondary") > 0)
        {
            Attack(1, 0);
        }
        //DOWN
        else if (Input.GetAxis("VerticalSecondary") < 0)
        {
            Attack(0, -1);
        }
        //LEFT
        else if (Input.GetAxis("HorizontalSecondary") < 0)
        {
            Attack(-1, 0);
        }
        
    }

    private void ResetAttackCooldown(){
        attackOnCooldown = false;
    }

    private void Attack(float xScaleFactor, float yScaleFactor)
    {
        attackOnCooldown = true;
        Invoke(nameof(ResetAttackCooldown), 0.75f);
        GameObject spawnedAttack = Instantiate(attack, transform.position + new Vector3(-0.25f, -0.25f, 0), Quaternion.identity);
        spawnedAttack.GetComponent<HitboxController>().SetScaleFactor(xScaleFactor, yScaleFactor);
        spawnedAttack.transform.SetParent(transform);
    }
    
    private void HandleMovement()
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
}


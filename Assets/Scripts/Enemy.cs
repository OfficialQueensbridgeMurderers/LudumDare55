using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool dead = false;
    protected new Rigidbody rigidbody;

    protected float minRotationSpeed = 200;
    protected float maxRotationSpeed = 500;
    public Material deadMat;

    public float leapForce = 10f;
    public float leapCooldown = 3;
    public bool leapOnCooldown = true;
    protected Transform playerTransform;
    protected bool preventDoubleHit = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Player").transform;
        Invoke(nameof(ResetLeapCooldown), leapCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    protected void ResetLeapCooldown(){
        leapOnCooldown = false;
    }

    protected virtual void HandleMovement()
    {
        if (playerTransform == null || leapOnCooldown || dead)
        {
            return;
        }

        // Calculate the direction vector from current position to player's position
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Apply a force in the calculated direction to make the object leap towards the player
        rigidbody.AddForce((direction * leapForce) + Vector3.up * 2, ForceMode.Impulse);

        Invoke(nameof(ResetLeapCooldown), leapCooldown);

        leapOnCooldown = true;
    }
    
    public void Hit(Vector3 hitDirection){
        //Destroy(gameObject);
        if (!preventDoubleHit){
            preventDoubleHit = true;
            Invoke(nameof(ResetPreventDoubleHit), 0.5f);
            if (!dead){
                Death(hitDirection);
            }
            else{
                Sacrifice();
            }
        }
    }

    protected void ResetPreventDoubleHit(){
        preventDoubleHit = false;
    }

    protected virtual void Sacrifice(){
        SacrificeManager sacrificeManager = GameObject.Find("SacrificeManager").GetComponent<SacrificeManager>();
        sacrificeManager.SacrificeLeaper(transform.position);
        Destroy(gameObject);
    }

    protected void Death(Vector3 hitDirection){
        dead = true;

        GetComponent<Renderer>().material = deadMat;

        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;

        rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationX;
        rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;

        rigidbody.AddForce(hitDirection * 300);

        float xSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        float ySpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        float zSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        // Apply the random rotation velocity to the Rigidbody
        rigidbody.angularVelocity = new Vector3(xSpeed, ySpeed, zSpeed);

        //Debug.Log("force added");
    }
}

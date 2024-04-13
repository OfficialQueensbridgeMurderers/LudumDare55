using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyingState{
    Chasing,
    Attacking
}
public class FlyingEnemy : Enemy
{
    FlyingState currentState = FlyingState.Chasing;
    float targetDistanceToPlayer = 3;
    float moveSpeed = 4;
    bool shot = false;
    public GameObject projectile;
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

    protected override void Sacrifice(){
        SacrificeManager sacrificeManager = GameObject.Find("SacrificeManager").GetComponent<SacrificeManager>();
        sacrificeManager.SacrificeFlyingEnemy(transform.position);
        Destroy(gameObject);
    }

    protected override void HandleMovement()
    {
        if (dead){
            return;
        }

        if (currentState == FlyingState.Chasing){
            Vector3 directionToTarget = playerTransform.position - transform.position;

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            if (directionToTarget.magnitude <= targetDistanceToPlayer){
                currentState = FlyingState.Attacking;
            }
        }
        else if (currentState == FlyingState.Attacking){
            if (!shot){
                ShootProjectile();
            }
        }
    }

    protected void ChasingState(){
        currentState = FlyingState.Chasing;
        shot = false;
    }

    protected void ShootProjectile()
    {
        shot = true;
        Invoke(nameof(ChasingState), 1.5f);

        Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        spawnedProjectile.direction = (playerTransform.position - transform.position).normalized;
        spawnedProjectile.speed = 10;

    }
}

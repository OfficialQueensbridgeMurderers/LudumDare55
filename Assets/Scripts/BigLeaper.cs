using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLeaper : Enemy
{
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
    

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<BigLeaper>() == null){
            other.gameObject.SendMessage("Hit", other.gameObject.transform.position - transform.position);
        }
    }

    protected override void HandleMovement()
    {
        if (playerTransform == null || leapOnCooldown || dead)
        {
            return;
        }

        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        // Loop through all found GameObjects
        foreach (GameObject obj in objectsWithTag)
        {
            if (obj == gameObject){
                continue;
            }
            // Calculate the distance between the current GameObject and the reference point
            float distance = Vector3.Distance(obj.transform.position, transform.position);

            // Check if this GameObject is closer than the previously closest one
            if (distance < closestDistance)
            {
                // Update the closest GameObject and its distance
                closestObject = obj;
                closestDistance = distance;
            }
        }

        float distanceToPlayer = (playerTransform.position - transform.position).magnitude;
        Transform target;
        if (distanceToPlayer < closestDistance){
            target = playerTransform;
        }
        else{
            target = closestObject.transform;
        }

        // Calculate the direction vector from current position to player's position
        Vector3 direction = (target.position - transform.position).normalized;

        if (direction.y < 0){
            if (direction.x > 0){
                direction.x += direction.y *-1;
            }
            else{
                direction.x += direction.y;
            }
        }

        // Apply a force in the calculated direction to make the object leap towards the player
        rigidbody.AddForce((direction * leapForce) + Vector3.up * 4, ForceMode.Impulse);

        Invoke(nameof(ResetLeapCooldown), leapCooldown);

        leapOnCooldown = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float maxDistance = 5;
    protected float distanceTraveled = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = direction * speed * Time.deltaTime;
        transform.position += delta;
        distanceTraveled += delta.magnitude;

        if (distanceTraveled >= maxDistance){
            Destroy(gameObject);
        }
    }
}

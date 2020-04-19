using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public Vector3 velocity;
    public float range;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Change projectile's position.
        transform.position += velocity * Time.deltaTime;
        if (((Time.time - startTime) * velocity.magnitude ) > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
//        Debug.Log("Projectile " + name + " collided with " + collision.gameObject.name);
        Destroy(gameObject);
    }
}

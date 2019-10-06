using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;

    public Camera PlayerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        
        //send out a raycast and print the name of the object it collided with
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            //show the raycast in the scene window
            Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward, Color.green, range);
        }
        
    }
}

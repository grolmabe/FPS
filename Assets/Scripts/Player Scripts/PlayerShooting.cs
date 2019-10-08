using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public Camera PlayerCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactFlash;
    public GameObject impactDust;

    private float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play(); 
        //send out a raycast and print the name of the object it collided with
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            GameObject impactFlashGameObject = Instantiate(impactFlash, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject impactDustGameObject = Instantiate(impactDust, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactFlashGameObject, 2.0f);
            Destroy(impactDustGameObject, 2.0f);
        }
        //show the raycast in the scene window
        Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * range, Color.green, 5f);
    }
}

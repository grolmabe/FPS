using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public float damage;
    public float range;
    public float fireRate;
    public Camera PlayerCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactFlash;
    public GameObject impactDust;
    public bool fullAuto;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
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

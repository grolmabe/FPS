using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float activeHeight; // The height (y-coordinate) of the weapon when it is active, relative to PlayerCamera.
    public float inactiveHeight; // The height (y-coordinate) of the weapon when it is inactive, relative to PlayerCamera.
    public float damage; // The amount of damage this weapon does if it hits fully.
    public float range; // The maximum range of this weapon. Objects further than range units away can't be hit.
    public float fireRate = 0.0f; // The maximum rate, in shots per second, at which this weapon can fire. 
    public ParticleSystem muzzleFlash; // The ParticleSystem that implements the muzzle flash for this weapon.
    public GameObject impactFlash; // The object that implements the impact flash effect for this weapon.
    public GameObject impactDust; // The object that implements the impact dust effect for this weapon.
    public float impactFlashLifespan = 1.0f;
    public float impactDustLifespan = 1.0f;
    public bool fullAuto; // Is this a fully automatic weapon?
    public enum WeaponType {raycast, projectile};
    public WeaponType type; // Is this a raycast weapon, or does it shoot projectiles that are actual objects in the scene?
    public AmmoManager ammo;
    public int roundsPerShot;
    public AmmoManager.AmmoType ammoType;

    private bool safetyOn = true; // Is the safety switch on. If so, the weapon can't be fired.
    private float nextTimeToFire = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame if this object is active.
    void Update()
    {
        // Check for the weapon being fired.
        if (fullAuto == true)
        {
            // If this is an automatic weapon, request to fire if the fire button is currently being pressed.
            if (Input.GetButton("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            // If this is a non-automatic weapon, request to fire if the fire button was pressed.
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

    }

    public void Shoot()
    {
        if ((safetyOn == false) && ((fireRate == 0.0f) || (Time.time >= nextTimeToFire)) && (ammo.Remove(ammoType, roundsPerShot) == roundsPerShot))
        {
            muzzleFlash.Play();
            if (type == WeaponType.raycast)
            {
                ShootRaycast();
            }
            else if (type == WeaponType.projectile)
            {
                ShootProjectile();
            }
            if (fireRate != 0.0f)
            {
                nextTimeToFire = Time.time + 1.0f / fireRate;
            }
        }
    }

    void ShootRaycast()
    {
        RaycastHit hit;

        // Send out a raycast in the direction the weapon is aimed.
        // We use the parent transform as the origin point, as that is the camera and gives a better experience.
        if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, range))
        {
            // The raycast hit an object.
            // Debug.Log(hit.transform.name);
            // Create the impact effects at the point of impact.
            GameObject impactFlashGameObject = Instantiate(impactFlash, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject impactDustGameObject = Instantiate(impactDust, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactFlashGameObject, impactFlashLifespan);
            Destroy(impactDustGameObject, impactDustLifespan);
            // Apply the damage to the object that was hit.
        }
        //show the raycast in the scene window
        //Debug.DrawRay(transform.parent.position, transform.parent.forward * range, Color.green, 5f);
    }

    void ShootProjectile()
    {

    }

    public void SafetyOn()
    {
        safetyOn = true;
    }

    public void SafetyOff()
    {
        safetyOn = false;
    }

}

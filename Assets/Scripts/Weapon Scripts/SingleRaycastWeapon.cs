using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRaycastWeapon : Weapon
{
    protected override void Fire()
    {
        // Send out a raycast in the direction the weapon is aimed.
        // We use the parent transform as the origin point, as that is the camera and gives a better experience.
        RaycastFire(transform.parent.forward);
    }

    protected void RaycastFire(Vector3 direction)
    {
        RaycastHit hit;

        // Send out a raycast in the given direction.
        if (Physics.Raycast(transform.parent.position, direction, out hit, range))
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
        Debug.DrawRay(transform.parent.position, direction * range, Color.green, 5f);

    }
}

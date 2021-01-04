using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySingleRaycastWeapon : SingleRaycastWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        // The enemy only has a single weapon, so enable it.
        SafetyOff();
        gameObject.SetActive(true);
        base.Start();
    }

    protected override bool FireRequested()
    {
        RaycastHit hit;

        //show the raycast in the scene window
        Debug.DrawRay(transform.parent.position, transform.parent.forward * range, Color.green, 5f);

        // Send out a ray into the scene in the direction the parent object is facing and see if we hit anything.
        if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, range))
        {
            Debug.Log("Raycast hit object " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Player")
            {
                // The ray hit the player, so we want to fire the weapon.
                return true;
            }
        }
        // The ray did not hit the player, so we don't want to fire the weapon.
        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleProjectileWeapon : Weapon
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float projectileOffset; // The offset along the forward direction to start the projectile from.
    public GameObject player; // So we can ignore collisions and raycasts between the player and the projectile.

    protected override void Fire()
    {
        // Send out a projectile in the direction the weapon is aimed.
        // We use the parent transform as the origin point, as that is the camera and gives a better experience.
        ProjectileFire(transform.parent.forward);
    }

    protected void ProjectileFire(Vector3 direction)
    {
        ProjectileMovement pm;
        GameObject projectile;
        RaycastHit hit;
        Vector3 adjustedDirection;
        int saveLayer;
        float playerVelocityComponent;

        // Since our crosshairs are relative to the camera, but our projectile
        // needs to originate from the weapon, we need to have the projectile
        // destination match where the crosshairs are.
        // Send out a raycast in the given direction.
        // First, make sure it can't hit the player object.
        saveLayer = player.layer;
        player.layer = LayerMask.NameToLayer("Ignore Raycast");
        if (Physics.Raycast(transform.parent.position, direction, out hit, range))
        {
            // The raycast hit an object. The direction for the projectile is
            // from the position of the weapon to the point where the raycast
            // hit the object.
            //Debug.Log("Raycast hit object " + hit.collider.gameObject.name);
            adjustedDirection = hit.point - transform.position;
        }
        
        {
            // The raycast didn't hit an object. The direction for the
            // projectile is from the weapon to the point where the parent
            // transform's forward direction reaches the range for this weapon.
            adjustedDirection = transform.parent.position + direction.normalized * range - transform.position;
        }
        // Set the player object's layer back to what it was.
        player.layer = saveLayer;

        projectile = Instantiate(projectilePrefab, transform.position + transform.forward * projectileOffset, Quaternion.identity);
        if (projectile != null)
        {
            // Add this object to the "Ignore Raycast" layer so that a future
            // aiming raycast won't hit this projectile. (In early testing,
            // this was happening, leading to undesireable trajectories.)
            projectile.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            // Ignore collisions between the projectile and the player object.
            // This prevents issues if the player is looking almost directly
            // down and the gun ends up inside the player object (because the
            // projectile might then start inside the player object).
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), player.GetComponent<Collider>());
            // Determine the component of the player's velocity in the projectile's direction.
            // So we can add this to the projectile's speed relative to the weapon.
            playerVelocityComponent = Vector3.Dot(player.GetComponent<PlayerMovement>().GetVelocity(), adjustedDirection.normalized);
            //Debug.Log("player's velocity = " + player.GetComponent<PlayerMovement>().GetVelocity());
            //Debug.Log("playerVelocityComponent = " + playerVelocityComponent);
            pm = projectile.GetComponent<ProjectileMovement>();
            pm.velocity = adjustedDirection.normalized * (projectileSpeed + playerVelocityComponent);
            pm.range = range;
        }
    }

}

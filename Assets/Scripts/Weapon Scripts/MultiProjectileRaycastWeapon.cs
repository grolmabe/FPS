using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiProjectileRaycastWeapon : SingleProjectileRaycastWeapon
{
    public int projectilesPerRound;
    public float clusterFactor;
    public float coneAngle;
    private NormalDistribution nd = new NormalDistribution();

    protected override void Fire()
    {
        RaycastHit hit;
        Vector3 direction;
        Vector2 point;
        float angle;
        float d;

        for (int i = 0; i < projectilesPerRound; i++)
        {
            // Determine the direction for this projectile.
            angle = Random.Range(0f, 2f * Mathf.PI);
            d = nd.Next(0f, 1f / clusterFactor, -1f, 1f);
            point = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * d;
            direction = ((transform.parent.forward * (1 / Mathf.Tan(Mathf.Deg2Rad * (coneAngle / 2)))) + (transform.parent.up * point.y) + (transform.parent.right * point.x)).normalized;
            RaycastFire(direction);
        }
    }
}

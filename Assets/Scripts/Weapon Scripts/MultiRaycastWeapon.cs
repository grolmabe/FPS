using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiRaycastWeapon : SingleRaycastWeapon
{
    public int projectilesPerRound;
    public float clusterFactor;
    public float coneAngle;
    private NormalDistribution nd = new NormalDistribution();

    // The Fire() method implements firing a weapon that has rounds containing
    // multiple projectiles. (Think of a shotgun, where there are multiple
    // shots in a single shell.)
    // A separate trajectory is determined for each projectile, and then
    // that trajectory is raycasted out into the scene to see if it hits
    // anything.
    // 
    // Implementation notes:
    //
    // The trajectory for each projectile is randomized within certain
    // constraints. It is restricted to falling within a cone that has a given
    // apex angle, with the apex at the parent transform's position and
    // extending out in the parent transform's forward direction. The
    // randomization of the trajectory is accomplished by choosing an
    // arbitrary point on a circular cross-section of the cone (i.e.,
    // perpendicular to the central axis of the cone) with radius of one unit,
    // then finding a direction vector from the parent transform's position to
    // that point. The point on the cross-section is chosen by choosing a
    // random angle (between 0 and 2 Pi radians) from the parent transform's
    // "right" direction, then selecting a point along that angle that is an
    // arbitrary distance between -1 and 1 units from the centre. That distance
    // is selected according to a normal (gaussian) distribution, so the points
    // selected in this manner will be clustered closer to the centre. The
    // standard deviation used is determined by the clusterFactor property
    // (specifically, 1 / clusterFactor).

    protected override void Fire()
    {
        Vector3 direction; // The direction of the current projectile.
        Vector2 point; // The projectile direction is changed from directly
                       // forward by this x and y amount.
        float angle; // The 
        float d;

        for (int i = 0; i < projectilesPerRound; i++)
        {
            // Determine the direction for this projectile.
            // Get a random angle.
            angle = Random.Range(0f, 2f * Mathf.PI);
            // Get an arbitrary value in the range of -1 to 1 that follows
            // a normal distribution.
            d = nd.Next(0f, 1f / clusterFactor, -1f, 1f);
            // Determine the x and y coordinates of a vector in the direction
            // given by "angle" of length given by "d".
            point = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * d;
            // Determine the direction vector for this projectile. This is
            // done by starting with a vector in the parent transform's forward
            // direction that extends to the circular cross section, then
            // adding the x-axis (right) and y-axis (up) component vectors to
            // get to the select point on the circular cross section, then
            // normalizing the resulting vector.
            direction = ((transform.parent.forward * (1f / Mathf.Tan(Mathf.Deg2Rad * (coneAngle / 2)))) + (transform.parent.right * point.x) + (transform.parent.up * point.y)).normalized;
            RaycastFire(direction);
        }
    }
}

// Object-Bounding-Box Collision Hull for 2D Space
// Same Axes as Local Axes

using UnityEngine;

public class OBBCollisionHull2D : CollisionHull2D
{
    [Header("Object Bounding Box Hull Attributes")]
    // pg. 258 in Millington 2nd Ed. "general OBB should have a separation orientation Quaternion
    // This is how it is differentiated from AABB collision hulls
    public Quaternion orientation;
    [Tooltip("Half-dimensions of the box")]
    public Vector3 halfSize;
    [Tooltip("Center of the box")]
    public Vector3 center;

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c)
    {
        /// <see cref="CircleCollisionHull2D.TestCollisionVsCircle(CircleCollisionHull2D)"/>

        return false;
    }

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other, ref Collision c)
    {
        /// <see cref="AABBCollisionHull2D.TestCollisionVsAABB(AABBCollisionHull2D)"/>

        return false;
    }

    public override bool TestCollisionVsOBB(OBBCollisionHull2D other, ref Collision c)
    {
        // same as AABB-OBB part 2, twice

        return false;
    }

    private void Awake()
    {
        SetType(CollisionHullType2D.HULL_OBB);
        particle = GetComponent<Particle2DComponent>();
        center = particle.transform.position;
        halfSize = 0.5f * particle.transform.localScale;
    }
}

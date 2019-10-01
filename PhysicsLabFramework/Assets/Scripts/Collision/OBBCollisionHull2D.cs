// Object-Bounding-Box Collision Hull for 2D Space
// Same Axes as Local Axes

using UnityEngine;

public class OBBCollisionHull2D : CollisionHull2D
{
    [Header("Object Bounding Box Hull Attributes")]
    // pg. 258 in Millington 2nd Ed. "general OBB should have a separation orientation Quaternion
    // This is how it is differentiated from AABB collision hulls
    public Quaternion orientation;
    [Tooltip("Center of the box")]
    public Vector3 center;
    [Tooltip("Minimum Extent of the box")]
    public Vector3 minExtent;
    [Tooltip("Maximum Extent of the box")]
    public Vector3 maxExtent;

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        /// <see cref="CircleCollisionHull2D.TestCollisionVsCircle(CircleCollisionHull2D)"/>

        return false;
    }

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other)
    {
        /// <see cref="AABBCollisionHull2D.TestCollisionVsAABB(AABBCollisionHull2D)"/>

        return false;
    }

    public override bool TestCollisionVsOBB(OBBCollisionHull2D other)
    {
        // same as AABB-OBB part 2, twice

        return false;
    }

    public override void UpdateCenterPos()
    {
        if (particle)
            center = particle.GetPosition();
    }

    public override void UpdateExtents()
    {
        minExtent = 0.5f * particle.transform.lossyScale;
        maxExtent = 1.5f * particle.transform.lossyScale;
    }

    private void Awake()
    {
        SetType(CollisionHullType2D.HULL_OBB);
        particle = GetComponent<Particle2DComponent>();
        center = particle.transform.position;
    }
}

// Axis-Aligned-Bounding-Box Collision Hull for 2D Space
// Same Axes as World Axes

using UnityEngine;

public class AABBCollisionHull2D : CollisionHull2D
{
    [Header("Axis-Aligned Hull Attributes")]
    [Tooltip("Center of the box")]
    public Vector3 center;
    [Tooltip("Minimum Extent of the box")]
    public Vector3 minExtent;
    [Tooltip("Maximum Extent of the box")]
    public Vector3 maxExtent;

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        /// <see cref="CircleCollisionHull2D.TestCollisionVsAABB(AABBCollisionHull2D)"/>

        // Step 01: Get center & radius of other
        Vector3 otherCenter = other.center;
        float otherRadius = other.radius;

        // Step 02: Clamp center within extents
        float xPosClamp = Mathf.Clamp(otherCenter.x, minExtent.x, maxExtent.x);
        float yPosClamp = Mathf.Clamp(otherCenter.y, minExtent.y, maxExtent.y);
        float zPosClamp = Mathf.Clamp(otherCenter.z, minExtent.z, maxExtent.z);

        // Step 03: Establish closest point
        Vector3 closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);

        // Step 04: get distance for contact
        float distance = (closestPoint - otherCenter).sqrMagnitude;

        // Step 05: Check that the closest point is within the AABB box
        if (distance < otherRadius * otherRadius)
            return true;
        else
            return false;
    }

    // TODO Implement AABB vs AABB
    public override bool TestCollisionVsAABB(AABBCollisionHull2D other)
    {
        // pass if, for all axes, max extent of A is greather than min extent of B

        // Step 01: Get min extent of B
        
        
        return false;
    }

    // TODO Implement AABB vs OBB
    public override bool TestCollisionVsOBB(OBBCollisionHull2D other)
    {
        // same as above twice:
        // first, find max extents of OBB, do AABB vs this box
        // then, transform this box into OBB's space, find max extents, repeat

        // Step 01: Get max extents of OBB
        Vector3 otherMax = other.maxExtent;
        // Step 02: AABB vs OtherMax

        return false;
    }

    public override void UpdateCenterPos()
    {
        if (particle)
            center = particle.GetPosition();
    }

    // Initialize local variables
    private void Awake()
    {
        SetType(CollisionHullType2D.HULL_AABB);
        particle = GetComponent<Particle2DComponent>();
        // Initialize center to object position (we're working in a origin-centered world)
        center = particle.GetPosition();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(center, 0.5f * minExtent.magnitude);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, 0.5f * maxExtent.magnitude);
    }
}

// Axis-Aligned-Bounding-Box Collision Hull for 2D Space
// Same Axes as World Axes

using UnityEngine;

public class AABBCollisionHull2D : CollisionHull2D
{
    [Header("Axis-Aligned Hull Attributes")]
    [Tooltip("Center of the box")]
    public Vector3 center;
    protected Vector3 minExtent, maxExtent;

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

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other)
    {
        // Pass Condition: If, for all axes (X, Y, Z), the MaxExtent of This is overlapping the MinExtent of Other

        // Step 01: Store other's min extent
        Vector3 otherMin = other.minExtent;

        // Step 01: Compare min & max extents and store in boolean variables
        bool diffX = otherMin.x < maxExtent.x ? true : false;
        bool diffY = otherMin.y < maxExtent.y ? true : false;
        bool diffZ = otherMin.z < maxExtent.z ? true : false;

        // Check that all extents are passing properly
        // If yes, return true, else, return false
        if (diffX && diffY && diffZ)
            return true;
        else
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

    public void UpdateExtents()
    {

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
        Gizmos.DrawWireCube(center, minExtent);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, maxExtent);
    }
}

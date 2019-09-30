// Axis-Aligned-Bounding-Box Collision Hull for 2D Space
// Same Axes as World Axes

using UnityEngine;

public class AABBCollisionHull2D : CollisionHull2D
{
    [Header("Axis-Aligned Hull Attributes")]
    [Tooltip("Half-dimensions of the box")]
    public Vector3 halfSize;
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
        // pass if, for all axes, max extent of A is greather than min extent of B
        
        // Step 01: store extents of A & B

        return false;
    }

    public override bool TestCollisionVsOBB(OBBCollisionHull2D other)
    {
        // same as above twice:
        // first, find max extents of OBB, do AABB vs this box
        // then, transform this box into OBB's space, find max extents, repeat
        // Step 01:
        
        return false;
    }

    // Initialize local variables
    private void Awake()
    {
        SetType(CollisionHullType2D.HULL_AABB);
        particle = GetComponent<Particle2DComponent>();
        // Initialize center to object position (we're working in a origin-centered world)
        center = particle.GetPosition();
        // Initialize halfSize to half of the global scale
        halfSize = 0.5f * particle.transform.lossyScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, maxExtent);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, minExtent);
    }

    public override void UpdateCenterPos()
    {
        if (particle)
            center = particle.GetPosition();
    }
}

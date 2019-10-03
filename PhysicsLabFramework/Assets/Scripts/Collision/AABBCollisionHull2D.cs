// Axis-Aligned-Bounding-Box Collision Hull for 2D Space
// Same Axes as World Axes

using UnityEngine;

public class AABBCollisionHull2D : CollisionHull2D
{
    public Vector3 center
    {
        get; private set;
    }
    public Vector3 halfSize
    {
        get; private set;
    }

    public Vector3 minExtent
    {
        get; private set;
    }
    public Vector3 maxExtent
    {
        get; private set;
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, out Collision c)
    {
        /// <see cref="CircleCollisionHull2D.TestCollisionVsAABB(AABBCollisionHull2D)"/>

        // Step 01: Get center & radius of other
        Vector3 otherCenter = other.center;
        float otherRadSqr = other.radius * other.radius;

        // Step 02: Clamp center within extents
        float xPosClamp = Mathf.Clamp(otherCenter.x, minExtent.x, maxExtent.x);
        float yPosClamp = Mathf.Clamp(otherCenter.y, minExtent.y, maxExtent.y);
        float zPosClamp = Mathf.Clamp(otherCenter.z, minExtent.z, maxExtent.z);

        // Step 03: Establish closest point
        Vector3 closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);

        // Step 04: get distance for contact
        float distance = (closestPoint - otherCenter).sqrMagnitude;

        c = new Collision();

        // Step 05: Check that the closest point is within the AABB box
        if (distance < otherRadSqr)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other, out Collision c)
    {
        // Pass Condition: If, for all axes (X, Y, Z), the MaxExtent of This is overlapping the MinExtent of Other

        // Step 01: Store other's min extent
        Vector3 otherMin = other.minExtent;
        Vector3 otherMax = other.maxExtent;

        // Step 01: Compare min & max extents and store in boolean variables
        bool diffX = (otherMin.x < maxExtent.x && minExtent.x < otherMax.x) ? true : false;
        bool diffY = (otherMin.y < maxExtent.y && minExtent.y < otherMax.y) ? true : false;
        bool diffZ = (otherMin.z < maxExtent.z && minExtent.z < otherMax.z) ? true : false;

        c = new Collision();

        // Check that all extents are passing properly
        // If yes, return true, else, return false
        if (diffX && diffY && diffZ)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(OBBCollisionHull2D other, out Collision c)
    {
        // same as above twice:
        // first, find max extents of OBB, do AABB vs this box
        // then, transform this box into OBB's space, find max extents, repeat

        // Step 01: Get extents of other box
        Vector3 otherMin = other.minExtent;
        Vector3 otherMax = other.maxExtent;

        bool diffX = (otherMin.x < maxExtent.x && minExtent.x < otherMax.x) ? true : false;
        bool diffY = (otherMin.y < maxExtent.y && minExtent.y < otherMax.y) ? true : false;
        bool diffZ = (otherMin.z < maxExtent.z && minExtent.z < otherMax.z) ? true : false;

        Vector3 transBox = Vector3.Project(particle.GetPosition(), other.particle.GetPosition());

        c = new Collision();

        // Honestly, no idea what I'm doing here, and I need to move on to Project 5
        return false;
    }

    // Called in an Update loop to re-define min & max extents
    public void UpdateExtents()
    {
        halfSize = 0.5f * particle.transform.localScale;
        
        minExtent = new Vector3(center.x - halfSize.x, center.y - halfSize.y, center.z - halfSize.z);
        maxExtent = new Vector3(center.x + halfSize.x, center.y + halfSize.y, center.z + halfSize.z);
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

        // Initialize halfSize to half of the world-scale of the particle
        halfSize = 0.5f * particle.transform.lossyScale;
    }
}

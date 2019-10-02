// Object-Bounding-Box Collision Hull for 2D Space
// Same Axes as Local Axes

using UnityEngine;

public class OBBCollisionHull2D : CollisionHull2D
{
    // pg. 258 in Millington 2nd Ed. "general OBB should have a separation orientation Quaternion
    // This is how it is differentiated from AABB collision hulls
    // public Quaternion orientation;

    public Vector3 center
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
    public Vector3 halfSize
    {
        get; private set;
    }

    // Don't know how to acquire this
    public Vector3 inverseWorldMatrix
    {
        get; private set;
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        /// <see cref="CircleCollisionHull2D.TestCollisionVsOBB(OBBCollisionHull2D)"/>

        // Step 01: Get center & sqrRadius of other
        Vector3 origCenter = other.center;
        float radiusSqr = other.radius * other.radius;

        // Step 02: Multiply center by local world matrix inverse
        Vector3 newCenter = Vector3.Cross(origCenter, inverseWorldMatrix);

        // Step 03: Clamp new center to local min and max
        float xPosClamp = Mathf.Clamp(newCenter.x, minExtent.x, maxExtent.x);
        float yPosClamp = Mathf.Clamp(newCenter.y, minExtent.y, maxExtent.y);
        float zPosClamp = Mathf.Clamp(newCenter.z, minExtent.z, maxExtent.z);

        // Step 04: Establish closest point
        Vector3 closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);

        // Step 05: Establish distance of contact
        float distance = (closestPoint - newCenter).sqrMagnitude;

        // Step 06: Check to see if we're in contact
        if (distance < radiusSqr)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other)
    {
        /// <see cref="AABBCollisionHull2D.TestCollisionVsOBB(OBBCollisionHull2D)"/>
        
        // Step 01: Get extents of other box
        Vector3 otherMin = other.minExtent;
        Vector3 otherMax = other.maxExtent;

        bool diffX = (otherMin.x < maxExtent.x && minExtent.x < otherMax.x) ? true : false;
        bool diffY = (otherMin.y < maxExtent.y && minExtent.y < otherMax.y) ? true : false;
        bool diffZ = (otherMin.z < maxExtent.z && minExtent.z < otherMax.z) ? true : false;

        Vector3 transBox = Vector3.Project(particle.GetPosition(), other.particle.GetPosition());

        // Honestly, no idea what I'm doing here, and I need to move on to Project 5
        return false;
    }

    // Don't know what I'm doing, hopefully will figure out in class
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

    public void UpdateExtents()
    {
        halfSize = 0.5f * particle.transform.localScale;
        minExtent = new Vector3(center.x - halfSize.x, center.y - halfSize.y, center.z - halfSize.z);
        maxExtent = new Vector3(center.x + halfSize.x, center.y + halfSize.y, center.z + halfSize.z);
    }

    private void Awake()
    {
        SetType(CollisionHullType2D.HULL_OBB);
        particle = GetComponent<Particle2DComponent>();
        center = particle.transform.position;
        halfSize = 0.5f * particle.transform.localScale;
    }
}

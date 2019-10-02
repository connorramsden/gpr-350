// Circle Collision Hull for 2D Space

using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
    private const float MAX_RADIUS = 100.0f;

    public float radius
    {
        get; private set;
    }

    public Vector3 center
    {
        get; private set;
    }

    // Architecture Style 2 //
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c)
    {
        // collision if distance between centers is <= sum of radii
        // optimized collision if (distance between centers)^2 <= (sum of radii)^2
        // Step 01: Get the two centers
        // Step 02: Take the difference
        // Step 03: distance squared = dot(diff, diff)
        // Step 04: add the radii
        // Step 05: square the sum of radii
        // Step 06: DO THE TEST: distSq <= sumSq

        Vector3 distance = center - other.center;
        float distSq = Vector3.Dot(distance, distance);
        float sum = radius + other.radius;
        float sumSq = sum * sum;

        if (distSq <= sumSq)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other, ref Collision c)
    {
        // find the closest point to the cicle on the box
        // done by clamping center of circle to be within box dimensions
        // if closest point is within circle, pass (do point vs circle collision test)

        // Step 01: Get max and min extent of other
        Vector3 minExtent = other.minExtent;
        Vector3 maxExtent = other.maxExtent;

        // Step 02: Clamp center within extents
        float xPosClamp = Mathf.Clamp(center.x, minExtent.x, maxExtent.x);
        float yPosClamp = Mathf.Clamp(center.y, minExtent.y, maxExtent.y);
        float zPosClamp = Mathf.Clamp(center.z, minExtent.z, maxExtent.z);

        // Step 03: Establish closest point
        Vector3 closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);

        // Step 04: Establish distance of contact (Millington 2nd Ed. pg. 317)
        float distance = (closestPoint - center).sqrMagnitude;

        // Step 05: Check to see if we're in contact
        if (distance < radius * radius)
            return true;
        else
            return false;
    }
    
    public override bool TestCollisionVsOBB(OBBCollisionHull2D other)
    {
        // same as AABB collision, but first..
        // multiply circle center by box world matrix inverse

        // Step 01: Get max and min extent of other
        Vector3 otherMin = other.minExtent;
        Vector3 otherMax = other.maxExtent;

        // Step 02: multiply center by other world matrix inverse
        Vector3 newCenter = Vector3.Cross(center, other.inverseWorldMatrix);

        // Step 03: Clamp new center to max & min of other
        float xPosClamp = Mathf.Clamp(newCenter.x, otherMin.x, otherMax.x);
        float yPosClamp = Mathf.Clamp(newCenter.y, otherMin.y, otherMax.y);
        float zPosClamp = Mathf.Clamp(newCenter.z, otherMin.z, otherMax.z);

        // Step 04: Establish closest point
        Vector3 closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);

        // Step 05: Establish distance of contact
        float distance = (closestPoint - newCenter).sqrMagnitude;

        // Step 06: Check to see if we're in contact
        if (distance < radius * radius)
            return true;
        else
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
        SetType(CollisionHullType2D.HULL_CIRCLE);
        particle = GetComponent<Particle2DComponent>();

        // Initialize center position
        center = particle.GetPosition();

        radius = particle.radius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, radius);
    }

}

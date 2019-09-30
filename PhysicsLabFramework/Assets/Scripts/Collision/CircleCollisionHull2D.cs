// Circle Collision Hull for 2D Space

using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
    private const float MAX_RADIUS = 100.0f;

    [Header("Circle Hull Attributes")]
    [Tooltip("Radius of the circle"), Range(0.0f, MAX_RADIUS)]
    public float radius;
    [Tooltip("Center of the circle")]
    public Vector3 center;

    // Architecture Style 2 //
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // collision if distance between centers is <= sum of radii
        // optimized collision if (distance between centers)^2 <= (sum of radii)^2
        // Step 01: Get the two centers
        // Step 02: Take the difference
        // Step 03: distance squared = dot(diff, diff)
        // Step 04: add the radii
        // Step 05: square the sum of radii
        // Step 06: DO THE TEST: distSq <= sumSq

        Vector2 distance = particle.GetPosition() - other.particle.GetPosition();
        float distSq = Vector3.Dot(distance, distance);
        float sum = radius + other.radius;
        float sumSq = sum * sum;

        if (distSq <= sumSq)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other)
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

        if(closestPoint.sqrMagnitude < radius * radius)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(OBBCollisionHull2D other)
    {
        // same as AABB collision, but first..
        // multiply circle center by box world matrix inverse

        return false;
    }

    // Initialize local variables
    private void Awake()
    {
        SetType(CollisionHullType2D.HULL_CIRCLE);
        particle = GetComponent<Particle2DComponent>();

        // Initialize center position
        center = particle.GetPosition();
    }

    public override void UpdateCenterPos()
    {
        if (particle)
            center = particle.GetPosition();
    }
}

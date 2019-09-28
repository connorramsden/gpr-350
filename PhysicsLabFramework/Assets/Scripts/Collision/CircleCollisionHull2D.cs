using UnityEngine;

// Circle Collision Hull for 2D Space
public class CircleCollisionHull2D : CollisionHull2D
{
    private const float MAX_RADIUS = 100.0f;

    public CircleCollisionHull2D(CollisionHullType2D newType) : base(CollisionHullType2D.HULL_CIRCLE)
    {}

    [Header("Circle Attributes"), Tooltip("Radius of the circle")]
    [Range(0.0f, MAX_RADIUS)]
    public float radius;

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
        // find the closest point to the ricle on the box
        // done by clamping center of circle ot be within box dimensions
        // if closest point is within circle, pass (do point vs circle collision test)

        return false;
    }

    public override bool TestCollisionVsOBB(OBBCollisionHull2D other)
    {
        // same as AABB collision, but first..
        // multiply circle center by box world matrix inverse

        return false;
    }
}

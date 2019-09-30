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

        // Step 01: Get the center of the circle
        center = particle.transform.position;
        // Step 02: Get box (other) dimensions
        Vector3 boxDimensions = other.transform.lossyScale;
        // Step 03: Clamp center within box dimensions
        // Vector3 closestPoint = Vector3.ClampMagnitude(particle, other.particle);

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
        center = particle.transform.position;
    }
}

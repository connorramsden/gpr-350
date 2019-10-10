// Circle Collision Hull for 2D Space

using UnityEngine;
using static NS_Collision.CollisionResolutionManager;

namespace NS_Collision
{
    public class CircleCollisionHull2D : CollisionHull2D
    {
        private const float MAX_RADIUS = 100.0f;

        public float radius
        {
            get; private set;
        }

        // Architecture Style 2 //
        public override bool TestCollisionVsCircle(CircleCollisionHull2D other, out NCollision c)
        {
            // collision if distance between centers is <= sum of radii
            // optimized collision if (distance between centers)^2 <= (sum of radii)^2       

            bool isColliding = false;

            // Step 01: Get the two centers
            // Step 02: Take the difference
            Vector2 distance = center - other.center;
            // Step 03: distance squared = dot(diff, diff)
            float distSq = Vector2.Dot(distance, distance);
            // Step 04: add the radii
            float sum = radius + other.radius;
            // Step 05: square the sum of radii
            float sumSq = sum * sum;

            // Step 06: DO THE TEST: distSq <= sumSq
            if (distSq <= sumSq)
            {
                isColliding = true;
            }

            c.hullOne = this;
            c.hullTwo = other;
            c.status = isColliding;

            // REMOVE THIS LATER
            c = new NCollision();

            return isColliding;
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull2D other, out NCollision c)
        {
            // find the closest point to the cicle on the box
            // done by clamping center of circle to be within box dimensions
            // if closest point is within circle, pass (do point vs circle collision test)

            // Step 01: Get max and min extent of other
            Vector2 minExtent = other.minExtent;
            Vector2 maxExtent = other.maxExtent;

            // Step 02: Clamp center within extents
            float xPosClamp = Mathf.Clamp(center.x, minExtent.x, maxExtent.x);
            float yPosClamp = Mathf.Clamp(center.y, minExtent.y, maxExtent.y);

            // Step 03: Establish closest point
            Vector2 closestPoint = new Vector2(xPosClamp, yPosClamp);

            // Step 04: Establish distance of contact (Millington 2nd Ed. pg. 317)
            float distance = (closestPoint - center).sqrMagnitude;

            c = new NCollision();

            // Step 05: Check to see if we're in contact
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
            type = CollisionHullType2D.HULL_CIRCLE;
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
}
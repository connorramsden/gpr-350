// Axis-Aligned-Bounding-Box Collision Hull for 2D Space
// Same Axes as World Axes

using UnityEngine;
using System.Collections.Generic;

namespace NS_Collision
{
    public class AABBCollisionHull2D : CollisionHull2D
    {
        public Vector2 halfSize
        {
            get; private set;
        }
        public Vector2 minExtent
        {
            get; private set;
        }
        public Vector2 maxExtent
        {
            get; private set;
        }

        public override bool TestCollisionVsCircle(CircleCollisionHull2D other, out NCollision c)
        {
            /// <see cref="CircleCollisionHull2D.TestCollisionVsAABB(AABBCollisionHull2D)"/>

            // Create a new NCollision
            // Assign hulls and instantiate the contact list
            // Status defaults to false
            c = new NCollision
            {
                hullOne = this,
                hullTwo = other,
                contact = new List<NCollision.Contact>(),
                status = false
            };

            // Step 01: Get center & radius of other
            Vector2 otherCenter = other.center;
            float otherRadSqr = other.radius * other.radius;

            // Step 02: Clamp center within extents
            float xPosClamp = Mathf.Clamp(otherCenter.x, minExtent.x, maxExtent.x);
            float yPosClamp = Mathf.Clamp(otherCenter.y, minExtent.y, maxExtent.y);

            // Step 03: Establish closest point
            Vector2 closestPoint = new Vector2(xPosClamp, yPosClamp);

            // Step 04: get distance for contact
            float distance = (closestPoint - otherCenter).sqrMagnitude;

            // Step 05: Check that the closest point is within the AABB box
            if (distance < otherRadSqr)
                c.status = true;
            
            // Finish setting up the collision
            c.contactCount = c.contact.Count;
            
            return c.status;
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull2D other, out NCollision c)
        {
            // Pass Condition: If, for all axes (X & Y), the MaxExtent of This is overlapping the MinExtent of Other

            // Create a new NCollision
            // Assign hulls and instantiate the contact list
            // Status defaults to false
            c = new NCollision
            {
                hullOne = this,
                hullTwo = other,
                contact = new List<NCollision.Contact>(),
                status = false
            };

            // Step 01: Store other's min extent
            Vector2 otherMin = other.minExtent;
            Vector2 otherMax = other.maxExtent;

            // Step 01: Compare min & max extents and store in boolean variables
            bool diffX = (otherMin.x < maxExtent.x && minExtent.x < otherMax.x) ? true : false;
            bool diffY = (otherMin.y < maxExtent.y && minExtent.y < otherMax.y) ? true : false;

            // Check that all extents are passing properly
            // If yes, return true, else, return false
            if (diffX && diffY)
                c.status = true;

            // Finish setting up the collision
            c.contactCount = c.contact.Count;

            return c.status;
        }

        // Called in an Update loop to re-define min & max extents
        public void UpdateExtents()
        {
            halfSize = new Vector2(0.5f * particle.width, 0.5f * particle.height);

            minExtent = new Vector2(center.x - halfSize.x, center.y - halfSize.y);
            maxExtent = new Vector2(center.x + halfSize.x, center.y + halfSize.y);
        }

        // Initialize local variables
        private void Awake()
        {
            type = CollisionHullType2D.HULL_AABB;

            particle = GetComponent<Particle2DComponent>();

            UpdateCenterPos();
            UpdateExtents();
        }
    }
}
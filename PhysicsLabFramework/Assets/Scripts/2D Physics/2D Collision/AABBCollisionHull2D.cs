// Axis-Aligned-Bounding-Box Collision Hull for 2D Space
// Same Axes as World Axes

using UnityEngine;
using System.Collections.Generic;
using NS_Physics2D;

namespace NS_Collision_2D
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
        
        /// <see cref="CircleCollisionHull.TestCollisionVsAABB(AABBCollisionHull2D)"/>
        public override bool TestCollisionVsCircle(CircleCollisionHull other, out NCollision2D collision2D)
        {
            // Create a new NCollision2D
            // Assign hulls and instantiate the contact list
            // Status defaults to false
            collision2D = new NCollision2D
            {
                hullOne = this,
                hullTwo = other,
                contact = new List<NCollision2D.Contact>(),
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
                collision2D.status = true;

            NCollision2D.Contact contact = new NCollision2D.Contact
            {
                pointOfContact = closestPoint,
                coeffRestitution = 0.25f,
                normal = (collision2D.hullOne.particle.movement.position - collision2D.hullTwo.particle.movement.position).normalized,
                depth = distance
            };

            collision2D.contact.Add(contact);

            // Finish setting up the collision2D
            collision2D.contactCount = collision2D.contact.Count;

            return collision2D.status;
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull2D other, out NCollision2D collision2D)
        {
            // Pass Condition: If, for all axes (X & Y), the MaxExtent of This is overlapping the MinExtent of Other

            // Create a new NCollision2D
            // Assign hulls and instantiate the contact list
            // Status defaults to false
            collision2D = new NCollision2D
            {
                hullOne = this,
                hullTwo = other,
                contact = new List<NCollision2D.Contact>(),
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
                collision2D.status = true;

            NCollision2D.Contact contactOne = new NCollision2D.Contact
            {
                pointOfContact = (minExtent - otherMin),
                coeffRestitution = 0.25f,
                depth = (minExtent - otherMin).sqrMagnitude,
                normal = (collision2D.hullOne.particle.movement.position - collision2D.hullTwo.particle.movement.position).normalized
            };

            NCollision2D.Contact contactTwo = new NCollision2D.Contact
            {
                pointOfContact = (maxExtent - otherMax),
                coeffRestitution = 0.25f,
                depth = (maxExtent - otherMax).sqrMagnitude,
                normal = (collision2D.hullOne.particle.movement.position - collision2D.hullTwo.particle.movement.position).normalized
            };

            collision2D.contact.Add(contactOne);
            collision2D.contact.Add(contactTwo);

            // Finish setting up the collision2D
            collision2D.contactCount = collision2D.contact.Count;

            return collision2D.status;
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
            type = CollisionHullType2D.HULL_AABB_2D;

            particle = GetComponent<Particle2DComponent>();

            UpdateCenterPos();
            UpdateExtents();
        }

        public override bool TestCollisisionVsOBB(OBBCollisionHull2D other, out NCollision2D c)
        {
            throw new System.NotImplementedException();
        }
    }
}
// Axis-Aligned-Bounding-Box Collision Hull for 2D Space
// Same Axes as World Axes

using UnityEngine;
using System.Collections.Generic;
using Physics2D;

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

        public override bool TestCollisionVsCircle(CircleCollisionHull2D other, out NCollision collision)
        {
            /// <see cref="CircleCollisionHull2D.TestCollisionVsAABB(AABBCollisionHull2D)"/>

            // Create a new NCollision
            // Assign hulls and instantiate the contact list
            // Status defaults to false
            collision = new NCollision
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
                collision.status = true;

            NCollision.Contact contact = new NCollision.Contact
            {
                pointOfContact = closestPoint,
                coeffRestitution = 0.25f,
                normal = (collision.hullOne.particle.movement.position - collision.hullTwo.particle.movement.position).normalized,
                depth = distance
            };

            collision.contact.Add(contact);

            // Finish setting up the collision
            collision.contactCount = collision.contact.Count;

            return collision.status;
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull2D other, out NCollision collision)
        {
            // Pass Condition: If, for all axes (X & Y), the MaxExtent of This is overlapping the MinExtent of Other

            // Create a new NCollision
            // Assign hulls and instantiate the contact list
            // Status defaults to false
            collision = new NCollision
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
                collision.status = true;

            NCollision.Contact contactOne = new NCollision.Contact
            {
                pointOfContact = (minExtent - otherMin),
                coeffRestitution = 0.25f,
                depth = (minExtent - otherMin).sqrMagnitude,
                normal = (collision.hullOne.particle.movement.position - collision.hullTwo.particle.movement.position).normalized
            };

            NCollision.Contact contactTwo = new NCollision.Contact
            {
                pointOfContact = (maxExtent - otherMax),
                coeffRestitution = 0.25f,
                depth = (maxExtent - otherMax).sqrMagnitude,
                normal = (collision.hullOne.particle.movement.position - collision.hullTwo.particle.movement.position).normalized
            };

            collision.contact.Add(contactOne);
            collision.contact.Add(contactTwo);

            // Finish setting up the collision
            collision.contactCount = collision.contact.Count;

            return collision.status;
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

        public override bool TestCollisisionVsOBB(OBBCollisionHull2D other, out NCollision c)
        {
            throw new System.NotImplementedException();
        }
    }
}
// Circle Collision Hull for 2D Space

using UnityEngine;
using System.Collections.Generic;
using Physics2D;

namespace NS_Collision
{
    public class CircleCollisionHull2D : CollisionHull2D
    {
        public float radius
        {
            get; private set;
        }

        // Architecture Style 2 //
        public override bool TestCollisionVsCircle(CircleCollisionHull2D other, out NCollision collision)
        {
            // collision if distance between centers is <= sum of radii
            // optimized collision if (distance between centers)^2 <= (sum of radii)^2       

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
                collision.status = true;
            }

            NCollision.Contact contact = new NCollision.Contact
            {
                
            };

            collision.contact.Add(contact);

            // Finish setting up the collision
            collision.contactCount = collision.contact.Count;

            return collision.status;
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull2D other, out NCollision collision)
        {
            // find the closest point to the cicle on the box
            // done by clamping center of circle to be within box dimensions
            // if closest point is within circle, pass (do point vs circle collision test)

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

            // Step 05: Check to see if we're in contact
            if (distance < radius * radius)
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

        public override bool TestCollisisionVsOBB(OBBCollisionHull2D other, out NCollision c)
        {
            throw new System.NotImplementedException();
        }
    }
}
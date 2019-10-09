using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Collision
{
    public class CollisionResolutionManager : MonoBehaviour
    {
        // A collision is an event (two objects touching)
        // Lab 05 Step 01
        // Called NCollision b/c Collision & Collision2D are Unity types
        public struct NCollision
        {
            // References to hulls involved
            public CollisionHull2D hullOne, hullTwo;

            // Collision Status (did it happen?)
            public bool status;

            // A contact is the point(s) at which a collision occurs
            public struct Contact
            {
                // Contact Location
                public Vector2 pointOfContact;
                // Normal
                public Vector2 normal;
                // Coefficient of Restitution
                public float coeffRestitution;
                // Collision Depth
                public float depth;
            }

            // List of contacts for this collision
            public List<Contact> contact;

            // Number of contacts (size of contact list)
            public int contactCount;

            // Velocity between two objects approaching one another
            public Vector2 closingVelocity;
        }

        // Formula from Millington 2nd Ed. pg. 120
        public static float CalcSeparatingVel(NCollision collision, NCollision.Contact contact)
        {
            CollisionHull2D partOne = collision.hullOne;
            CollisionHull2D partTwo = collision.hullTwo;

            // Calculate relative velocity of both particles
            Vector2 relativeVel = partOne.particle.movement.velocity - partTwo.particle.movement.velocity;

            // TODO: FIX THIS FUNCTION
            return 0.0f;

            // return relativeVel * contact.normal;
        }

        // Formulae from Millington 2nd Ed. pg. 120-121
        public static void ResolveVelocity(NCollision collision)
        {
            // Create a list of separating velocities
            List<float> sepVelList = new List<float>();

            // For each contact in the passed collision, calculate the separating velocity
            // and add it to a list
            foreach (NCollision.Contact contact in collision.contact)
            {
                sepVelList.Add(CalcSeparatingVel(collision, contact));
            }

            // If there is only one separating velocity and it is greater than 0, no impulse is required
            if (sepVelList.Count == 1 && sepVelList[0] > 0.0f)
                return;

        }
    }
}

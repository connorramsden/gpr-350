using System.Collections.Generic;
using UnityEngine;

namespace NS_Collision
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
        public float closingVelocity;
    }

    public class CollisionResolutionManager : MonoBehaviour
    {
        // Formula from Millington 2nd Ed. pg. 114
        private static float CalcClosingVel(Particle2DComponent partOne, Particle2DComponent partTwo)
        {
            // vc = NEG(velOne - velTwo) DOT norm(posOne - posTwo) 

            // Step 01: Get relative velocity (velOne - velTwo)
            Vector2 relativeVelocity = partOne.movement.velocity - partTwo.movement.velocity;
            // Step 02.a: Get relative position (posOne - posTwo)
            Vector2 relativePosition = partOne.GetPosition() - partTwo.GetPosition();
            // Step 02.b: Normalize the relative position
            relativePosition.Normalize();
            // Step 03: Calculate closing velocity
            return -Vector2.Dot(relativeVelocity, relativePosition);
        }

        // Formulae from Millington 2nd Ed. pg. 120-121
        public static void ResolveVelocity(NCollision c)
        {
            Particle2DComponent partOne = c.hullOne.particle;
            Particle2DComponent partTwo = c.hullTwo.particle;

            // Assuming one point of contact
            // TODO: FIX LATER
            NCollision.Contact contact = c.contact[0];

            // Find the velocity in the direction of the contact
            c.closingVelocity = CalcClosingVel(partOne, partTwo);

            // Check if the collision needs to be resolved
            if(c.closingVelocity > 0)
            {
                // If the contact is separating or stationary, no impulse is required
                return;
            }

            // Calculate new separating velocity
            float newClosingVelcoity = -c.closingVelocity * contact.coeffRestitution;
            float deltaVelocity = newClosingVelcoity - c.closingVelocity;

            // Apply the change in velocity to each object in proportion to
            // their inverse mass (i.e., those with loewr inverse mass [higher mass] get less change in velocity).
            float totalInverseMass = partOne.massInv + partTwo.massInv;

            // If all particles have infinite mass, then impulses have no effect.
            if (totalInverseMass <= 0.0f)
                return;

            // Calculate the impulse to apply
            float impulse = deltaVelocity / totalInverseMass;
            
            // Find the amount of impulse per unit of inverse mass
            Vector2 impulsePerInvMass = contact.normal * impulse;

            // Apply impulses: they are applied in the direction of the contact,
            // and are proportional to the inverse mass
            partOne.movement.velocity = partOne.movement.velocity + impulsePerInvMass * partOne.massInv;
            partTwo.movement.velocity = partTwo.movement.velocity + impulsePerInvMass * -partTwo.massInv;
        }
    }
}

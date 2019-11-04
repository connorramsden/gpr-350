using System.Collections.Generic;
using UnityEngine;
using NS_Physics2D;

namespace NS_Collision_2D
{
    // A collision2D is an event (two objects touching)
    // Lab 05 Step 01
    // Called NCollision2D b/c Collision & Collision2D are Unity types
    public struct NCollision2D
    {
        // References to hulls involved
        public CollisionHull2D hullOne, hullTwo;

        // Collision Status (did it happen?)
        public bool status;

        // A contact is the point(s) at which a collision2D occurs
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

        // List of contacts for this collision2D
        public List<Contact> contact;

        // Number of contacts (size of contact list)
        public int contactCount;

        // Velocity between two objects approaching one another
        public float closingVelocity;
    }

    public class CollisionResolutionManager : MonoBehaviour
    {
        // Formula from Millington 2nd Ed. pg. 114
        public static float CalcClosingVel(Particle2DComponent partOne, Particle2DComponent partTwo)
        {
            // vc = NEG(velOne - velTwo) DOT norm(posOne - posTwo) 
            // Step 01: Get relative velocity (velOne - velTwo)
            Vector2 relativeVelocity = partOne.movement.velocity - partTwo.movement.velocity;
            // Step 02: Get relative position norm(posOne - posTwo)
            Vector2 relativePosition = (partOne.GetPosition() - partTwo.GetPosition()).normalized;
            // Step 03: Calculate closing velocity
            return -Vector2.Dot(relativeVelocity, relativePosition);
        }

        // Formulae from Millington 2nd Ed. pg. 120-121
        private static void ResolveVelocity(NCollision2D c, float duration)
        {
            Particle2DComponent partOne = c.hullOne.particle;
            Particle2DComponent partTwo = c.hullTwo.particle;

            // Assuming one point of contact
            NCollision2D.Contact contact = c.contact[0];

            // Check if the collision2D needs to be resolved
            if (c.closingVelocity > 0)
            {
                // If the contact is separating or stationary, no impulse is required
                return;
            }

            // Calculate new separating velocity
            float newClosingVelcoity = -c.closingVelocity * contact.coeffRestitution;

            Vector2 accCausedVelocity = partOne.movement.acceleration - partTwo.movement.acceleration;
            float accCausedClosingVelocity = Vector2.Dot(accCausedVelocity, contact.normal) * duration;

            if (accCausedClosingVelocity < 0)
            {
                newClosingVelcoity += contact.coeffRestitution * accCausedClosingVelocity;

                if (newClosingVelcoity < 0)
                    newClosingVelcoity = 0;
            }

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

        private static void ResolveInterpenetration(NCollision2D collision2D, float duration)
        {
            Particle2DComponent partOne = collision2D.hullOne.particle;
            Particle2DComponent partTwo = collision2D.hullTwo.particle;

            // The movement of each object is based on their inverse mass
            // So, total that value
            float totalInverseMass = partOne.massInv + partTwo.massInv;

            // If all particles have infinite mass, then we do nothing
            if (totalInverseMass <= 0)
                return;

            // Check out results for each contact in the collision2D contact list
            foreach (NCollision2D.Contact contact in collision2D.contact)
            {
                // Continue to the next contact if we have no penetration
                if (contact.depth <= 0)
                    continue;

                // Find the amount of penetration resolution per unit of inverse mass
                Vector2 movePerIMass = contact.normal * (contact.depth / totalInverseMass);

                // Calculate the movement amounts
                Vector2 partOneMoveAmount = movePerIMass * partOne.massInv;
                Vector2 partTwoMoveAmount = movePerIMass * -partTwo.massInv;

                partOne.movement.position = partOne.movement.position + partOneMoveAmount;
                partTwo.movement.position = partTwo.movement.position + partTwoMoveAmount;
            }

        }

        // Check to ensure that a collision2D has been properly set up.
        private static bool VerifyCollision(NCollision2D collision2D)
        {
            // ensure the collision2D has a valid closing velocity
            if (collision2D.closingVelocity.Equals(null))
            {
                Debug.LogError("Collision invalid: no closing velocity");
                return false;
            }
            // Ensure the collision2D has both hulls assigned
            if (!collision2D.hullOne && !collision2D.hullTwo)
            {
                Debug.LogError("Collision invalid: hulls not properly assigned");
                return false;
            }
            // Ensure the collision2D has at least one contact point
            if (collision2D.contactCount <= 0)
            {
                Debug.LogError("Collision invalid: no contact points assigned");
                return false;
            }
            
            foreach(NCollision2D.Contact contact in collision2D.contact)
            {
                if(contact.depth.Equals(null))
                {
                    Debug.LogError("Collision invalid: improper depth");
                    return false;
                }
                if(contact.normal.magnitude.Equals(null))
                {
                    Debug.LogError("Collision invalid: improper normal");
                    return false;
                }
                if(contact.pointOfContact.magnitude.Equals(null))
                {
                    Debug.LogError("Collision invalid: improper point of contact");
                    return false;
                }
            }

            // Return true if all checks have passed
            return true;
        }

        public static void ResolveCollision(NCollision2D collision2D, float duration)
        {
            // Ensure the collision2D is valid
            if (!VerifyCollision(collision2D))
                return;

            ResolveVelocity(collision2D, duration);
            ResolveInterpenetration(collision2D, duration);
        }
    }
}

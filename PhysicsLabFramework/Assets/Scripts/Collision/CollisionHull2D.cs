using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2DComponent))]
public abstract class CollisionHull2D : MonoBehaviour
{
    // A collision is an event (two objects touching)
    // Lab 05 Step 01
    public struct Collision
    {
        // References to hulls involved
        public CollisionHull2D hullOne, hullTwo;

        // Collision Status (did it happen?)
        public bool status;

        // A contact is the point(s) at which a collision occurs
        public struct Contact
        {
            // Location
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

    // All collision types must be enumerators in this set
    public enum CollisionHullType2D
    {
        INVALID_TYPE = -1,
        HULL_CIRCLE,
        HULL_AABB,
        HULL_OBB,
    }

    // Public getter, private setter
    public CollisionHullType2D type
    {
        get; private set;
    }

    protected void SetType(CollisionHullType2D newType)
    {
        type = newType;
    }

    public Particle2DComponent particle
    {
        get; protected set;
    }

    // Architecture Style 1 //
    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, out Collision c)
    {
        c = new Collision();
        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other, out Collision c);

    public abstract bool TestCollisionVsAABB(AABBCollisionHull2D other, out Collision c);

    public abstract bool TestCollisionVsOBB(OBBCollisionHull2D other, out Collision c);

    public abstract void UpdateCenterPos();

    // Formula from Millington 2nd Ed. pg. 120
    public static float CalcSeparatingVel(Collision collision, Collision.Contact contact)
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
    public static void ResolveVelocity(Collision collision)
    {
        // Create a list of separating velocities
        List<float> sepVelList = new List<float>();

        // For each contact in the passed collision, calculate the separating velocity
        // and add it to a list
        foreach (Collision.Contact contact in collision.contact)
        {
            sepVelList.Add(CalcSeparatingVel(collision, contact));
        }

        // If there is only one separating velocity and it is greater than 0, no impulse is required
        if (sepVelList.Count == 1 && sepVelList[0] > 0.0f)
            return;

    }

    private void Awake()
    {
        particle = gameObject.GetComponent<Particle2DComponent>();
    }
}

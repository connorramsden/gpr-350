using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Particle2DComponent))]
public abstract class CollisionHull2D : MonoBehaviour
{
    // A collision is an event (two objects touching)
    public struct Collision
    {
        // A contact is the point(s) at which a collision occurs
        public struct Contact
        {
            public Vector2 pointOfContact;
            public Vector2 normal;
            public float coeffRestitution;
        }

        // List of contacts for this collision
        public List<Contact> contact;
        // Number of contacts (size of contact list)
        public int contactCount;

        // Hulls that are colliding
        public CollisionHull2D a, b;
        // True = collision, false = no collision
        public bool status;

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

    protected Particle2DComponent particle;

    // Architecture Style 1 //
    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {

        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c);

    public abstract bool TestCollisionVsAABB(AABBCollisionHull2D other, ref Collision c);

    public abstract bool TestCollisionVsOBB(OBBCollisionHull2D other, ref Collision c);
}

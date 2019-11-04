using UnityEngine;
using NS_Physics2D;

namespace NS_Collision_2D
{
    [RequireComponent(typeof(Particle2DComponent))]
    public abstract class CollisionHull2D : MonoBehaviour
    {
        // Possible Hull types
        public enum CollisionHullType2D
        {
            INVALID_TYPE = -1,
            HULL_CIRCLE,
            HULL_AABB_2D,
            HULL_OBB_2D,
        }

        // Holds the CH2D's type
        public CollisionHullType2D type
        {
            get; protected set;
        }

        // Holds the actual 'body' the CH2D sits on
        public Particle2DComponent particle
        {
            get; protected set;
        }

        // Holds the CH2d's particle's center position
        public virtual Vector2 center
        {
            get; protected set;
        }

        public abstract bool TestCollisionVsCircle(CircleCollisionHull other, out NCollision2D c);

        public abstract bool TestCollisionVsAABB(AABBCollisionHull2D other, out NCollision2D c);

        public abstract bool TestCollisisionVsOBB(OBBCollisionHull2D other, out NCollision2D c);

        public virtual void UpdateCenterPos()
        {
            if (particle)
                center = particle.GetPosition();
        }

        private void Awake()
        {
            particle = gameObject.GetComponent<Particle2DComponent>();
        }
    }
}
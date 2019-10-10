using System.Collections.Generic;
using UnityEngine;

namespace NS_Collision
{
    [RequireComponent(typeof(Particle2DComponent))]
    public abstract class CollisionHull2D : MonoBehaviour
    {
        // Possible Hull types
        public enum CollisionHullType2D
        {
            INVALID_TYPE = -1,
            HULL_CIRCLE,
            HULL_AABB,
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

        public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other, out NCollision c);

        public abstract bool TestCollisionVsAABB(AABBCollisionHull2D other, out NCollision c);

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
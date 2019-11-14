using NS_Physics3D;
using UnityEngine;

namespace NS_Collision_3D
{
    [RequireComponent(typeof(Particle3D))]
    public abstract class CollisionHull3D : MonoBehaviour
    {
        public enum CollisionHullType3D
        {
            INVALID_TYPE = -1,
            HULL_SPHERE,
            HULL_AABB_3D,
            HULL_OBB_3D
        }

        public CollisionHullType3D hullType { get; protected set; }

        protected Particle3D p3d;

        public Vector3 hullCenter { get; private set; }

        public bool isCollidingVsSphere = false;
        public bool isCollidingVsAABB = false;
        public bool isCollidingVsOBB = false;
        
        
        public abstract bool TestCollisionVsSphere(SphereCollisionHull other);
        public abstract bool TestCollisionVsAABB(AABBCollisionHull3D other);
        public abstract bool TestCollisionVsOBB(OBBCollisionHull3D other);

        protected float GetRadius()
        {
            return p3d.radius;
        }

        public float GetRadiusSqr()
        {
            return p3d.radius * p3d.radius;
        }

        public void UpdateCenterPos()
        {
            if (p3d)
                hullCenter = p3d.position;
        }
    }
}

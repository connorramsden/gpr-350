using System;
using NS_Physics3D;
using static Phys.CH3D.Types;
using UnityEngine;

namespace NS_Collision_3D
{
    [RequireComponent(typeof(Particle3D))]
    [Serializable]
    public abstract class CollisionHull3D : MonoBehaviour
    {
        /*
        public HullType hullType { get; protected set; }
        
        protected Particle3D p3d;

        public Vector3 hullCenter { get; private set; }
        
        public bool isCollidingVsSphere;
        public bool isCollidingVsAABB;
        public bool isCollidingVsOBB;
        public bool isColliding;

        public abstract bool TestCollisionVsSphere(SphereCollisionHull other);
        public abstract bool TestCollisionVsAABB(AABBCollisionHull3D other);
        public abstract bool TestCollisionVsOBB(OBBCollisionHull3D other);

        protected float GetRadius()
        {
            return p3d.radius;
        }

        protected float GetRadiusSqr()
        {
            return p3d.radius * p3d.radius;
        }

        public void UpdateCenterPos()
        {
            if (p3d)
                hullCenter = p3d.position;
        }*/
        
        
    }
}
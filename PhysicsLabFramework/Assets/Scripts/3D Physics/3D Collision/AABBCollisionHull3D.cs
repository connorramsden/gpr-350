using System;
using NS_Physics3D;
using UnityEngine;

namespace NS_Collision_3D
{
    [Serializable]
    public class AABBCollisionHull3D : CollisionHull3D
    {
        private Vector3 halfSize { get; set; }
        public Vector3 minExtent { get; private set; }
        public Vector3 maxExtent { get; private set; }

        public SVector3 Min;
        public SVector3 Max;

        public override bool TestCollisionVsSphere(SphereCollisionHull other)
        {
            // Utilize the Sphere's test vs AABB against this Box
            return other.TestCollisionVsAABB(this);
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull3D other)
        {
            Vector3 otherMin = other.minExtent;
            Vector3 otherMax = other.maxExtent;

            bool diffX = otherMin.x < maxExtent.x && minExtent.x < otherMax.x;
            bool diffY = otherMin.y < maxExtent.y && minExtent.y < otherMax.y;
            bool diffZ = otherMin.z < maxExtent.z && minExtent.z < otherMax.z;

            return diffX && diffY && diffZ;
        }

        public override bool TestCollisionVsOBB(OBBCollisionHull3D other)
        {
            throw new NotImplementedException();
        }

        public void UpdateExtents()
        {
            halfSize = new Vector3(0.5f * p3d.width, 0.5f * p3d.height, 0.5f * p3d.depth);

            minExtent = hullCenter - halfSize;
            maxExtent = hullCenter + halfSize;

            Min = minExtent;
            Max = maxExtent;
        }

        private void Awake()
        {
            hullType = CollisionHullType3D.HULL_AABB_3D;
            p3d = GetComponent<Particle3D>();

            UpdateCenterPos();
            UpdateExtents();
        }
    }
}
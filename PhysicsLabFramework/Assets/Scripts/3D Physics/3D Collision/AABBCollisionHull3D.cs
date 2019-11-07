﻿using NS_Physics3D;
using UnityEngine;

namespace NS_Collision_3D
{
    public class AABBCollisionHull3D : CollisionHull3D
    {
        public Vector3 halfSize { get; private set; }
        public Vector3 minExtent { get; private set; }
        public Vector3 maxExtent { get; private set; }

        public override bool TestCollisionVsSphere(SphereCollisionHull other)
        {
            Vector3 otherCenter = other.hullCenter;

            float xPosClamp = Mathf.Clamp(otherCenter.x, minExtent.x, maxExtent.x);
            float yPosClamp = Mathf.Clamp(otherCenter.y, minExtent.y, maxExtent.y);
            float zPosClamp = Mathf.Clamp(otherCenter.z, minExtent.z, maxExtent.z);

            var closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);
            float distance = (closestPoint - otherCenter).sqrMagnitude;

            return distance < other.GetRadiusSqr();
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
            throw new System.NotImplementedException();
        }

        public void UpdateExtents()
        {
            halfSize = new Vector3(0.5f * p3d.width, 0.5f * p3d.height, 0.5f * p3d.depth);

            minExtent = hullCenter - halfSize;
            maxExtent = hullCenter + halfSize;
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
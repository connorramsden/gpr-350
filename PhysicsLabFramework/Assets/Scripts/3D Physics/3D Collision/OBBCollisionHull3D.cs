using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Physics3D;

namespace NS_Collision_3D
{
    public class OBBCollisionHull3D : CollisionHull3D
    {
        public override bool TestCollisionVsSphere(SphereCollisionHull other)
        {
            throw new System.NotImplementedException();
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull3D other)
        {
            throw new System.NotImplementedException();
        }

        public override bool TestCollisionVsOBB(OBBCollisionHull3D other)
        {
            throw new System.NotImplementedException();

        }

        private void Awake()
        {
            hullType = CollisionHullType3D.HULL_OBB_3D;
            p3d = GetComponent<Particle3D>();

            UpdateCenterPos();
        }
    }
}

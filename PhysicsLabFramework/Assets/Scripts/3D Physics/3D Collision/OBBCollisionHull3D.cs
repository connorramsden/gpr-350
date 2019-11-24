using System;
using NS_Physics3D;
using UnityEngine;
using static Phys.CH3D.Types;

namespace NS_Collision_3D
{
    [Serializable]
    public class OBBCollisionHull3D : CollisionHull3D
    {
        /*
        private Vector3 halfSize { get; set; }
        public Vector3 minExtentLocal { get; private set; }
        public Vector3 maxExtentLocal { get; private set; }
        public Vector3 minExtentWorld { get; private set; }
        public Vector3 maxExtentWorld { get; private set; }
        
        public override bool TestCollisionVsSphere(SphereCollisionHull other)
        {
            return other.TestCollisionVsOBB(this);
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull3D other)
        {
            throw new System.NotImplementedException();
        }

        public override bool TestCollisionVsOBB(OBBCollisionHull3D other)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateExtents()
        {
            
        }

        private void Awake()
        {
            hullType = HullType.HullObb;
            p3d = GetComponent<Particle3D>();

            UpdateCenterPos();
            UpdateExtents();
        }
        */
    }
}
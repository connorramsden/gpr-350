using NS_Physics3D;
using UnityEngine;

namespace NS_Collision_3D
{
    public class SphereCollisionHull : CollisionHull3D
    {
        public override bool TestCollisionVsSphere(SphereCollisionHull other)
        {
            Debug.Log("Call from inside SphereVsSphere");
            Vector3 distance = hullCenter - other.hullCenter;

            float distSq = Vector3.Dot(distance, distance);
            float radSum = GetRadius() + other.GetRadius();
            float radSumSq = radSum * radSum;

            return distSq <= radSumSq;
        }

        public override bool TestCollisionVsAABB(AABBCollisionHull3D other)
        {
            Debug.Log("Call from inside SphereVsAABB");
            Vector3 min = other.minExtent;
            Vector3 max = other.maxExtent;

            float xPosClamp = Mathf.Clamp(hullCenter.x, min.x, max.x);
            float yPosClamp = Mathf.Clamp(hullCenter.y, min.y, max.y);
            float zPosClamp = Mathf.Clamp(hullCenter.z, min.z, max.z);

            Vector3 closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);

            float distance = (closestPoint - hullCenter).sqrMagnitude;

            return distance < GetRadiusSqr();
        }

        public override bool TestCollisionVsOBB(OBBCollisionHull3D other)
        {
            throw new System.NotImplementedException();
        }

        private void Awake()
        {
            hullType = CollisionHullType3D.HULL_SPHERE;
            p3d = GetComponent<Particle3D>();
            UpdateCenterPos();
        }
    }
}
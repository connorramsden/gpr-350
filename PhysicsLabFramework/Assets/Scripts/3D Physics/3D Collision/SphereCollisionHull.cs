using System;
using NS_Physics3D;
using static Phys.CH3D.Types;
using UnityEngine;

namespace NS_Collision_3D
{
    [Serializable]
    public class SphereCollisionHull : CollisionHull3D
    {
        /*
        // Test collision between this Sphere and another Sphere
        public override bool TestCollisionVsSphere(SphereCollisionHull other)
        {
            // Step 01: Get the distance between the other Sphere and this Sphere
            Vector3 distance = hullCenter - other.hullCenter;
            // Step 02: Square the retrieved distance
            float distSq = Vector3.Dot(distance, distance);
            // Step 03: Get the sum of this Sphere's radius, and the other Sphere's radius
            float radSum = GetRadius() + other.GetRadius();
            // Step 04: Square the retrieved radii sum
            float radSumSq = radSum * radSum;

            // Return true if the squared distance is less than the squared radii sum
            return distSq <= radSumSq;
        }

        // Test collision between this Sphere and an Axis-Aligned Bounding Box
        public override bool TestCollisionVsAABB(AABBCollisionHull3D other)
        {
            // Step 01: Retrieve the Box's minimum and maximum extents
            Vector3 min = other.minExtent;
            Vector3 max = other.maxExtent;

            // Step 02: Clamp this Sphere's center within the extents of the Box
            float xPosClamp = Mathf.Clamp(hullCenter.x, min.x, max.x);
            float yPosClamp = Mathf.Clamp(hullCenter.y, min.y, max.y);
            float zPosClamp = Mathf.Clamp(hullCenter.z, min.z, max.z);

            // Step 03: Calculate the closest point between this Sphere and the Box utilizing the clamped points
            Vector3 closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);

            // Step 04: Calculate the distance between the closest point and this Sphere's center
            float distance = (closestPoint - hullCenter).sqrMagnitude;

            // Return true if the distance is less than the squared value of this Sphere's radii
            return distance < GetRadiusSqr();
        }

        // Test collision between this Sphere and an Object-Aligned Bounding Box
        public override bool TestCollisionVsOBB(OBBCollisionHull3D other)
        {
            // Same as vs AABB, but with a few more steps

            // Step 01: Retrieve the Box's minimum and maximum extents
            Vector3 min = other.minExtentLocal;
            Vector3 max = other.maxExtentLocal;

            // Step 02: Transform this Sphere's center into the Box's local space
            Vector3 circleCenter = other.transform.InverseTransformPoint(hullCenter);

            // Step 03: Get the combined vector of the newly calculated circle center, and the Box's local-space center
            circleCenter += other.hullCenter;

            // Step 04: Familiar territory, clamp this Sphere's center within the extents of the Box
            float xPosClamp = Mathf.Clamp(circleCenter.x, min.x, max.x);
            float yPosClamp = Mathf.Clamp(circleCenter.y, min.y, max.y);
            float zPosClamp = Mathf.Clamp(circleCenter.z, min.z, max.z);

            // Step 05: Calculate the closest point between this Sphere and the Box utilizing the clamped points
            Vector3 closestPoint = new Vector3(xPosClamp, yPosClamp, zPosClamp);

            // Step 06: Calculate the distance between the closest point and this Sphere's center, relative to the Box.
            float distance = (closestPoint - circleCenter).sqrMagnitude;

            // Return true if the distance is less than the squared value of this Sphere's radii
            return distance < GetRadiusSqr();
        }

        // Initialize internal variables
        private void Awake()
        {
            hullType = HullType.HullSphere;
            p3d = GetComponent<Particle3D>();
            UpdateCenterPos();
        }
        */
    }
}
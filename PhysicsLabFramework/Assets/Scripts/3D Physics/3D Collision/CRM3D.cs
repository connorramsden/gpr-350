using System.Collections.Generic;
using UnityEngine;

namespace NS_Collision_3D
{
    // 3D Collision Resolution Manager
    public class CRM3D : MonoBehaviour
    {
        // Create a private, static instance of CollisionResolutionManager3D
        private static CRM3D instance;

        // Ensure that only one instance of CollisionResolutionManager3D will ever exist
        public static CRM3D Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CRM3D>();
                }
                else if (instance != FindObjectOfType<CRM3D>())
                {
                    Destroy(FindObjectOfType<CRM3D>());
                }

                return instance;
            }
        }

        // A list to store all 3D Collision Hulls in a scene
        private List<CollisionHull3D> collisionList;

        // Add a hull to the hull list; called from every 3D Hull's Start()
        public void AddHullToList(CollisionHull3D hull)
        {
            collisionList.Add(hull);
        }

        // Check collisions for every 3D Hull in the hull list
        private void CheckCollisions()
        {
            // Loop over collision list once
            foreach (CollisionHull3D hull in collisionList)
            {
                // Update the hull's center position
                hull.UpdateCenterPos();

                if (hull.hullType == CollisionHull3D.CollisionHullType3D.HULL_AABB_3D)
                    (hull as AABBCollisionHull3D).UpdateExtents();

                // Loop over collision list (all OTHER hulls)
                foreach (CollisionHull3D otherHull in collisionList)
                {
                    // Never check this hull for collisions with itself
                    if (hull == otherHull) continue;

                    // Run specific collision detection depending on the otherHull's type
                    switch (otherHull.hullType)
                    {
                        case CollisionHull3D.CollisionHullType3D.HULL_SPHERE:
                            hull.isCollidingVsSphere = hull.TestCollisionVsSphere(otherHull as SphereCollisionHull);
                            break;
                        case CollisionHull3D.CollisionHullType3D.HULL_AABB_3D:
                            hull.isCollidingVsAABB = hull.TestCollisionVsAABB(otherHull as AABBCollisionHull3D);
                            break;
                        case CollisionHull3D.CollisionHullType3D.HULL_OBB_3D:
                            hull.isCollidingVsOBB = hull.TestCollisionVsOBB(otherHull as OBBCollisionHull3D);
                            break;
                    }
                }

                hull.isColliding = hull.isCollidingVsSphere || hull.isCollidingVsAABB || hull.isCollidingVsOBB;

                PhysicsSerializer.SerializeHull(hull);
                Debug.Log(RustPlugin.test_bool());
            }
        }

        // Initialize local variables
        private void Awake()
        {
            collisionList = new List<CollisionHull3D>();
        }

        // Perform operations on a fixed-time basis
        public void FixedUpdate()
        {
            CheckCollisions();
        }
    }
}
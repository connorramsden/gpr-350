using System;
using System.Collections.Generic;
using NS_Physics3D;
using UnityEngine;

namespace NS_Collision_3D
{
    // 3D Collision Resolution Manager
    public class CRM3D : MonoBehaviour
    {
        private static CRM3D instance;

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

        private List<CollisionHull3D> collisionList;

        public void AddHullToList(CollisionHull3D hull)
        {
            collisionList.Add(hull);
        }

        private void CheckCollisions()
        {
            // Loop over collision list once
            foreach (CollisionHull3D hull in collisionList)
            {
                hull.UpdateCenterPos();
                if (hull.hullType == CollisionHull3D.CollisionHullType3D.HULL_AABB_3D)
                    (hull as AABBCollisionHull3D)?.UpdateExtents();

                // Loop over collision list (all OTHER hulls)
                foreach (CollisionHull3D otherHull in collisionList)
                {
                    if (hull == otherHull) continue;

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
            }
        }

        private void Awake()
        {
            collisionList = new List<CollisionHull3D>();
        }

        public void FixedUpdate()
        {
            CheckCollisions();
        }
    }
}
using System.Collections.Generic;
using System.IO;
using Phys;
using UnityEngine;
using Google.Protobuf;
using Google.Protobuf.Collections;

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
        // private List<CollisionHull3D> collisionList;

        private List<CH3D> collisionList;

        // Add a hull to the hull list; called from every 3D Hull's Start()
        public void AddHullToList(CH3D hull)
        {
            collisionList.Add(hull);
        }

        // Check collisions for every 3D Hull in the hull list
        private void CheckCollisions()
        {
            // Loop over collision list once
            foreach (CH3D hull in collisionList)
            {
                byte[] bytes;
                
                
                
                using (MemoryStream stream = new MemoryStream())
                {
                    hull.WriteTo(stream);
                    bytes = stream.ToArray();
                }

                Debug.Log(RustPlugin.check_collisions(bytes));

                /*
                // Update the hull's center position
                hull.UpdateCenterPos();

                if (hull.HullType == HullType.HullAabb)
                    (hull as AABBCollisionHull3D).UpdateExtents();

                // Loop over collision list (all OTHER hulls)
                foreach (CH3D otherHull in collisionList)
                {
                    // Never check this hull for collisions with itself
                    if (hull.Equals(otherHull)) continue;

                    // Run specific collision detection depending on the otherHull's type
                    switch (otherHull.HullType)
                    {
                        case HullType.HullSphere:
                            hull.isCollidingVsSphere = hull.TestCollisionVsSphere(otherHull as SphereCollisionHull);
                            break;
                        case HullType.HullAabb:
                            hull.isCollidingVsAABB = hull.TestCollisionVsAABB(otherHull as AABBCollisionHull3D);
                            break;
                        case HullType.HullObb:
                            hull.isCollidingVsOBB = hull.TestCollisionVsOBB(otherHull as OBBCollisionHull3D);
                            break;
                    }
                }

                hull.IsColliding = hull.IsCollidingVsSphere || hull.IsCollidingVsAABB || hull.IsCollidingVSOBB;
                */
            }
        }

        // Initialize local variables
        private void Awake()
        {
            collisionList = new List<CH3D>();
        }

        // Perform operations on a fixed-time basis
        public void FixedUpdate()
        {
            CheckCollisions();
        }
    }
}
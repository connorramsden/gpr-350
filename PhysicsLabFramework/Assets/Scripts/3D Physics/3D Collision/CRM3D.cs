using System;
using System.Runtime.InteropServices;
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

        private void Start()
        {
        }

        // Perform operations on a fixed-time basis
        public void FixedUpdate()
        {
        }
    }
}

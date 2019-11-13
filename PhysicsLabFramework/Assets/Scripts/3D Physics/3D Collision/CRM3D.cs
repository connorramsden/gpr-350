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

        private void Awake()
        {
            collisionList = new List<CollisionHull3D>();
        }

        public void Start()
        {
            CollisionHull3D[] collisionHulls = FindObjectsOfType<CollisionHull3D>();

            foreach (CollisionHull3D collisionHull in collisionHulls)
            {
                collisionList.Add(collisionHull);
            }
        }

        public void FixedUpdate()
        {
            foreach (CollisionHull3D hull in collisionList)
            {
                hull.isColliding = Input.GetKey(KeyCode.Space);
            }
        }
    }
}

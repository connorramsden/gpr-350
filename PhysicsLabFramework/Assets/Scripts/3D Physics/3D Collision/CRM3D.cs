using System;
using System.Collections.Generic;
using NS_Physics3D;
using UnityEngine;

namespace NS_Collision_3D
{
    // 3D Collision Resolution Manager
    public class CRM3D : MonoBehaviour
    {
        public List<Particle3D> particleList;

        private void Awake()
        {
            particleList = new List<Particle3D>();
        }

        private void Start()
        {
            var particles = GameObject.FindGameObjectsWithTag("3D Particle");

            foreach (GameObject particle in particles)
            {
                particleList.Add(particle.GetComponent<Particle3D>());
            }
        }

        private void FixedUpdate()
        {
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using NS_Collision_2D;

namespace NS_Physics2D
{
    public class Particle2DSystem : MonoBehaviour
    {
        private List<GameObject> particleList;

        // Trying to mimic ECS-style updating all entities in a single, system-based update call
        private void UpdateAllParticles(float dt)
        {
            foreach (GameObject particle in particleList)
            {
                Particle2DComponent p2d = particle.GetComponent<Particle2DComponent>();

                if (!particle) continue;
                if (!p2d.shouldMove)
                    continue;

                p2d.UpdatePosition(dt);
                p2d.ApplyForces();
                p2d.UpdateAcceleration();

                if (!p2d.shouldRotate) continue;

                p2d.UpdateRotation(dt);
                p2d.ApplyTorque();
                p2d.UpdateAngularAcceleration();
            }
        }

        // Check a the passed particle against all other particles for Circle Collisions
        private bool CheckCircleCollision(GameObject particleOne, out NCollision2D collision2D)
        {
            CircleCollisionHull cch2d = particleOne.GetComponent<CircleCollisionHull>();
            Particle2DComponent p2dOne = particleOne.GetComponent<Particle2DComponent>();

            bool isColliding = false;

            collision2D = new NCollision2D();

            // Iterate over the particleList and check vs all objects
            foreach (GameObject particleTwo in particleList)
            {
                // Don't let a particle collide with itself
                if (particleOne == particleTwo) continue;

                Particle2DComponent p2dTwo = particleTwo.GetComponent<Particle2DComponent>();

                if (particleTwo.TryGetComponent(out CircleCollisionHull otherCircle))
                {
                    isColliding = cch2d.TestCollisionVsCircle(otherCircle, out collision2D);
                }
                else if (particleTwo.TryGetComponent(out AABBCollisionHull2D otherAABB))
                {
                    isColliding = cch2d.TestCollisionVsAABB(otherAABB, out collision2D);
                }

                collision2D.closingVelocity = CollisionResolutionManager.CalcClosingVel(p2dOne, p2dTwo);
            }

            // If none of the above are true, will return false
            return isColliding;
        }

        // Check the passed particle against all other particles for Box Collisions (AABB & OBB)
        private bool CheckAABBCollision(GameObject particleOne, out NCollision2D collision2D)
        {
            AABBCollisionHull2D aabb2d = particleOne.GetComponent<AABBCollisionHull2D>();
            Particle2DComponent p2dOne = particleOne.GetComponent<Particle2DComponent>();

            bool isColliding = false;

            collision2D = new NCollision2D();

            foreach (GameObject particleTwo in particleList)
            {
                // Particles cannot collide with themselves
                if (particleOne == particleTwo) continue;

                Particle2DComponent p2dTwo = particleTwo.GetComponent<Particle2DComponent>();

                if (particleTwo.TryGetComponent(out CircleCollisionHull otherCircle))
                {
                    isColliding = aabb2d.TestCollisionVsCircle(otherCircle, out collision2D);
                }
                else if (particleTwo.TryGetComponent(out AABBCollisionHull2D otherAABB))
                {
                    isColliding = aabb2d.TestCollisionVsAABB(otherAABB, out collision2D);
                }

                collision2D.closingVelocity = CollisionResolutionManager.CalcClosingVel(p2dOne, p2dTwo);
            }

            // If none of the above are true, will return false
            return isColliding;
        }

        private void CheckCollisions()
        {
            foreach (GameObject particle in particleList)
            {
                NCollision2D collisionToResolve = new NCollision2D();

                // Establish a local boolean
                bool isColliding = false;

                // If the particle has a circle hull, check circle-collisions
                if (particle.TryGetComponent(out CircleCollisionHull circleHull))
                {
                    circleHull.UpdateCenterPos();
                    isColliding = CheckCircleCollision(particle, out collisionToResolve);
                }
                // If the particle has an AABB 
                else if (particle.TryGetComponent(out AABBCollisionHull2D aabbHull))
                {
                    aabbHull.UpdateCenterPos();
                    aabbHull.UpdateExtents();
                    isColliding = CheckAABBCollision(particle, out collisionToResolve);
                }

                if (isColliding)
                {
                    CollisionResolutionManager.ResolveCollision(collisionToResolve, 1.0f);
                }
            }
        }

        private void Awake()
        {
            // Initialize particleList
            particleList = new List<GameObject>();
        }

        private void Start()
        {
            var particles = GameObject.FindGameObjectsWithTag("Particle");

            // Add all particles
            foreach (GameObject particle in particles)
            {
                particleList.Add(particle);
            }

            foreach (GameObject particle in particleList)
            {
                Particle2DComponent p2d = particle.GetComponent<Particle2DComponent>();

                // Set all particle's starting mass to their editor values
                p2d.SetMass(p2d.GetStartingMass());
                p2d.SetInertia();
            }
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            UpdateAllParticles(dt);
            CheckCollisions();
        }
    }
}
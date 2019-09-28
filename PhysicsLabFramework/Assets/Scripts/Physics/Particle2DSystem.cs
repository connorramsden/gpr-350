using System.Collections.Generic;
using UnityEngine;

public class Particle2DSystem : MonoBehaviour
{
    List<GameObject> particleList;

    // Trying to mimic ECS-style updating all entities in a single, system-based update call
    private void UpdateAllParticles(float dt)
    {
        foreach (GameObject particle in particleList)
        {
            Particle2DComponent p2d = particle.GetComponent<Particle2DComponent>();

            if (p2d.shouldMove)
            {
                p2d.UpdatePosition(dt);
                p2d.ApplyForces();
                p2d.UpdateAcceleration();
            }
            if (p2d.shouldRotate)
            {
                p2d.UpdateRotation(dt);
                p2d.ApplyTorque();
                p2d.UpdateAngularAcceleration();
            }
        }
    }

    // Check a the passed particle against all other partciles for Circle Collisions
    private bool CheckCircleCollision(GameObject particle)
    {
        bool isColliding = false;
        // Get the passed particle's circle collision hull
        CircleCollisionHull2D cch2d = particle.GetComponent<CircleCollisionHull2D>();

        // Iterate over the particle list
        foreach (GameObject other in particleList)
        {
            // So long as the passed particle is not the 'other' particle (not the same particle)
            if (particle != other)
            {
                // Determine which collision type to call based on what component the 'other' particle has
                if (other.TryGetComponent(typeof(CircleCollisionHull2D), out Component circleHull))
                {
                    // Try a Circle v Circle collision
                    isColliding = cch2d.TestCollisionVsCircle(other.GetComponent<CircleCollisionHull2D>());
                }
                else if(other.TryGetComponent(typeof(AABBCollisionHull2D), out Component aabbHull))
                {
                    isColliding = cch2d.TestCollisionVsAABB(other.GetComponent<AABBCollisionHull2D>());
                }
                else if(other.TryGetComponent(typeof(OBBCollisionHull2D), out Component obbHull))
                {
                }
            }
        }

        return isColliding;
    }

    private void CheckAABBCollision(GameObject particle)
    {

    }

    private void CheckCollisions()
    {
        bool isColliding = false;

        foreach (GameObject particle in particleList)
        {
            isColliding = CheckCircleCollision(particle);

            if (isColliding)
            {
                particle.GetComponent<Particle2DComponent>().shouldMove = false;
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
        // Snag all GO's with tag Particle for particleList
        particleList.AddRange(GameObject.FindGameObjectsWithTag("Particle"));

        foreach (GameObject particle in particleList)
        {
            Particle2DComponent p2d = particle.GetComponent<Particle2DComponent>();

            // Set all particle's starting mass to their editor valus
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

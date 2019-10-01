using System.Collections.Generic;
using UnityEngine;

public class Particle2DSystem : MonoBehaviour
{
    List<GameObject> particleList;

    public Material redMat;
    public Material greenMat;

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
    private bool CheckCircleCollision()
    {
        return false;
    }

    // Check the passed particle against all other particles for Box Collisions (AABB & OBB)
    private bool CheckAABBCollision()
    {
        return false;
    }

    private void CheckCollisions()
    {
        foreach (GameObject particle in particleList)
        {
            // Get the particle's Particle2D Component
            Particle2DComponent p2d = particle.GetComponent<Particle2DComponent>();

            // Establish a local boolean
            bool isColliding = false;

            // If the particle has a circle hull, check circle-collisions
            if (particle.TryGetComponent(out CircleCollisionHull2D circleHull))
            {
                circleHull.UpdateCenterPos();

            }
            // If the particle has an AABB 
            else if (particle.TryGetComponent(out AABBCollisionHull2D aabbHull))
            {
                aabbHull.UpdateCenterPos();
                aabbHull.UpdateExtents();
            }

            if (isColliding)
            {
                p2d.SetMaterial(greenMat);
            }
            else
            {
                p2d.SetMaterial(redMat);
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

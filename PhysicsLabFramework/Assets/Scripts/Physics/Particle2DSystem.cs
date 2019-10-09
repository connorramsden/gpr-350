using System.Collections.Generic;
using UnityEngine;
using NS_Collision;
using static NS_Collision.CollisionResolutionManager;

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
    private bool CheckCircleCollision(GameObject particleOne)
    {
        CircleCollisionHull2D cch2d = particleOne.GetComponent<CircleCollisionHull2D>();

        bool isColliding = false;

        // Iterate over the particleList and check vs all objects
        foreach (GameObject particleTwo in particleList)
        {
            if (particleOne != particleTwo)
            {
                NCollision c = new NCollision();
                
                if (particleTwo.TryGetComponent(out CircleCollisionHull2D otherCircle))
                {
                    isColliding = cch2d.TestCollisionVsCircle(otherCircle, out c);
                }
                else if (particleTwo.TryGetComponent(out AABBCollisionHull2D otherAABB))
                {
                    isColliding = cch2d.TestCollisionVsAABB(otherAABB, out c);
                }
            }
        }

        // If none of the above are true, will return false
        return isColliding;
    }

    // Check the passed particle against all other particles for Box Collisions (AABB & OBB)
    private bool CheckAABBCollision(GameObject particleOne)
    {
        AABBCollisionHull2D aabb2d = particleOne.GetComponent<AABBCollisionHull2D>();
        bool isColliding = false;

        foreach (GameObject particleTwo in particleList)
        {
            NCollision c = new NCollision();
            
            if (particleTwo.TryGetComponent(out CircleCollisionHull2D otherCircle))
            {
                isColliding = aabb2d.TestCollisionVsCircle(otherCircle, out c);
            }
            else if (particleTwo.TryGetComponent(out AABBCollisionHull2D otherAABB))
            {
                isColliding = aabb2d.TestCollisionVsAABB(otherAABB, out c);
            }
        }

        // If none of the above are true, will return false
        return isColliding;
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
                isColliding = CheckCircleCollision(particle);
            }
            // If the particle has an AABB 
            else if (particle.TryGetComponent(out AABBCollisionHull2D aabbHull))
            {
                aabbHull.UpdateCenterPos();
                aabbHull.UpdateExtents();
                isColliding = CheckAABBCollision(particle);
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

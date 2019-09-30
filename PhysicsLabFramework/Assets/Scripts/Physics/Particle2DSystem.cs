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
                if (other.TryGetComponent(out CircleCollisionHull2D circleHull))
                {
                    // Try a Circle vs Circle collision
                    isColliding = cch2d.TestCollisionVsCircle(circleHull);
                }
                else if (other.TryGetComponent(out AABBCollisionHull2D aabbHull))
                {
                    // Try a Circle vs ABB collision
                    isColliding = cch2d.TestCollisionVsAABB(aabbHull);
                }
                else if (other.TryGetComponent(out OBBCollisionHull2D obbHull))
                {
                    // Try a Circle vs OBB collision
                    isColliding = cch2d.TestCollisionVsOBB(obbHull);
                }
            }
        }

        return isColliding;
    }

    // Check the passed particle against all other particles for Box Collisions (AABB & OBB)
    private bool CheckBoxCollision(GameObject particle)
    {
        bool isColliding = false;
        CollisionHull2D bb2d;

        if (particle.TryGetComponent(out AABBCollisionHull2D tryAabbHull))
        {
            bb2d = tryAabbHull;
        }
        else if (particle.TryGetComponent(out OBBCollisionHull2D tryObbHull))
        {
            bb2d = tryObbHull;
        }
        else{
            bb2d = null;
            Debug.LogError("Error in Box Collision Detection");
        }

        foreach (GameObject other in particleList)
        {
            if (particle != other)
            {
                if (other.TryGetComponent(out CircleCollisionHull2D circleHull))
                {
                    // Try a Circle vs Circle collision
                    isColliding = bb2d.TestCollisionVsCircle(circleHull);
                }
                else if (other.TryGetComponent(out AABBCollisionHull2D aabbHull))
                {
                    // Try a Circle vs ABB collision
                    isColliding = bb2d.TestCollisionVsAABB(aabbHull);
                }
                else if (other.TryGetComponent(out OBBCollisionHull2D obbHull))
                {
                    // Try a Circle vs OBB collision
                    isColliding = bb2d.TestCollisionVsOBB(obbHull);
                }
            }
        }

        return isColliding;
    }

    private void CheckCollisions()
    {
        bool isColliding = false;

        foreach (GameObject particle in particleList)
        {
            if (particle.TryGetComponent(out CircleCollisionHull2D circleHull))
                isColliding = CheckCircleCollision(particle);
            else if (particle.TryGetComponent(out AABBCollisionHull2D aabbHull))
                isColliding = CheckBoxCollision(particle);
            else if (particle.TryGetComponent(out OBBCollisionHull2D obbHull))
                isColliding = CheckBoxCollision(particle);

            if (isColliding)
            {
                particle.GetComponent<MeshRenderer>().material = particle.GetComponent<Particle2DComponent>().greenMat;
            }
            else
            {
                particle.GetComponent<MeshRenderer>().material = particle.GetComponent<Particle2DComponent>().redMat;
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

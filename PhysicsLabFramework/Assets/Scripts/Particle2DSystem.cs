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
                p2d.UpdateRotation(dt);
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
            p2d.SetMass(p2d.getStartingMass());
        }
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        UpdateAllParticles(dt);
    }
}

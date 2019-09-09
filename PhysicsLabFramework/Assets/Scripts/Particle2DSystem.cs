using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Particle2DSystem : MonoBehaviour
{
    List<GameObject> particleList;

    public bool shouldOscillate;

    // Trying to mimic ECS-style updating all entities
    private void UpdateAllParticles(float dt)
    {
        foreach (GameObject particle in particleList)
        {
            Particle2DComponent p2d = particle.GetComponent<Particle2DComponent>();

            if (p2d.shouldMove)
                p2d.UpdatePosition(dt);
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

        foreach(GameObject particle in particleList)
        {
            if (shouldOscillate)
                particle.GetComponent<Particle2DComponent>().shouldOscillate = true;
            else
                particle.GetComponent<Particle2DComponent>().shouldOscillate = false;
        }
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        UpdateAllParticles(dt);
    }
}

using UnityEngine;
using MLAgents;
using NS_Physics3D;

public class RollerAgent : Agent
{
    protected Particle3D p3d;
    public float speed;
    public Particle3D target;

    public override void AgentReset()
    {
        // If the agent has fallen, zero its momentum
        if (transform.position.y < 0)
        {
            p3d.angularVelocity = Vector3.zero;
            p3d.velocity = Vector3.zero;
            p3d.position = new Vector3(0f, 0.5f, 0f);
        }

        // Move the cube to a random new position
        target.position = new Vector3(Random.value * 8f - 4f, 0.5f, Random.value * 8f - 4f);
    }

    public override void CollectObservations()
    {
        // Target & Agent positions
        AddVectorObs(target.position);
        AddVectorObs(transform.position);

        // Agent velocity
        AddVectorObs(p3d.velocity.x);
        AddVectorObs(p3d.velocity.z);
    }

    public override void AgentAction(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        p3d.AddForce(controlSignal * speed);

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        // Fell off platform
        if (transform.position.y < 0)
        {
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }

    // Initialize local variables
    private void Awake()
    {
        p3d = GetComponent<Particle3D>();
    }
}

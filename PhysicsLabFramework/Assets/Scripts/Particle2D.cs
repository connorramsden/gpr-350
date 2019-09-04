using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    // Step 1
    public Vector2 position, velocity, acceleration;
    public float rotation, angularVelocity, angularAccel;

    // step 2
    void UpdatePositionExplicitEuler(float dt)
    {
        // formula for explicit euler (1st order integration)
        // x(t+dt) = x(t) + v(t)dt
        // Euler: For the current value, add the derivative
        // F(t+dt) = F(t) + f(t)dt
        //                + (dF/dt) dt
        position += velocity * dt;

        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    void UpdatePositionKinematic(float dt)
    {
        // x(t+dt) = x(t) + v(t)dt + 0.5(a(t)dt^2)
        position += (velocity * dt) + (0.5f * (acceleration * dt) * (dt * dt));

        velocity += acceleration * dt;
    }

    void UpdateRotationEulerExplicit(float dt)
    {
        rotation += angularVelocity * dt;

        angularVelocity += angularAccel * dt;
    }

    void UpdateRotationKinematic(float dt)
    {
        rotation += (angularVelocity * dt) + (0.5f * angularAccel * dt * (dt * dt));

        angularVelocity += angularAccel * dt;
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // step 3
        // choose integrator
       UpdatePositionExplicitEuler(dt);
       UpdatePositionKinematic(dt);
       UpdateRotationEulerExplicit(dt);
        UpdateRotationKinematic(dt);
        // update transform
        transform.position = position;
        transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, rotation));

        // step4
        // test by faking motion along a curve
        acceleration.x = -Mathf.Sin(Time.time);
    }
}

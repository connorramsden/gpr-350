using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    // Bonus
    public enum IntegrationType
    {
        EULER,
        KINEMATIC
    }

    struct Particle
    {
        public Vector2 position, velocity, acceleration;
        public float rotation;
        public float angularVelocity;
        public float angularAccel;
    }

    Particle integratedParticle;

    [Header("Integration Type")]
    public IntegrationType currentIntegration;

    // step 2
    void UpdatePositionExplicitEuler(float dt)
    {
        // formula for explicit euler (1st order integration)
        // x(t+dt) = x(t) + v(t)dt
        // Euler: For the current value, add the derivative
        // F(t+dt) = F(t) + f(t)dt
        //                + (dF/dt) dt
        integratedParticle.position += integratedParticle.velocity * dt;

        // v(t+dt) = v(t) + a(t)dt
        integratedParticle.velocity += integratedParticle.acceleration * dt;
    }

    void UpdatePositionKinematic(float dt)
    {
        // x(t+dt) = x(t) + v(t)dt + 0.5(a(t)dt^2)
        integratedParticle.position += (integratedParticle.velocity * dt) + (0.5f * integratedParticle.acceleration * (dt * dt));

        integratedParticle.velocity += integratedParticle.acceleration * dt;
    }

    void UpdateRotationEulerExplicit(float dt)
    {
        integratedParticle.rotation += integratedParticle.angularVelocity * dt;

        integratedParticle.angularVelocity += integratedParticle.angularAccel * dt;
    }

    void UpdateRotationKinematic(float dt)
    {
        integratedParticle.rotation += (integratedParticle.angularVelocity * dt) + (0.5f * integratedParticle.angularAccel * (dt * dt));

        integratedParticle.angularVelocity += integratedParticle.angularAccel * dt;
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        Debug.Log($"{gameObject.name}'s integration type is {currentIntegration}");

        // step 3
        // choose integrator
        if (currentIntegration.Equals(IntegrationType.EULER))
        {
            UpdatePositionExplicitEuler(dt);
            UpdateRotationEulerExplicit(dt);
        }
        else
        {
            UpdatePositionKinematic(dt);
            UpdateRotationKinematic(dt);
        }

        // update transform
        transform.position = new Vector3(integratedParticle.position.x, transform.position.y);
        // Debug.Log($"{gameObject.name}'s position is {gameObject.transform.position.x}");
        // update rotation
        transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, integratedParticle.rotation));

        // step4
        // test by faking motion along a curve
        integratedParticle.acceleration.x = -Mathf.Sin(Time.time);
    }
}

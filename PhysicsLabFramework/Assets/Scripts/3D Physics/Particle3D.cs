using UnityEngine;

namespace Physics3D
{
    public class Particle3D : MonoBehaviour
    {
        // Position of the 3D Particle
        public Vector3 position
        {
            get; private set;
        }

        public Vector3 velocity
        {
            get; private set;
        }

        public Vector3 acceleration
        {
            get; private set;
        }

        public Vector3 force
        {
            get; private set;
        }

        public Quaternion rotation
        {
            get; private set;
        }

        public Vector3 angularVelocity
        {
            get; private set;
        }

        public Vector3 angularAcceleration
        {
            get; private set;
        }

        public Vector3 torque
        {
            get; private set;
        }

        // Update a particle's position based on Euler Explicit integration
        public void UpdatePositionEulerExplicit(float dt)
        {
            // x(t+dt) = x(t) + v(t)dt
            // Euler:
            // F(t+dt) = F(t) + f(t)dt
            //                + (dF/dt) dt
            position += velocity * dt;

            // v(t+dt) = v(t) + a(t)dt
            velocity += acceleration * dt;
        }

        // Updates a Particle's position based on Kinematic integration
        public void UpdatePositionKinematic(float dt)
        {
        }

        // Update a particle's rotation based on Euler Explicit integration
        public void UpdateRotationEulerExplicit(float dt)
        {
        }

        // Update's a particle's rotation based on Kinematic integration
        public void UpdateRotationKinematic(float dt)
        {
        }

        public void UpdateVelocityEulerExplicit()
        {
        }

        public void UpdateAngularVelocityEulerExplicit()
        {
        }
    }
}

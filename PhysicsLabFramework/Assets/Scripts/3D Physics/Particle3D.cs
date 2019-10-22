using System;
using UnityEngine;

namespace Physics3D
{
    public class Particle3D : MonoBehaviour
    {
        public enum IntegrationType
        {
            INVALID_INTEGRATION = -1,
            EULER_EXPLICIT,
            KINEMATIC
        }

        public IntegrationType integrationType;

        // Lab 06 Step 01
        // Position, Velocity, Acceleration, and Force are 3D Vectors
        public Vector3 position { get; private set; }
        public Vector3 velocity { get; private set; }
        public Vector3 acceleration { get; private set; }
        public Vector3 force { get; private set; }

        // Rotation is a Quaternion (from a single float about the Z-Axis)
        // Implemented as an NQuaternion, my custom Quaternion class
        public NQuaternion rotation { get; private set; }

        // Angular Velocity, Angular Acceleration, and Torque are 3D Vectors (from single floats about the Z-Axis)
        public Vector3 angularVelocity { get; private set; }
        public Vector3 angularAcceleration { get; private set; }
        public Vector3 torque { get; private set; }

        // Mass & Inverse Mass are still floats
        public float mass { get; private set; }
        public float inverseMass { get; private set; }

        // Update a particle's position based on Euler Explicit integration
        private void UpdatePositionEulerExplicit(float dt)
        {
            // x(t+dt) = x(t) + v(t)dt

            // Euler:
            // F(t+dt) = F(t) + f(t)dt
            //                + (dF/dt) dt
            position += velocity * dt;
        }

        // Updates a Particle's position based on Kinematic integration
        private void UpdatePositionKinematic(float dt)
        {
            position += velocity * dt;
        }

        public void UpdatePosition(float dt)
        {
            switch (integrationType)
            {
                case IntegrationType.EULER_EXPLICIT:
                {
                    UpdatePositionEulerExplicit(dt);
                    break;
                }
                case IntegrationType.KINEMATIC:
                {
                    UpdatePositionKinematic(dt);
                    break;
                }
            }

            velocity += acceleration * dt;
        }

        // Update a particle's rotation based on Euler Explicit integration
        private void UpdateRotationEulerExplicit(float dt)
        {
            // q(t+dt) = q(t) + w(t)q(t) * dt/2
            // where w === angularVelocity
            rotation += .5f * rotation * angularVelocity * dt;
        }

        // Updates a particle's rotation based on Kinematic integration
        private void UpdateRotationKinematic(float dt)
        {
            rotation += .5f * rotation * angularAcceleration * dt;
        }

        public void UpdateRotation(float dt)
        {
            switch (integrationType)
            {
                case IntegrationType.EULER_EXPLICIT:
                {
                    UpdateRotationEulerExplicit(dt);
                    break;
                }
                case IntegrationType.KINEMATIC:
                {
                    UpdateRotationKinematic(dt);
                    break;
                }
            }

            // Must normalize rotation in order to stop unwanted scaling
            rotation.Normalize();

            angularVelocity += angularAcceleration * dt;
        }

        public void UpdateVelocityEulerExplicit()
        {
        }

        public void UpdateAngularVelocityEulerExplicit()
        {
        }
    }
}
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
        [Header("Position Integration")] public Vector3 position;
        public Vector3 velocity;
        public Vector3 acceleration;
        public Vector3 force;

        [Header("Rotation Integration")]
        // Rotation is a Quaternion (from a single float about the Z-Axis)
        // Implemented as an NQuaternion, my custom Quaternion class
        public NQuaternion rotation;

        // Angular Velocity, Angular Acceleration, and Torque are 3D Vectors (from single floats about the Z-Axis)
        public Vector3 angularVelocity;

        public Vector3 angularAcceleration;
        // public Vector3 torque;

        [Header("Force Integration")]
        // Mass & Inverse Mass are still floats
        public float mass;
        public float inverseMass;

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
            // x(t+dt) = x(t) + v(t)dt + 1/2(a(t)dt^2)
            position += velocity * dt + .5f * acceleration * (dt * dt);
        }

        private void UpdatePosition(float dt)
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

            // Euler integration for updating velocity
            velocity += acceleration * dt;
        }

        // Update a particle's rotation based on Euler Explicit integration
        private void UpdateRotationEulerExplicit(float dt)
        {
            // q(t+dt) = q(t) + w(t)q(t) * dt/2
            // where w === angularVelocity
            rotation += angularVelocity * rotation * (dt * .5f);
        }

        // Updates a particle's rotation based on Kinematic integration
        private void UpdateRotationKinematic(float dt)
        {
            // Bonus, not yet implemented
            throw new NotImplementedException();
        }

        private void UpdateRotation(float dt)
        {
            switch (integrationType)
            {
                case IntegrationType.EULER_EXPLICIT:
                {
                    UpdateRotationEulerExplicit(dt);
                    break;
                }
                case IntegrationType.KINEMATIC:
                default:
                {
                    UpdateRotationEulerExplicit(dt);
                    break;
                }
            }

            // Must normalize rotation in order to stop unwanted scaling
            rotation.Normalize();
    
            // Euler integration for updating angular velocity
            angularVelocity += angularAcceleration * dt;
        }

        private void Awake()
        {
            // Upon Awake(), set the object's tag to 3D Particle
            if (!gameObject.CompareTag("3D Particle"))
            {
                gameObject.tag = "3D Particle";
            }
        }

        private void FixedUpdate()
        {
            // Acquire fixed DeltaTime
            float dt = Time.fixedDeltaTime;
            
            // Update position & rotation
            UpdatePosition(dt);
            UpdateRotation(dt);

            // Oscillate movement
            acceleration.x = -Mathf.Sin(Time.time);

            // Update position & rotation
            transform.position = position;
            transform.Rotate(rotation.x, rotation.y, rotation.z);
        }
    }
}
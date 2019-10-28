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

        public enum InertiaShape
        {
            INVALID_SHAPE = -1,
            SPHERE_SOLID,
            SPHERE_HOLLOW,
            BOX_SOLID,
            BOX_HOLLOW,
            CUBE_SOLID,
            CUBE_HOLLOW,
            CYLINDER_SOLID,
            CONE_SOLID
        }

        [Header("Integration Selection")]
        public IntegrationType integrationType;
        public InertiaShape inertiaShape;

        public bool shouldOscillate;

        [Header("Position Integration")]
        // Lab 06 Step 01
        // Position, Velocity, Acceleration, and Force are 3D Vectors
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 acceleration;
        public Vector3 force;

        // Lab 07 Step 01
        // Updates every frame by converting rotation & position into homogenous matrices
        public Matrix4x4 worldTransformMatrix;
        public Matrix4x4 worldTransformInverse;

        [Header("Rotation Integration")]
        // Rotation is a Quaternion (from a single float about the Z-Axis)
        // Implemented as an NQuaternion, my custom Quaternion class
        public NQuaternion rotation;
        // Angular Velocity, Angular Acceleration, and Torque are 3D Vectors (from single floats about the Z-Axis)
        public Vector3 angularVelocity;
        // Converted from torque using Newton-2 for rotation
        public Vector3 angularAcceleration;
        // Applied in world space using cross-product
        public Vector3 torque;

        // Lab 07 Step 01
        [Header("Force Integration")]
        // 3D Vectors; world center is updated every frame by transformation
        public Vector3 localMass;
        public Vector3 worldMass;
        // 3D Matrices; world is updated every frame by performing change of basis
        public Matrix4x4 localInertia;
        public Matrix4x4 worldInertia;

        // Updates a particle's position based on Euler Explicit integration
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

        // Updates a particle's position based on selected integration method
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

        // Updates a particle's rotation based on Euler Explicit integration
        private void UpdateRotationEulerExplicit(float dt)
        {
            // q(t+dt) = q(t) + w(t)q(t) * dt/2
            // where w === angularVelocity
            NQuaternion goFast = (angularVelocity * rotation);
            rotation += goFast * (dt * .5f);
        }

        // Updates a particle's rotation based on Kinematic integration
        private void UpdateRotationKinematic(float dt)
        {
            // Bonus, not yet implemented
            throw new NotImplementedException();
        }

        // Updates a particle's rotation based on selected integration method
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

        // Initialize local variables here
        private void Awake()
        {
            // Upon Awake(), set the object's tag to 3D Particle
            if (!gameObject.CompareTag("3D Particle"))
            {
                gameObject.tag = "3D Particle";
            }

            shouldOscillate = true;
        }

        // Update in fixed-step time
        private void FixedUpdate()
        {
            // Acquire fixed DeltaTime
            float dt = Time.fixedDeltaTime;

            // Update position & rotation
            UpdatePosition(dt);
            UpdateRotation(dt);

            // Oscillate movement
            if (shouldOscillate)
                acceleration.x = -Mathf.Sin(Time.time);

            // Update position & rotation
            transform.position = position;
            transform.rotation = rotation.ToQuaternion();
        }
    }
}

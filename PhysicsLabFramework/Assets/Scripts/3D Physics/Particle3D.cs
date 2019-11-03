using System;
using UnityEngine;
using static Physics3D.AngularMath;

namespace Physics3D
{
    public class Particle3D : MonoBehaviour
    {
        // Lab 06 Step 02
        public enum IntegrationType
        {
            INVALID_INTEGRATION = -1,
            EULER_EXPLICIT,
            KINEMATIC
        }

        // Lab 07 Step 01
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

        // Lab 06 Step 01
        [Header("Position Integration")]
        // Position, Velocity, Acceleration, and Force are 3D Vectors
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 acceleration;
        // public Vector3 force;

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
        [Header("Dynamics Integration")]
        // 3D Vectors; world center is updated every frame by transformation
        public Vector3 localMassCenter;
        public Vector3 worldMassCenter;
        // 3D Matrices; world is updated every frame by performing change of basis
        public Matrix4x4 localInertia;
        public Matrix4x4 worldInertia;
        // Updates every frame by converting rotation & position into homogeneous matrices
        public Matrix4x4 worldTransformMatrix;
        public Matrix4x4 worldTransformInverse;
        
        [Header("Inertia Tensor Integration")]
        // Float for radius
        public float radius;
        public float mass;
        public float width, height, depth;

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
            NQuaternion rotTimesVel = (angularVelocity * rotation);
            rotation += rotTimesVel * (dt * .5f);
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
            // Set the object's tag to 3D Particle
            if (!gameObject.CompareTag("3D Particle"))
            {
                gameObject.tag = "3D Particle";
            }
            
            // Set the particle's Inertia tensor based on its shape
            SetInertia(ref localInertia, this);
        }

        // Update in fixed-step time
        private void FixedUpdate()
        {
            // Acquire fixed DeltaTime
            float dt = Time.fixedDeltaTime;

            // Update position & rotation
            UpdatePosition(dt);
            UpdateRotation(dt);
            
            // Calculate the world transform matrix based on rotation & position
            CalculateTransformMatrix(ref worldTransformMatrix, rotation, position);

            // Update position & rotation
            transform.position = position;
            transform.rotation = rotation.ToQuaternion();
        }
    }
}

using System;
using UnityEngine;

namespace NS_Physics3D
{
    public static class AngularMath
    {
        // -------------------------- 3x3 Matrix Math -------------------------- //
        
        // Transforms a Vector3 by the passed Matrix3x3 | Millington 2nd Ed. pg. 176-177
        public static Vector3 MulVec3x3Mat(Vector3 vec, Matrix4x4 mat)
        {
            return new Vector3
            {
                x = vec.x * mat[0, 0] + vec.y * mat[0, 1] + vec.z * mat[0, 2],
                y = vec.x * mat[1, 0] + vec.y * mat[1, 1] + vec.z * mat[1, 2],
                z = vec.x * mat[2, 0] + vec.y * mat[2, 1] + vec.z * mat[2, 2]
            };
        }
        
        // Inverts the passed 3x3 Matrix | Millington 2nd Ed. pg. 186
        public static void Invert3x3Matrix(Matrix4x4 orig, ref Matrix4x4 inverse)
        {
            float t1 = orig[0, 0] * orig[1, 1];
            float t2 = orig[0, 0] * orig[1, 2];
            float t3 = orig[0, 1] * orig[1, 0];
            float t4 = orig[0, 2] * orig[1, 0];
            float t5 = orig[0, 1] * orig[2, 0];
            float t6 = orig[0, 2] * orig[2, 0];

            float det = t1 * orig[2, 2] - t2 * orig[2, 1] - t3 * orig[2, 2] + t4 * orig[2, 1] + t5 * orig[1, 2] -
                        t6 * orig[1, 1];

            if (det == 0.0f)
                return;

            float invd = 1.0f / det;

            inverse[0, 0] = (orig[1, 1] * orig[2, 2] - orig[1, 2] * orig[2, 1]) * invd;
            inverse[0, 1] = -(orig[0, 1] * orig[2, 2] - orig[0, 2] * orig[2, 1]) * invd;
            inverse[0, 2] = (orig[0, 1] * orig[1, 2] - orig[0, 2] * orig[1, 1]) * invd;

            inverse[1, 0] = -(orig[1, 0] * orig[2, 2] - orig[1, 2] * orig[2, 0]) * invd;
            inverse[1, 1] = (orig[0, 0] * orig[2, 2] - t6) * invd;
            inverse[1, 2] = -(t2 - t4) * invd;

            inverse[2, 0] = (orig[1, 0] * orig[2, 1] - orig[1, 1] * orig[2, 0]) * invd;
            inverse[2, 1] = -(orig[0, 0] * orig[2, 1] - t5) * invd;
            inverse[2, 1] = (t1 - t3) * invd;
        }

        // Set the inertia tensor of a Particle based on its shape | Dan Buckstein Angular Dynamics Slides
        public static void SetInertia(out Matrix4x4 inertiaTensor, Particle3D p3d)
        {
            // Retrieve the particle's inertia shape
            Particle3D.InertiaShape iShape = p3d.inertiaShape;

            // Zero out inertia tensor
            inertiaTensor = Matrix4x4.zero;

            // Retrieve particle shape variables
            float radius = p3d.radius;
            float mass = p3d.mass;
            float width = p3d.width;
            float height = p3d.height;
            float depth = p3d.depth;

            switch (iShape)
            {
                case Particle3D.InertiaShape.SPHERE_SOLID:
                {
                    // Solid Sphere of radius r & mass m
                    // I = 2/5 * m * r^2
                    float tensor = 0.4f * mass * radius * radius;
                    inertiaTensor.m00 = inertiaTensor.m11 = inertiaTensor.m22 = tensor;
                    break;
                }
                case Particle3D.InertiaShape.SPHERE_HOLLOW:
                {
                    // Hollow Sphere of radius r & mass m
                    // I = 2/3 * m * r^2
                    // Some loss of precision, but better than fraction
                    float tensor = 0.667f * mass * radius * radius;
                    inertiaTensor.m00 = inertiaTensor.m11 = inertiaTensor.m22 = tensor;
                    break;
                }
                case Particle3D.InertiaShape.BOX_SOLID:
                {
                    // Solid box of width w, height h, depth d , and mass m
                    // Iw = 1/12 * m * (h^2 * d^2)
                    float inertiaWidth = 0.833f * mass * (height * height * depth * depth);
                    // Ih = 1/12 * m * (d^2 * w^2)
                    float inertiaHeight = 0.833f * mass * (depth * depth * width * width);
                    // Id = 1/12 * m * (w^2 * h^2)
                    float inertiaDepth = 0.833f * mass * (width * width * height * height);

                    inertiaTensor.m00 = inertiaWidth;
                    inertiaTensor.m11 = inertiaHeight;
                    inertiaTensor.m22 = inertiaDepth;
                    break;
                }
                case Particle3D.InertiaShape.BOX_HOLLOW:
                {
                    // Hollow box (inferred) of width w, height h, depth d , and mass m
                    // Iw = 5/3 * m * (h^2 * d^2)
                    float inertiaWidth = 1.667f * mass * (height * height * depth * depth);
                    // Ih = 5/3 * m * (d^2 * w^2)
                    float inertiaHeight = 1.667f * mass * (depth * depth * width * width);
                    // Id = 5/3 * m * (w^2 * h^2)
                    float inertiaDepth = 1.667f * mass * (width * width * height * height);

                    inertiaTensor.m00 = inertiaWidth;
                    inertiaTensor.m11 = inertiaHeight;
                    inertiaTensor.m22 = inertiaDepth;
                    break;
                }
                case Particle3D.InertiaShape.CYLINDER_SOLID:
                {
                    // Solid cylinder of radius r, height h, and mass m
                    // I = 1/12 * m * (3 * r^2 + h^2)
                    // Im = 1/12 * m * r^2
                    float tensor = 0.833f * mass * (3f * radius * radius + height * height);
                    float tensorMod = 0.833f * mass * radius * radius;

                    inertiaTensor.m00 = inertiaTensor.m11 = tensor;
                    inertiaTensor.m22 = tensorMod;
                    break;
                }
                case Particle3D.InertiaShape.CONE_SOLID:
                {
                    // Solid cone of radius r, height h, and mass m about apex
                    // I = 3/5 * m * h^2 + 3/20 * m * r^2
                    // Im = 3/10 * m * r^2
                    float tensor = 0.6f * mass * height * height + 0.15f * mass * radius * radius;
                    float tensorMod = 0.3f * mass * radius * radius;
                    inertiaTensor.m00 = inertiaTensor.m11 = tensor;
                    inertiaTensor.m22 = tensorMod;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(p3d), iShape, null);
            }

            // Set the particle's inverse inertia tensor
            Invert3x3Matrix(inertiaTensor, ref p3d.inertiaInverse);
        }

        // -------------------------- 4x4 Matrix Math -------------------------- //

        // Transform a Vector3 by the passed Matrix4x4 | Millington 2nd Ed. pg. 180
        public static Vector3 MulVec4x4Mat(Vector3 vec, Matrix4x4 mat)
        {
            return new Vector3
            {
                x = vec.x * mat[0] + vec.y * mat[1] + vec.z * mat[2] + mat[3],
                y = vec.x * mat[4] + vec.y * mat[5] + vec.z * mat[6] + mat[7],
                z = vec.x * mat[8] + vec.y * mat[9] + vec.z * mat[10] + mat[11]
            };
        }
        
        // Returns the determinant of the passed 4x4 matrix | Millington 2nd Ed. pg. 188
        public static float GetDeterminant(Matrix4x4 mat)
        {
            return mat[8] * mat[5] * mat[2] +
                   mat[4] * mat[9] * mat[2] +
                   mat[8] * mat[1] * mat[6] -
                   mat[0] * mat[9] * mat[6] -
                   mat[4] * mat[1] * mat[10] +
                   mat[0] * mat[5] * mat[10];
        }

        // Inverts the passed 4x4 matrix | Millington 2nd. Ed. pg. 188-189
        public static void Invert4x4Matrix(Matrix4x4 orig, out Matrix4x4 inverse)
        {
            inverse = Matrix4x4.zero;

            float det = GetDeterminant(orig);

            if (det == 0.0f)
                return;

            det = 1.0f / det;

            inverse[0] = (-orig[9] * orig[6] + orig[5] * orig[10]) * det;
            inverse[4] = (orig[8] * orig[6] - orig[4] * orig[10]) * det;
            inverse[8] = (-orig[8] * orig[5] + orig[4] * orig[9] * orig[15]) * det;

            inverse[1] = (orig[9] * orig[2] - orig[1] * orig[10]) * det;
            inverse[5] = (-orig[8] * orig[2] + orig[0] * orig[10]) * det;
            inverse[9] = (orig[8] * orig[1] - orig[0] * orig[9] * orig[15]) * det;

            inverse[2] = (-orig[5] * orig[2] + orig[1] * orig[6] * orig[15]) * det;
            inverse[6] = (orig[4] * orig[2] - orig[0] * orig[6] * orig[15]) * det;
            inverse[10] = (-orig[4] * orig[1] + orig[0] * orig[5] * orig[15]) * det;

            inverse[3] = (orig[9] * orig[6] * orig[3]
                          - orig[5] * orig[10] * orig[3]
                          - orig[9] * orig[2] * orig[7]
                          + orig[1] * orig[10] * orig[7]
                          + orig[5] * orig[2] * orig[11]
                          - orig[1] * orig[6] * orig[11]) * det;

            inverse[7] = (-orig[8] * orig[6] * orig[3]
                          + orig[4] * orig[10] * orig[3]
                          + orig[8] * orig[2] * orig[7]
                          - orig[0] * orig[10] * orig[7]
                          - orig[4] * orig[2] * orig[11]
                          + orig[0] * orig[6] * orig[11]) * det;

            inverse[11] = (orig[8] * orig[5] * orig[3]
                           - orig[4] * orig[9] * orig[3]
                           - orig[8] * orig[1] * orig[7]
                           + orig[0] * orig[9] * orig[7]
                           + orig[4] * orig[1] * orig[11]
                           - orig[0] * orig[5] * orig[11]) * det;
        }

        // Converts Quaternion rotation & Vector3 position into a homogeneous matrix | Millington 2nd End. pg. 192-193
        public static void CalculateTransformMatrix(ref Matrix4x4 worldMatrix, NQuaternion quat, Vector3 vec)
        {
            // i,j,k,r = x,y,z,w
            worldMatrix[0] = 1f - (2f * quat.y * quat.y + 2f * quat.z * quat.z);
            worldMatrix[1] = 2f * quat.x * quat.y + 2f * quat.z * quat.w;
            worldMatrix[2] = 2f * quat.x * quat.z - 2f * quat.y * quat.w;
            worldMatrix[3] = vec.x;

            worldMatrix[4] = 2f * quat.x * quat.y - 2f * quat.z * quat.w;
            worldMatrix[5] = 1f - (2f * quat.x * quat.x + 2f * quat.z * quat.z);
            worldMatrix[6] = 2f * quat.y * quat.z + 2f * quat.x * quat.w;
            worldMatrix[7] = vec.y;

            worldMatrix[8] = 2f * quat.x * quat.z + 2f * quat.y * quat.w;
            worldMatrix[9] = 2f * quat.y * quat.z - 2f * quat.x * quat.w;
            worldMatrix[10] = 1f - (2f * quat.x * quat.x - 2f * quat.y * quat.y);
            worldMatrix[11] = vec.z;
        }

        // Bring the passed local Vector3 into world-space | Millington 2nd Ed. pg. 193
        public static Vector3 LocalToWorld(Vector3 local, Matrix4x4 transform)
        {
            return MulVec4x4Mat(local, transform);
        }

        // Bring the passed world Vector3 into local-space | Millington 2nd Ed. pg. 193
        public static Vector3 WorldToLocal(Vector3 world, Matrix4x4 transform)
        {
            Invert4x4Matrix(transform, out Matrix4x4 inverse);
            return MulVec4x4Mat(world, inverse);
        }
        
        // Transforms a Vector3 by the inverse of the passed Matrix 4x4 | Millington 2nd Ed. pg. 194
        public static Vector3 TransformVectorInverse(Vector3 vec, Matrix4x4 mat)
        {
            Vector3 outVec = vec;

            outVec.x -= mat[3];
            outVec.y -= mat[7];
            outVec.z -= mat[11];

            outVec.x = vec.x * mat[0] + vec.y * mat[4] + vec.z * mat[8];
            outVec.y = vec.x * mat[1] + vec.y * mat[5] + vec.z * mat[9];
            outVec.z = vec.x * mat[2] + vec.y * mat[6] + vec.z * mat[10];

            return outVec;
        }

        // Transform a directional Vector3 by the passed Matrix4x4 | Millington 2nd Ed. pg. 196
        public static Vector3 TransformVectorDirection(Vector3 vec, Matrix4x4 mat)
        {
            return new Vector3
            {
                x = vec.x * mat[0] + vec.y * mat[1] + vec.z * mat[2],
                y = vec.x * mat[4] + vec.y * mat[5] + vec.z * mat[6],
                z = vec.x * mat[8] + vec.y * mat[9] + vec.z * mat[10]
            };
        }

        // Transform a directional Vector3 by the inverse of the passed Matrix4x4. | Millington 2nd Ed. pg. 196
        public static Vector3 TransformVectorDirectionInverse(Vector3 vec, Matrix4x4 mat)
        {
            return new Vector3
            {
                x = vec.x * mat[0] + vec.y * mat[4] + vec.z * mat[8],
                y = vec.x * mat[1] + vec.y * mat[5] + vec.z * mat[9],
                z = vec.x * mat[2] + vec.y * mat[6] + vec.z * mat[10]
            };
        }

        // -------------------------- Dynamics Integration -------------------------- //
        
        // Calculate torque based on inertia tensor
        public static void CalculateTorque(Particle3D p3d, ref Vector3 torque)
        {
            // Torque = moment arm (vector) * Force
            Vector3 momentArm = p3d.pointOfAppliedForce - p3d.worldMassCenter;

            torque += Vector3.Cross(p3d.appliedForce, momentArm);
        }

        // Update angular acceleration based on torque
        public static void UpdateAngularAcceleration(Particle3D p3d, ref Vector3 torque, out Vector3 angularAccel)
        {
            // Step 01: Convert torque into local space
            Vector3 localSpaceTorque = WorldToLocal(torque, p3d.worldTransformMatrix);
            // Step 02: Cross-Product inverse inertia tensor and previously calculated local-space torque
            
            // Step 03: Bring newly calculated torque into world space
            angularAccel = LocalToWorld(localSpaceTorque, p3d.worldTransformMatrix);

            // Step 04: Kill torque, as it is re-calculated every frame
            torque = Vector3.zero;
        }
    }
}

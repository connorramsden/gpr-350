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

        // Transposes the passed 3x3 Rotation Matrix | Dan Buckstein Math Slide 28
        public static void TransposeRotation(Matrix4x4 orig, out Matrix4x4 transpose)
        {
            transpose = orig.transpose;
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
                    float tensor = (2f / 5f) * mass * radius * radius;
                    inertiaTensor.m00 = inertiaTensor.m11 = inertiaTensor.m22 = tensor;
                    break;
                }
                case Particle3D.InertiaShape.SPHERE_HOLLOW:
                {
                    // Hollow Sphere of radius r & mass m
                    // I = 2/3 * m * r^2
                    // Some loss of precision, but better than fraction
                    float tensor = (2f / 3f) * mass * radius * radius;
                    inertiaTensor.m00 = inertiaTensor.m11 = inertiaTensor.m22 = tensor;
                    break;
                }
                case Particle3D.InertiaShape.BOX_SOLID:
                {
                    // Solid box of width w, height h, depth d , and mass m
                    // Iw = 1/12 * m * (h^2 * d^2)
                    float inertiaWidth = (1f / 12f) * mass * (height * height * depth * depth);
                    // Ih = 1/12 * m * (d^2 * w^2)
                    float inertiaHeight = (1f / 12f) * mass * (depth * depth * width * width);
                    // Id = 1/12 * m * (w^2 * h^2)
                    float inertiaDepth = (1f / 12f) * mass * (width * width * height * height);

                    inertiaTensor.m00 = inertiaWidth;
                    inertiaTensor.m11 = inertiaHeight;
                    inertiaTensor.m22 = inertiaDepth;
                    break;
                }
                case Particle3D.InertiaShape.BOX_HOLLOW:
                {
                    // Hollow box (inferred) of width w, height h, depth d , and mass m
                    // Iw = 5/3 * m * (h^2 * d^2)
                    float inertiaWidth = (5f / 3f) * mass * (height * height * depth * depth);
                    // Ih = 5/3 * m * (d^2 * w^2)
                    float inertiaHeight = (5f / 3f) * mass * (depth * depth * width * width);
                    // Id = 5/3 * m * (w^2 * h^2)
                    float inertiaDepth = (5f / 3f) * mass * (width * width * height * height);

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
                    float tensor = (1f / 12f) * mass * (3f * radius * radius + height * height);
                    float tensorMod = (1f / 12f) * mass * radius * radius;

                    inertiaTensor.m00 = inertiaTensor.m11 = tensor;
                    inertiaTensor.m22 = tensorMod;
                    break;
                }
                case Particle3D.InertiaShape.CONE_SOLID:
                {
                    // Solid cone of radius r, height h, and mass m about apex
                    // I = 3/5 * m * h^2 + 3/20 * m * r^2
                    // Im = 3/10 * m * r^2
                    float tensor = (3f / 5f) * mass * height * height + (3f / 20f) * mass * radius * radius;
                    float tensorMod = (3f / 10f) * mass * radius * radius;
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
                x = vec.x * mat[0, 0] + vec.y * mat[0, 1] + vec.z * mat[0, 2] + mat[0, 3],
                y = vec.x * mat[1, 0] + vec.y * mat[1, 1] + vec.z * mat[1, 2] + mat[1, 3],
                z = vec.x * mat[2, 0] + vec.y * mat[2, 1] + vec.z * mat[2, 2] + mat[2, 3]
            };
        }

        // Returns the determinant of the passed 4x4 matrix | Millington 2nd Ed. pg. 188
        public static float GetDeterminant(Matrix4x4 mat)
        {
            return mat[2, 0] * mat[1, 1] * mat[0, 2] +
                   mat[1, 0] * mat[2, 1] * mat[0, 2] +
                   mat[2, 0] * mat[0, 1] * mat[1, 2] -
                   mat[0, 0] * mat[2, 1] * mat[1, 2] -
                   mat[1, 0] * mat[0, 1] * mat[2, 2] +
                   mat[0, 0] * mat[1, 1] * mat[2, 2];
        }

        // Inverts the passed 4x4 matrix | Millington 2nd. Ed. pg. 188-189
        public static void Invert4x4Matrix(Matrix4x4 orig, out Matrix4x4 inverse)
        {
            inverse = Matrix4x4.zero;
            // TODO: Optimize matrix inversion, see Buckstein math slides
        }

        // Converts Quaternion rotation & Vector3 position into a homogeneous matrix | Millington 2nd End. pg. 192-193
        public static void CalculateTransformMatrix(ref Matrix4x4 worldMatrix, NQuaternion quat, Vector3 vec)
        {
            // i,j,k,r = x,y,z,w
            worldMatrix[0, 0] = 1f - (2f * quat.y * quat.y + 2f * quat.z * quat.z);
            worldMatrix[0, 1] = 2f * quat.x * quat.y + 2f * quat.z * quat.w;
            worldMatrix[0, 2] = 2f * quat.x * quat.z - 2f * quat.y * quat.w;
            worldMatrix[0, 3] = vec.x;

            worldMatrix[1, 0] = 2f * quat.x * quat.y - 2f * quat.z * quat.w;
            worldMatrix[1, 1] = 1f - (2f * quat.x * quat.x + 2f * quat.z * quat.z);
            worldMatrix[1, 2] = 2f * quat.y * quat.z + 2f * quat.x * quat.w;
            worldMatrix[1, 3] = vec.y;

            worldMatrix[2, 0] = 2f * quat.x * quat.z + 2f * quat.y * quat.w;
            worldMatrix[2, 1] = 2f * quat.y * quat.z - 2f * quat.x * quat.w;
            worldMatrix[2, 2] = 1f - (2f * quat.x * quat.x - 2f * quat.y * quat.y);
            worldMatrix[2, 3] = vec.z;
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

            outVec.x -= mat[0, 3];
            outVec.y -= mat[1, 3];
            outVec.z -= mat[2, 3];

            outVec.x = vec.x * mat[0, 0] + vec.y * mat[1, 0] + vec.z * mat[2, 0];
            outVec.y = vec.x * mat[0, 1] + vec.y * mat[1, 1] + vec.z * mat[2, 1];
            outVec.z = vec.x * mat[0, 2] + vec.y * mat[1, 2] + vec.z * mat[2, 2];

            return outVec;
        }

        // Transform a directional Vector3 by the passed Matrix4x4 | Millington 2nd Ed. pg. 196
        public static Vector3 TransformVectorDirection(Vector3 vec, Matrix4x4 mat)
        {
            return new Vector3
            {
                x = vec.x * mat[0, 0] + vec.y * mat[0, 1] + vec.z * mat[0, 2],
                y = vec.x * mat[1, 0] + vec.y * mat[1, 1] + vec.z * mat[1, 2],
                z = vec.x * mat[2, 0] + vec.y * mat[2, 1] + vec.z * mat[2, 2]
            };
        }

        // Transform a directional Vector3 by the inverse of the passed Matrix4x4. | Millington 2nd Ed. pg. 196
        public static Vector3 TransformVectorDirectionInverse(Vector3 vec, Matrix4x4 mat)
        {
            return new Vector3
            {
                x = vec.x * mat[0, 0] + vec.y * mat[1, 0] + vec.z * mat[2, 0],
                y = vec.x * mat[0, 1] + vec.y * mat[1, 1] + vec.z * mat[2, 1],
                z = vec.x * mat[0, 2] + vec.y * mat[1, 2] + vec.z * mat[2, 2]
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
            Vector3 worldSpaceTorque = MulVec3x3Mat(localSpaceTorque, p3d.inertiaInverse);
            // Step 03: Bring newly calculated torque into world space
            angularAccel = LocalToWorld(worldSpaceTorque, p3d.worldTransformMatrix);

            // Step 04: Kill torque, as it is re-calculated every frame
            torque = Vector3.zero;
        }
    }
}
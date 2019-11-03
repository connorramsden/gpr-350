using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physics3D
{
    public static class AngularMath
    {
        // Converts Quaternion rotation & Vector3 position into a homogeneous matrix
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

        // Inverts the homogeneous world transformation matrix
        public static void ConvertWorldToInverse(ref Particle3D p3d)
        {
        }

        // Set the inertia tensor of a Particle based on its shape
        public static void SetInertia(ref Matrix4x4 inertiaTensor, Particle3D p3d)
        {
            Particle3D.InertiaShape iShape = p3d.inertiaShape;
            // Allows for single retrieval & reuse
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
                case Particle3D.InertiaShape.CUBE_SOLID:
                {
                    // Solid cube of mass m and sides s
                    // I = 1/6 * m * s^2
                    break;
                }
                case Particle3D.InertiaShape.CUBE_HOLLOW:
                {
                    // Hollow cube of mass m and sides s
                    // I = 1/6 * mass * sides^2
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
                    throw new ArgumentOutOfRangeException(nameof(iShape), iShape, null);
            }
        }
        
        
    }
}
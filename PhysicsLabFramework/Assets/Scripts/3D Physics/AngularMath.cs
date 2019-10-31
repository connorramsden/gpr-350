using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physics3D
{
    public static class AngularMath
    {
        // Converts Quaternion rotation & Vector3 position into a homogenous matrix
        public static void CalculateTransformMatrix(ref Matrix4x4 worldMatrix, NQuaternion quat, Vector3 vec)
        {
            // i,j,k,r = x,y,z,w
            Matrix4x4 rotMatrix = quat.ToRotationMatrix();
        }

        // Inverts the homogenous world transformation matrix
        public static void ConvertWorldToInverse(ref Particle3D p3d)
        {
        }

        // Set the inertia of a Particle based on its shape
        public static void SetInertia(Particle3D.InertiaShape inertiaShape, ref Matrix4x4 inertia)
        {
            switch (inertiaShape)
            {
                case Particle3D.InertiaShape.SPHERE_SOLID:
                    break;
                case Particle3D.InertiaShape.SPHERE_HOLLOW:
                    break;
                case Particle3D.InertiaShape.BOX_SOLID:
                    break;
                case Particle3D.InertiaShape.BOX_HOLLOW:
                    break;
                case Particle3D.InertiaShape.CUBE_SOLID:
                    break;
                case Particle3D.InertiaShape.CUBE_HOLLOW:
                    break;
                case Particle3D.InertiaShape.CYLINDER_SOLID:
                    break;
                case Particle3D.InertiaShape.CONE_SOLID:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inertiaShape), inertiaShape, null);
            }
        }
    }
}

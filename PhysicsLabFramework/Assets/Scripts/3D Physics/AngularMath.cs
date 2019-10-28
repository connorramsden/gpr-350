using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physics3D
{
    public static class AngularMath
    {
        // Converts Quaternion rotation & Vector3 position into a homogenous matrix
        public static void ConvertLocalToWorld(ref NQuaternion quat, ref Vector3 vec)
        {
        }

        // Inverts the homogenous world transformation matrix
        public static void ConvertWorldToInverse(ref Particle3D p3d)
        {
        }

        public static void SetInertia(Particle3D.InertiaShape inertiaShape, ref Matrix4x4 inertia)
        {

        }
    }
}

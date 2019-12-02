using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NS_Collision_3D
{
    public class CH3D : MonoBehaviour
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct BaseHull
        {
            public IntPtr hullType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SphereHull
        {
            public float radius;
            public Vector3 center;
        }
    }
}
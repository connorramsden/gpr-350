using System;
using UnityEngine;

namespace NS_Physics3D
{
    [Serializable]
    public struct SVector3 : IEquatable<SVector3>
    {
        public float x, y, z;

        public SVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }

        public static implicit operator Vector3(SVector3 r)
        {
            return new Vector3(r.x, r.y, r.z);
        }

        public static implicit operator SVector3(Vector3 r)
        {
            return new SVector3(r.x, r.y, r.z);
        }

        public override string ToString()
        {
            return $"[{x}, {y}, {z}]";
        }

        public bool Equals(SVector3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }
    }
}
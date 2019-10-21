using System;
using System.ComponentModel;
using UnityEngine;

// Referenced the open-source game engine Acid for my Quaternion implementation:
// https://github.com/EQMG/Acid
// As well as some Unity documentation about their Quaternion implementation

namespace Physics3D
{
    // Made my own Quaternion class because it was easier than using Unity's
    public class NQuaternion
    {
        // Basic identity Quaternion
        private static readonly NQuaternion identityNQuaternion = new NQuaternion(0.0f, 0.0f, 0.0f, 1f);

        // Do not modify these values directly unless you're Dan Buckstein
        public float x;
        public float y;
        public float z;
        public float w;

        public NQuaternion()
        {
        }

        // Quaternion Specialized Constructor
        public NQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        // Returns the normalized version of NQuaternion this is called on
        public NQuaternion normalized => Normalize(this);

        // Adds this NQuaternion with the passed NQuaternion
        public NQuaternion Add(NQuaternion other)
        {
            x += other.x;
            y += other.y;
            z += other.z;
            w += other.w;

            return this;
        }

        // Subtract the passed NQuaternion from this NQuaternion
        public NQuaternion Subtract(NQuaternion other)
        {
            x -= other.x;
            y -= other.y;
            z -= other.z;
            w -= other.w;
            return this;
        }

        // Multiply this NQuaternion by the passed NQuaternion
        public NQuaternion Multiply(NQuaternion other)
        {
            x *= other.w + this.w * other.x + this.y * other.z - this.z * other.y;
            y *= other.w + this.w * other.y + this.z * other.x - this.x * other.z;
            z *= other.w + this.w * other.z + this.x * other.y - this.y * other.x;
            w *= other.w - this.x * other.x - this.y * other.y - this.z * other.z;

            return this;
        }
        
        // Multiply this NQuaternion with the passed Vector3
        public Vector3 Mutliply(Vector3 other)
        {
            Vector3 ret = new Vector3(x, y, z);
            Vector3 crossOne = Vector3.Cross(ret, other);
            Vector3 crossTwo = Vector3.Cross(ret, crossOne);

            return other + 2.0f * (crossOne * w + crossTwo);
        }

        // Returns the Dot-Product of two NQuaternions
        public static float Dot(NQuaternion lhs, NQuaternion rhs)
        {
            return lhs.w * rhs.w + lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        // Smoothly linearly interpolates between the two passed NQuaternions by the passed float
        public static NQuaternion Slerp(NQuaternion lhs, NQuaternion rhs, float progression)
        {
            throw new NotImplementedException();
        }

        // Scales the passed NQuaternion by the passed scalar value
        public static NQuaternion Scale(NQuaternion quat, float scalar)
        {
            NQuaternion ret = new NQuaternion()
            {
                x = quat.x * scalar,
                y = quat.y * scalar,
                z = quat.z * scalar,
                w = quat.w * scalar
            };

            return ret;
        }

        // Returns the squared length of the passed NQuaternion
        public static float GetLengthSquared(NQuaternion quat)
        {
            return quat.x * quat.x + quat.y * quat.y + quat.z * quat.z + quat.w * quat.w;
        }

        // Returns the length of the passed NQuaternion
        public static float GetLength(NQuaternion quat)
        {
            return Mathf.Sqrt(GetLengthSquared(quat));
        }

        // Returns the normalized version of the passed NQuaternion (length of 1)
        public static NQuaternion Normalize(NQuaternion quat)
        {
            var length = GetLength(quat);

            NQuaternion ret = new NQuaternion()
            {
                x = quat.x / length,
                y = quat.y / length,
                z = quat.z / length,
                w = quat.w / length
            };

            return ret;
        }

        // Converts passed NQuaternion to its Euler angle values
        public static Vector3 ToEuler(NQuaternion quat)
        {
            Vector3 result = new Vector3()
            {
                x = Mathf.Atan2(2.0f * (quat.x * quat.w - quat.y * quat.z),
                    1.0f - 2.0f * (quat.x * quat.x + quat.y * quat.y)),
                y = Mathf.Asin(2.0f * (quat.x * quat.z + quat.y * quat.w)),
                z = Mathf.Atan2(2.0f * (quat.z * quat.w - quat.x * quat.y),
                    1.0f - 2.0f * (quat.y * quat.y + quat.z * quat.z))
            };

            return result;
        }

        // Adds two passed NQuaternions together
        public static NQuaternion operator +(NQuaternion lhs, NQuaternion rhs)
        {
            return lhs.Add(rhs);
        }

        // Negates the passed NQuaternion
        public static NQuaternion operator-(NQuaternion quat)
        {
            return new NQuaternion()
            {
                x = -quat.x,
                y = -quat.y,
                z = -quat.z,
                w = -quat.w
            };
        }
        
        // Subtracts right-hand NQuaternion from left-hand NQuaternion
        public static NQuaternion operator -(NQuaternion lhs, NQuaternion rhs)
        {
            return lhs.Subtract(rhs);
        }

        // Multiplies two passed NQuaternions together
        public static NQuaternion operator *(NQuaternion lhs, NQuaternion rhs)
        {
            return lhs.Multiply(rhs);
        }

        // Multiplies passed NQuaternion by passed Vector3
        public static Vector3 operator *(Vector3 lhs, NQuaternion rhs)
        {
            return rhs.Mutliply(lhs);
        }

        // Divides left-hand NQuaternion by right-hand NQuaternion
        public static NQuaternion operator /(NQuaternion lhs, NQuaternion rhs)
        {
            NQuaternion ret = new NQuaternion()
            {
                x = lhs.x / rhs.x, y = lhs.y / rhs.y, z = lhs.z / rhs.z, w = lhs.w / rhs.w
            };

            return ret;
        }

        // This is how Unity's Quaternion== works behind the scenes
        public static bool operator ==(NQuaternion lhs, NQuaternion rhs)
        {
            // Only check if both NQuaternions are valid
            if (lhs != null && rhs != null)
                return Dot(lhs, rhs) > 0.999998986721039;

            // Otherwise, throw a NullReferenceException
            throw new NullReferenceException();
        }

        // Returns false if the == operation is true
        public static bool operator !=(NQuaternion lhs, NQuaternion rhs)
        {
            return !(lhs == rhs);
        }
    }
}

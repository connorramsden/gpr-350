﻿using System;
using UnityEngine;

// Referenced the open-source game engine Acid for my Quaternion implementation:
// https://github.com/EQMG/Acid
// As well as some Unity documentation about their Quaternion implementation

namespace Physics3D
{
    // Made my own Quaternion class because it was easier than using Unity's
    [Serializable]
    public struct NQuaternion : IEquatable<NQuaternion>
    {
        // Basic identity Quaternion
        private static readonly NQuaternion identityNQuaternion = new NQuaternion(0.0f, 0.0f, 0.0f, 1f);

        // Do not modify these values directly unless you're Sir William Rowan Hamilton or Dan Buckstein
        public float x, y, z, w;

        // Quaternion Constructor
        public NQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        // Quaternion Constructor w/o explicit W value
        public NQuaternion(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0.0f;
        }

        // Returns the normalized version of NQuaternion this is called on
        public NQuaternion normalized => Normalize(this);
        public NQuaternion identity => identityNQuaternion;

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
        public NQuaternion Multiply(Vector3 other)
        {
            Vector3 quatVec = new Vector3(x, y, z);
            Vector3 crossOne = Vector3.Cross(other, quatVec);
            Vector3 crossTwo = Vector3.Cross(crossOne, quatVec);
            Vector3 finalVec = other + 2.0f * (crossOne * w + crossTwo);

            x = finalVec.x;
            y = finalVec.y;
            z = finalVec.z;

            return this;
        }

        // Returns the dot-product of this NQuaternion and the passed NQuaternion
        public float Dot(NQuaternion other)
        {
            return w * other.w + x * other.x + y * other.y + z * other.z;
        }

        // Returns the Dot-Product of two NQuaternions
        public static float Dot(NQuaternion lhs, NQuaternion rhs)
        {
            return lhs.Dot(rhs);
        }

        // Linearly interpolate between the two passed NQuaternions by the passed float
        public static NQuaternion Lerp(NQuaternion lhs, NQuaternion rhs, float progression)
        {
            throw new NotImplementedException();
        }

        // Smoothly linearly interpolates between the two passed NQuaternions by the passed float
        public static NQuaternion Slerp(NQuaternion lhs, NQuaternion rhs, float progression)
        {
            throw new NotImplementedException();
        }

        // Multiplies this NQuaternion by the passed Scalar value
        public NQuaternion Scale(float scalar)
        {
            x *= scalar;
            y *= scalar;
            z *= scalar;
            w *= scalar;
            return this;
        }

        // Lab 06 Step 01: Implement your own operator for quaternion multiplied by scalar
        // Scales the passed NQuaternion by the passed scalar value
        public static NQuaternion Scale(NQuaternion quat, float scalar)
        {
            return quat.Scale(scalar);
        }

        // Returns the squared length of this NQuaternion
        public float GetLengthSquared()
        {
            return x * x + y * y + z * z + w * w;
        }

        // Returns the squared length of the passed NQuaternion
        public static float GetLengthSquared(NQuaternion quat)
        {
            return quat.GetLengthSquared();
        }

        // Returns the length of this NQuaternion
        public float GetLength()
        {
            return Mathf.Sqrt(GetLengthSquared());
        }

        // Returns the length of the passed NQuaternion
        public static float GetLength(NQuaternion quat)
        {
            return quat.GetLength();
        }

        // Normalizes this NQuaternion
        public NQuaternion Normalize()
        {
            float length = GetLength();

            x /= length;
            y /= length;
            z /= length;
            w /= length;

            return this;
        }

        // Returns the normalized version of the passed NQuaternion (length of 1)
        public static NQuaternion Normalize(NQuaternion quat)
        {
            return quat.Normalize();
        }

        // Converts this NQuaternion to its Euler Angle values
        public Vector3 ToEuler()
        {
            Vector3 result = new Vector3()
            {
                x = Mathf.Atan2(2.0f * (x * w - y * z),
                    1.0f - 2.0f * (x * x + y * y)),
                y = Mathf.Asin(2.0f * (x * z + y * w)),
                z = Mathf.Atan2(2.0f * (z * w - x * y),
                    1.0f - 2.0f * (y * y + z * z))
            };

            return result;
        }

        // Converts passed NQuaternion to its Euler angle values
        public static Vector3 ToEuler(NQuaternion quat)
        {
            return quat.ToEuler();
        }

        // Adds two passed NQuaternions together
        public static NQuaternion operator +(NQuaternion lhs, NQuaternion rhs)
        {
            return lhs.Add(rhs);
        }

        // Negates the passed NQuaternion
        public static NQuaternion operator -(NQuaternion quat)
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
        public static NQuaternion operator *(Vector3 lhs, NQuaternion rhs)
        {
            return rhs.Multiply(lhs);
        }

        // Multiplies passed NQuaternion by the passed Vector3
        public static NQuaternion operator *(NQuaternion lhs, Vector3 rhs)
        {
            return lhs.Multiply(rhs);
        }

        // Scales passed NQuaternion by the passed float-scalar
        public static NQuaternion operator *(NQuaternion lhs, float rhs)
        {
            return lhs.Scale(rhs);
        }

        // Scales passed NQuaternion by the passed float-scalar
        public static NQuaternion operator *(float lhs, NQuaternion rhs)
        {
            return rhs.Scale(lhs);
        }

        // Divides left-hand NQuaternion by right-hand NQuaternion
        public static NQuaternion operator /(NQuaternion lhs, NQuaternion rhs)
        {
            NQuaternion ret = new NQuaternion()
            {
                x = lhs.x / rhs.x,
                y = lhs.y / rhs.y,
                z = lhs.z / rhs.z,
                w = lhs.w / rhs.w
            };

            return ret;
        }

        // This is how Unity's Quaternion== works behind the scenes
        public static bool operator ==(NQuaternion lhs, NQuaternion rhs)
        {
            // Only check if both NQuaternions are validS
            return Dot(lhs, rhs) > 0.999998986721039;
        }

        // Returns false if the == operation is true
        public static bool operator !=(NQuaternion lhs, NQuaternion rhs)
        {
            return !(lhs == rhs);
        }        

        // Unity wants me to implement this
        public bool Equals(NQuaternion other)
        {
            throw new NotImplementedException();
        }
    }
}
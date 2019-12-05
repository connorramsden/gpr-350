using System;
using UnityEngine;

// Referenced the open-source game engine Acid for my Quaternion implementation:
// https://github.com/EQMG/Acid
// As well as some Unity documentation about their Quaternion implementation

namespace NS_Physics3D
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
        private NQuaternion Add(NQuaternion other)
        {
            this.x += other.x;
            this.y += other.y;
            this.z += other.z;
            this.w += other.w;

            return this;
        }

        // Subtract the passed NQuaternion from this NQuaternion
        private NQuaternion Subtract(NQuaternion other)
        {
            this.x -= other.x;
            this.y -= other.y;
            this.z -= other.z;
            this.w -= other.w;
            return this;
        }

        // Multiply this NQuaternion by the passed NQuaternion
        private NQuaternion Multiply(NQuaternion other)
        {
            Vector3 thisVec = new Vector3(x, y, z);
            Vector3 otherVec = new Vector3(other.x, other.y, other.z);
            Vector3 vec = w * otherVec + other.w * thisVec + Vector3.Cross(thisVec, otherVec);

            w = w * other.w - Vector3.Dot(thisVec, otherVec);
            x = vec.x;
            y = vec.y;
            z = vec.z;

            return this;
        }

        // Multiply this NQuaternion with the passed Vector3
        private NQuaternion Multiply(Vector3 other)
        {
            NQuaternion vecToQuat = new NQuaternion(other.x, other.y, other.z, 0.0f);

            this *= vecToQuat;

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
            this.x *= scalar;
            this.y *= scalar;
            this.z *= scalar;
            this.w *= scalar;
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

            this.x /= length;
            this.y /= length;
            this.z /= length;
            this.w /= length;

            return this;
        }

        // Returns the normalized version of the passed NQuaternion (length of 1)
        public static NQuaternion Normalize(NQuaternion quat)
        {
            return quat.Normalize();
        }

        // Converts this NQuaternion to its Euler Angle values
        // https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
        public Vector3 ToEuler()
        {
            // x-axis rotation
            float sinr_cosp = 2.0f * (w * x + y * z);
            float cosr_cosp = 1.0f - 2.0f * (x * x + y * y);

            // y-axis rotation
            float sinp = 2.0f * (w * y - z * x);

            // z-axis rotation
            float siny_cosp = 2.0f * (w * z + x * y);
            float cosy_cosp = 1.0f - 2.0f * (y * y + z * z);

            Vector3 result = new Vector3()
            {
                x = Mathf.Atan2(sinr_cosp, cosr_cosp),
                y = Mathf.Asin(sinp),
                z = Mathf.Atan2(siny_cosp, cosy_cosp)
            };

            return result;
        }

        // Converts passed NQuaternion to its Euler angle values
        public static Vector3 ToEuler(NQuaternion quat)
        {
            return quat.ToEuler();
        }

        public Matrix4x4 ToRotationMatrix()
        {
            Matrix4x4 ret = new Matrix4x4
            {
                // set first row
                m00 = w * w + x * x - y * y - z * z,
                m01 = 2f * (x * y - w * z),
                m02 = 2f * (x * z + w * y),
                m03 = 0f,
                // set second row
                m10 = 2f * (x * y + w * z),
                m11 = w * w - x * x + y * y - z * z,
                m12 = 2f * (y * z - w * x),
                m13 = 0f,
                // set third row
                m20 = 2f * (x * z - w * y),
                m21 = 2f * (y * z + w * x),
                m22 = w * w - x * x - y * y + z * z,
                m23 = 0f,
                // set fourth row
                m30 = 0f,
                m31 = 0f,
                m32 = 0f,
                m33 = 1f
            };

            return ret;
        }

        public Quaternion ToQuaternion()
        {
            return new Quaternion()
            {
                x = this.x,
                y = this.y,
                z = this.z,
                w = this.w
            };
        }

        public static Quaternion ToQuaternion(NQuaternion quat)
        {
            return quat.ToQuaternion();
        }

        public void FromQuaternion(Quaternion quat)
        {
            x = quat.x;
            y = quat.y;
            z = quat.z;
            w = quat.w;
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

        // Unity wants me to implement these, don't really need them
        public bool Equals(NQuaternion other)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}

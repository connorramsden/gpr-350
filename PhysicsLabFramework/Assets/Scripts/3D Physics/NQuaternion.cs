using UnityEngine;

namespace Physics3D
{
    public class NQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public NQuaternion(float nX, float nY, float nZ, float nW)
        {
            x = nX;
            y = nY;
            z = nZ;
            w = nW;
        }

        public static NQuaternion identity {get;}
        public Vector3 eulerAngles {get; set;}
        public NQuaternion normalized {get;}

        public static float Angle(NQuaternion a, NQuaternion b)
        {
            return 0.0f;
        }

        public static NQuaternion AngleAxis(float angle, Vector3 axis)
        {
            return null;
        }

        public static NQuaternion AxisAngle(Vector3 axis, float angle)
        {
            return null;
        }

        public static float Dot(NQuaternion a, NQuaternion b)
        {
            return 0.0f;
        }

        public static NQuaternion operator*(Vector3 point, NQuaternion rotation)
        {
            return rotation * point;
        }
        public static NQuaternion operator*(NQuaternion rotation, Vector3 point)
        {
            return point * rotation;
        }
    }
}

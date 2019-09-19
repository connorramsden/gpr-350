using UnityEngine;

public class Particle2DRotation : MonoBehaviour
{
    public const float MAX_VELOCITY = 10.0f;
    public const float MAX_ACCELERATION = 10.0f;

    // Lab 01 Step 01
    // Rotation Components
    [Header("Rotation Attributes"), Tooltip("Amount of rotation per tick")]
    public float rotation;
    [Range(0, MAX_VELOCITY), Tooltip("Speed of rotation per tick")]
    public float angularVelocity;
    [Range(0, MAX_ACCELERATION), Tooltip("Multiplier for speed of rotation")]
    public float angularAccel;
}

using UnityEngine;

// Class containing Variables relevant to Particle Rotation
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

    // Lab 03 Step 02
    [Header("Torque Attributes")]
    public float torque;
    // Object Center vs Center of Mass, but not the object center
    public Vector2 localCenterOfMass, worldCenterOfMass;
    // point of applied force relative to center of mass
    public Vector2 pointOfAppliedForce;
    [Tooltip("Amount of Torque to apply")]
    public float torqueForce;
    public Vector2 appliedForce;
}

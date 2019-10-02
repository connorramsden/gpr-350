using UnityEngine;

// Class containing Variables relevant to Particle Movement
public class Particle2DMovement : MonoBehaviour
{
    // Lab 01 Step 01
    // Position Components
    [Header("Position Attributes")]
    [Tooltip("The current world position of the particle")]
    public Vector2 position;
    [Tooltip("How fast the particle will move")]
    public Vector2 velocity;
    [Tooltip("Multiplier for particle movement speed)")]
    public Vector2 acceleration;

    [Header("Forces to Apply")]
    public bool useGravity;
    public bool useNormal;
    public bool useSliding;
    public bool useFriction;
    public bool useDrag;
    public bool useSpring;

    // Lab 02 Step 01
    [Header("Force Attributes")]
    [Range(0.0f, 10.0f)]
    public float startingMass = 1.0f;
    public float surfaceAngle;
    public float coeffStaticFriction = 0.6f;
    public float coeffKineticFriction = 0.45f;
    public float fluidDensity = 1.255f;
    public float coeffDrag = 1.05f;
    public float springRestingLength = 3.0f;
    public float springStiffness = 6.2f;
}

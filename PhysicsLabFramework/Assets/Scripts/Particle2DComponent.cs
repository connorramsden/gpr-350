using UnityEngine;

public class Particle2DComponent : MonoBehaviour
{
    private const float MAX_VELOCITY = 10.0f;
    private const float MAX_ACCELERATION = 10.0f;
    private const float GRAVITY = 9.8f;
    
    // Lab 01 Step 01
    // Position Components
    [Header("Position Attributes")]
    [Tooltip("The current world position of the particle")]
    public Vector2 position;
    [Tooltip("How fast the particle will move")]
    public Vector2 velocity;
    [Tooltip("Multiplier for particle movement speed)")]
    public Vector2 acceleration;

    // Lab 01 Step 01
    // Rotation Components
    [Header("Rotation Attributes"), Tooltip("Amount of rotation per tick")]
    public float rotation;
    [Range(0, MAX_VELOCITY), Tooltip("Speed of rotation per tick")]
    public float angularVelocity;
    [Range(0, MAX_ACCELERATION), Tooltip("Multiplier for speed of rotation")]
    public float angularAccel;

    // Lab 02 Step 01
    [Header("Forces to Apply")]
    public bool useGravity;
    public bool useNormal;
    public bool useSliding;
    public bool useFriction;
    public bool useDrag;
    public bool useSpring;
    
    [Header("Force Attributes")]
    [Range(0.0f, 10.0f)]
    public float startingMass = 1.0f;
    public float surfaceAngle;

    public float mass
    {
        get; private set;
    }

    public float massInv
    {
        get; private set;
    }

    public void SetMass(float newMass)
    {
        // Newton-2 Integration for Force
        mass = Mathf.Max(0.0f, newMass);
        massInv = mass > 0.0f ? 1.0f / mass : 0.0f;

    }

    // Lab 02 Step 02 - Declaring Forces
    // Total force acting on a particle
    private Vector2 force;
    public GameObject anchorObject;
    public Vector2 anchorPoint;

    // Variables to store possible forces
    private Vector2 f_gravity, f_normal, f_sliding, f_friction, f_drag, f_spring;

    private void AddForce(Vector2 newForce)
    {
        // D'Alembert's Law (French Guy!)
        force += newForce;
    }

    public void ApplyForces()
    {
        // Calculate the surface normal based on the surface angle
        Vector2 surfaceNormal = new Vector2(Mathf.Cos(surfaceAngle * Mathf.Deg2Rad), Mathf.Sin(surfaceAngle * Mathf.Deg2Rad));

        if (useGravity)
        {
            f_gravity = ForceGenerator.GenerateForce_Gravity(mass, -GRAVITY, Vector2.up);
            AddForce(f_gravity);
        }
        else
        {
            f_gravity = Vector2.zero;
        }

        if (useNormal)
        {
            // Gravity used in this formula, must be ticked on
            if (!useGravity && surfaceAngle > 0.0f)
                useGravity = true;
            f_normal = ForceGenerator.GenerateForce_Normal(f_gravity, surfaceNormal);
            AddForce(f_normal);
        }
        else
        {
            f_normal = Vector2.zero;
        }

        if (useSliding)
        {
            // Gravity used in this formula, must be ticked on
            if (!useGravity)
                useGravity = true;
            // Normal used in this formula, must be ticked on
            if (!useNormal)
                useNormal = true;
            f_sliding = ForceGenerator.GenerateForce_Sliding(f_gravity, f_normal);
            AddForce(f_sliding);
        }
        else
        {
            f_sliding = Vector2.zero;
        }

        if (useFriction)
        {
            // Normal used in this formula, must be ticked on
            if (!useNormal)
                useNormal = true;
            f_friction = ForceGenerator.GenerateForce_Friction_Standard(f_normal, velocity, f_sliding, 0.61f, 0.47f);
            AddForce(f_friction);
        }

        if (useDrag)
        {
            f_drag = ForceGenerator.GenerateForce_Drag(velocity, velocity, 0.001225f, 1.0f, 1.05f);
            AddForce(f_drag);
        }
        else
        {
            f_drag = Vector2.zero;
        }

        if (useSpring)
        {
            f_spring = ForceGenerator.GenerateForce_Spring(position, anchorPoint, 5.0f, 6.4f);
            AddForce(f_spring);
        }
        else
        {
            f_spring = Vector2.zero;
        }
    }

    public void UpdateAcceleration()
    {
        // Newton2
        acceleration = force * massInv;

        // reset because they're coming back next frame probably
        force = Vector2.zero;
    }

    [Header("Additional Movement Attributes")]
    [Tooltip("Should the particle be able to move?")]
    public bool shouldMove;
    [Tooltip("Should the particle be able to rotate?")]
    public bool shouldRotate;

    // Updates a Particle's position based on KINEMATIC integration
    public void UpdatePosition(float dt)
    {
        // Use the Kinematic formula for movement integration
        // x(t+dt) = x(t) + v(t)dt + 1/2(a(t)dt^2)
        position += (velocity * dt) + (0.5f * acceleration * (dt * dt));

        // Update velocity based on acceleration
        velocity += acceleration * dt;

        // Update GO position based on calculated rotational physics
        transform.position = position;
    }

    // Update's a particle's rotation based on KINEMATIC integration
    public void UpdateRotation(float dt)
    {
        // Use Kinematic formula for rotation integration
        rotation += (angularVelocity * dt) + (0.5f * angularAccel * (dt * dt));

        // Update rotational velocity based on angular acceleration
        angularVelocity += angularAccel * dt;

        // Update GO rotation based on calculated rotational physics
        transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, rotation));
    }

    private void Awake()
    {
        // Upon Awake(), set the object's tag to Particle
        // if it is not already set
        if (!gameObject.CompareTag("Particle"))
        {
            gameObject.tag = "Particle";
        }

        position = transform.position;
        rotation = transform.rotation.eulerAngles.z;
    }

    private void Start()
    {
        // Initialize the anchor point to the passed GameObject's position
        anchorPoint = anchorObject.transform.position;
    }
}

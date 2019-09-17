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
    public bool useFrictionStatic;
    public bool useFrictionKinetic;
    public bool useDrag;
    public bool useSpring;
    
    [Header("Force Attributes")]
    [Range(0.0f, 10.0f)]
    public float startingMass = 1.0f;
    public float surfaceAngle;
    public Vector2 additionalForce = Vector2.zero;

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

    private Vector2 f_gravity;
    private Vector2 f_normal;
    private Vector2 f_sliding;
    private Vector2 f_friction_static;
    private Vector2 f_friction_kinetic;
    private Vector2 f_drag;
    private Vector2 f_spring;

    public void AddForce(Vector2 newForce)
    {
        // D'Alembert's Law (French Guy!)
        force += newForce;
    }

    public void ApplyForces()
    {
        if (useGravity)
            AddForce(f_gravity);
        if (useNormal)
            AddForce(f_normal);
        if (useSliding)
            AddForce(f_sliding);
        if (useFrictionStatic)
            AddForce(f_friction_static);
        if (useFrictionKinetic)
            AddForce(f_friction_kinetic);
        if (useDrag)
            AddForce(f_drag);
        if (useSpring)
            AddForce(f_spring);
        AddForce(additionalForce);
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

        // Update Particle's world position based on local (script) position
        transform.position = position;

        // Must pass negative gravitationalConstant
        f_gravity = ForceGenerator.GenerateForce_Gravity(mass, -GRAVITY, Vector2.up);

        Vector2 surfaceNormal_unit = new Vector2(Mathf.Sin(surfaceAngle), Mathf.Cos(surfaceAngle));

        f_normal = ForceGenerator.GenerateForce_Normal(f_gravity, surfaceNormal_unit);
        f_sliding = ForceGenerator.GenerateForce_Sliding(f_gravity, f_normal);
        f_friction_static = ForceGenerator.GenerateForce_Friction_Static(f_normal, velocity, 0.1f);
        f_friction_kinetic = ForceGenerator.GenerateForce_Friction_Kinetic(f_normal, velocity, 0.1f);
        f_drag = ForceGenerator.GenerateForce_Drag(velocity, velocity, 1.0f, 2.0f, 1.0f);
        // f_spring = ForceGenerator.GenerateForce_Spring(position, new Vector2()), 1.0f, 1.0f);
    }

    // Update's a particle's rotation based on KINEMATIC integration
    public void UpdateRotation(float dt)
    {
        rotation += (angularVelocity * dt) + (0.5f * angularAccel * (dt * dt));

        angularVelocity += angularAccel * dt;

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
}

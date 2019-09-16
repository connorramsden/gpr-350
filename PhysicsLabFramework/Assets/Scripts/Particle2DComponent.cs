using UnityEngine;

public class Particle2DComponent : MonoBehaviour
{
    private const float MAX_VELOCITY = 10.0f;
    private const float MAX_ACCELERATION = 10.0f;
    private const float GRAVITY = 9.8f;
    public GameObject ramp;

    public enum ForceType
    {
        F_GRAVITY,
        F_NORMAL,
        F_SLIDING,
        F_FRICTION_STATIC,
        F_FRICTION_KINETIC,
        F_DRAG,
        F_SPRING
    }

    public ForceType forceToUse;

    // Lab 01 Step 01
    // Position Components
    [Header("Position Attributes")]
    [Tooltip("The current world position of the particle")]
    public Vector3 position;
    [Tooltip("How fast the particle will move")]
    public Vector3 velocity;
    [Tooltip("Multiplier for particle movement speed)")]
    public Vector3 acceleration;

    // Lab 01 Step 01
    // Rotation Components
    [Header("Rotation Attributes"), Tooltip("Amount of rotation per tick")]
    public float rotation;
    [Range(0, MAX_VELOCITY), Tooltip("Speed of rotation per tick")]
    public float angularVelocity;
    [Range(0, MAX_ACCELERATION), Tooltip("Multiplier for speed of rotation")]
    public float angularAccel;

    // Lab 02 Step 01
    [Header("Force Attributes")]
    [Range(0.0f, 10.0f)]
    public float startingMass = 1.0f;

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
    private Vector3 force;

    private Vector3 f_gravity;                     
    private Vector3 f_normal;                      
    private Vector3 f_sliding;                     
    private Vector3 f_friction_static;             
    private Vector3 f_friction_kinetic;            
    private Vector3 f_drag;                        
    private Vector3 f_spring;                      

    public void AddForce(Vector3 newForce)
    {
        // D'Alembert's Law (French Guy!)
        force += newForce;
    }

    public void UtilizeForce()
    {
        switch (forceToUse)
        {
            case ForceType.F_NORMAL:
                AddForce(f_normal);
                break;
            case ForceType.F_SLIDING:
                AddForce(f_sliding);
                break;
            case ForceType.F_FRICTION_STATIC:
                AddForce(f_friction_static);
                break;
            case ForceType.F_FRICTION_KINETIC:
                AddForce(f_friction_kinetic);
                break;
            case ForceType.F_DRAG:
                AddForce(f_drag);
                break;
            case ForceType.F_SPRING:
                AddForce(f_spring);
                break;
            case ForceType.F_GRAVITY:
            default:
                AddForce(f_gravity);
                break;
        }
    }

    public void UpdateAcceleration()
    {
        // Newton2
        acceleration = force * massInv;

        // reset because they're coming back next frame probably
        force = Vector3.zero;
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
        f_gravity = ForceGenerator.GenerateForce_Gravity(mass, -GRAVITY, Vector3.up);
        float rampAngle = 0.0f;

        if (ramp)
        {
            rampAngle = ramp.GetComponent<Transform>().eulerAngles.x;
        }

        Vector3 surfaceNormal_unit = new Vector3(Mathf.Sin(rampAngle), Mathf.Cos(rampAngle));

        f_normal = ForceGenerator.GenerateForce_Normal(f_gravity, surfaceNormal_unit);
        f_sliding = ForceGenerator.GenerateForce_Sliding(f_gravity, f_normal);


        UtilizeForce();
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

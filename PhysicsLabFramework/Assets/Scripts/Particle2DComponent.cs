using UnityEngine;

// Class containing Variables and Methods relevant to General Particle Operation
// Require a Particle2D Movement & Rotation component on Particle2D objects
[RequireComponent(typeof(Particle2DMovement), typeof(Particle2DRotation))]
public class Particle2DComponent : MonoBehaviour
{
    // Gravitational Constant
    public const float GRAVITY = 9.8f;

    private Particle2DMovement particleMovement;
    private Particle2DRotation particleRotation;

    public enum ParticleShape
    {
        INVALID_TYPE = -1,
        DISK,
        RING,
        RECTANGLE,
        ROD
    }

    public ParticleShape particleShape;

    // Particle's mass
    public float mass
    {
        get; private set;
    }

    // Particle's inverse mass
    public float massInv
    {
        get; private set;
    }

    // Rotational Equivalent of Mass
    // Moment of Inertia
    // A measure of how difficult it is to change a particle's rotation speed (Millington p.198)
    public float inertia
    {
        get; private set;
    }

    // Particle's inverse Moment of Inertia
    public float inertiaInv
    {
        get; private set;
    }

    // Values necessary for Torque / Inertia / Rotation
    public float length, height, radius, innerRadius, outerRadius;

    public float GetStartingMass()
    {
        return particleMovement.startingMass;
    }

    public void SetMass(float newMass)
    {
        // Newton-2 Integration for Force
        mass = Mathf.Max(0.0f, newMass);
        massInv = mass > 0.0f ? 1.0f / mass : 0.0f;
    }

    // Sets the inertia and inverse inertia of a particle based on shape
    public void SetInertia()
    {
        // Inertia Calculations based on Shape from Millington 2nd Ed. Appendix A (Useful Inertia Tensors)
        switch (particleShape)
        {
            case ParticleShape.DISK:
                {
                    // I = 1/2 * mass * radius * radius
                    inertia = 0.5f * mass * radius * radius;
                    break;
                }
            case ParticleShape.RING:
                {
                    // I = 1/2 * mass * (outerRadius * outerRadius + innerRadius*innerRadius)
                    inertia = 0.5f * mass * outerRadius * outerRadius + innerRadius * innerRadius;
                    break;
                }
            case ParticleShape.RECTANGLE:
                {
                    // I = 1/12 * mass * (dx*dx + dy*dy) where dx = length and dy = height
                    inertia = 0.083f * mass * length * length + height * height;
                    break;
                }
            case ParticleShape.ROD:
                {
                    // I + 1/12 * mass * length * length
                    inertia = 0.083f * mass * length * length;
                    break;
                }
            case ParticleShape.INVALID_TYPE:
            default:
                Debug.LogError($"Particle Shape is of type {ParticleShape.INVALID_TYPE}. Please set a valid Particle Shape");
                inertia = 0.0f;
                break;
        }

        inertiaInv = inertia > 0.0f ? 1.0f / inertia : 0.0f;
    }

    // Lab 02 Step 02 - Declaring Force Variables
    // Total force acting on a particle
    private Vector2 force;
    public GameObject anchorObject;
    private Vector2 anchorPoint;

    // Variables to store possible forces
    private Vector2 f_gravity, f_normal, f_sliding, f_friction, f_drag, f_spring;

    // Adds the passed force to the current force vector
    private void AddForce(Vector2 newForce)
    {
        // D'Alembert's Principle (French Guy!)
        force += newForce;
    }

    // Applys all currently-enabled forces to the particle
    public void ApplyForces()
    {
        // Calculate the surface normal based on the surface angle
        Vector2 surfaceNormal = new Vector2(Mathf.Cos(particleMovement.surfaceAngle * Mathf.Deg2Rad), Mathf.Sin(particleMovement.surfaceAngle * Mathf.Deg2Rad));

        if (particleMovement.useGravity)
        {
            f_gravity = ForceGenerator.GenerateForce_Gravity(mass, -GRAVITY, Vector2.up);
            AddForce(f_gravity);
        }
        else
        {
            f_gravity = Vector2.zero;
        }

        if (particleMovement.useNormal)
        {
            // Gravity used in this formula, must be ticked on
            if (!particleMovement.useGravity && particleMovement.surfaceAngle > 0.0f)
                particleMovement.useGravity = true;
            f_normal = ForceGenerator.GenerateForce_Normal(f_gravity, surfaceNormal);
            AddForce(f_normal);
        }
        else
        {
            f_normal = Vector2.zero;
        }

        if (particleMovement.useSliding)
        {
            // Gravity used in this formula, must be ticked on
            if (!particleMovement.useGravity)
                particleMovement.useGravity = true;
            // Normal used in this formula, must be ticked on
            if (!particleMovement.useNormal)
                particleMovement.useNormal = true;
            f_sliding = ForceGenerator.GenerateForce_Sliding(f_gravity, f_normal);
            AddForce(f_sliding);
        }
        else
        {
            f_sliding = Vector2.zero;
        }

        if (particleMovement.useFriction)
        {
            // Normal used in this formula, must be ticked on
            if (!particleMovement.useNormal)
                particleMovement.useNormal = true;
            f_friction = ForceGenerator.GenerateForce_Friction_Standard(f_normal, particleMovement.velocity, f_sliding, particleMovement.coeffStaticFriction, particleMovement.coeffKineticFriction);
            AddForce(f_friction);
        }

        if (particleMovement.useDrag)
        {
            f_drag = ForceGenerator.GenerateForce_Drag(particleMovement.velocity, particleMovement.fluidDensity, 1.0f, particleMovement.coeffDrag);
            AddForce(f_drag);
        }
        else
        {
            f_drag = Vector2.zero;
        }

        if (particleMovement.useSpring)
        {
            f_spring = ForceGenerator.GenerateForce_Spring(particleMovement.position, anchorPoint, particleMovement.springRestingLength, particleMovement.springStiffness);
            AddForce(f_spring);
        }
        else
        {
            f_spring = Vector2.zero;
        }
    }

    // Converts force and inverse mass to acceleration
    public void UpdateAcceleration()
    {
        // Newton2
        particleMovement.acceleration = force * massInv;

        // reset because they're coming back next frame probably
        force = Vector2.zero;
    }

    public void ApplyTorque()
    {
        // D'Alembert's Principle:
        // T = pf * F where T is torque, pf is moment arm, and F is applied force at pf
        // Center of mass not necessarily object center

        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
            particleRotation.torque += particleRotation.torqueForce * (particleRotation.momentArm - particleRotation.worldCenterOfMass).magnitude;
        }
    }

    // Converts torque to angular acceleration and then resets torque
    public void UpdateAngularAcceleration()
    {
        //  Newton-2 Integration for AngularAcceleration
        //  torque = inertia * alpha
        //  alpha = inverseInertia * torque
        float alpha = inertiaInv * particleRotation.torque;
        particleRotation.angularAccel = alpha;

        particleRotation.torque = 0.0f;
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
        particleMovement.position += (particleMovement.velocity * dt) + (0.5f * particleMovement.acceleration * (dt * dt));

        // Update velocity based on acceleration
        particleMovement.velocity += particleMovement.acceleration * dt;

        // Update GO position based on calculated rotational physics
        transform.position = particleMovement.position;
    }

    // Update's a particle's rotation based on KINEMATIC integration
    public void UpdateRotation(float dt)
    {
        // Use Kinematic formula for rotation integration
        particleRotation.rotation += (particleRotation.angularVelocity * dt) + (0.5f * particleRotation.angularAccel * (dt * dt));

        // Update rotational velocity based on angular acceleration
        particleRotation.angularVelocity += particleRotation.angularAccel * dt;

        // Update GO rotation based on calculated rotational physics
        transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, particleRotation.rotation));
    }

    // Initializes local variables
    private void Awake()
    {
        // Upon Awake(), set the object's tag to Particle
        // if it is not already set
        if (!gameObject.CompareTag("Particle"))
        {
            gameObject.tag = "Particle";
        }

        particleMovement = gameObject.GetComponent<Particle2DMovement>();
        particleRotation = gameObject.GetComponent<Particle2DRotation>();

        particleMovement.position = transform.position;
        particleRotation.rotation = transform.rotation.eulerAngles.z;
    }

    // Initializes external variables
    private void Start()
    {
        // Initialize the anchor point to the passed GameObject's position
        if (anchorObject)
            anchorPoint = anchorObject.transform.position;
        else
            anchorPoint = Vector2.zero;
    }
}

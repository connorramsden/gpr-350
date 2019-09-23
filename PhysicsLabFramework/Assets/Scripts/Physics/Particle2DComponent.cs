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
        CUBOID,
        SPHERE,
        CYLINDER,
        CONE
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
    public Vector3 inertia
    {
        get; private set;
    }

    // Particle's inverse Moment of Inertia
    public Vector3 inertiaInv
    {
        get; private set;
    }

    // Values necessary for Torque / Inertia / Rotation
    public float length, width, height, radius;

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

    public void SetInertia()
    {
        // Inertia Calculations based on Shape from Millington Appendix A.3

        float inertX = 0.0f;
        float inertY = 0.0f;
        float inertZ = 0.0f;

        switch (particleShape)
        {
            case ParticleShape.CUBOID:
            {
                // inertia.x = 1/12 * mass * (dy^2) + (dz^2);
                inertX = 1 / 12 * mass;
                // inertia.y = 1/12 * mass * (dx^2) + (dz^2);
                inertY = 1 / 12 * mass;
                // inertia.z = 1/12 * mass * (dx^2) + (dy^2);
                inertZ = 1 / 12 * mass;

                break;
            }
            case ParticleShape.SPHERE:
            {
                // inertia.x = 2/5 * mass * radius * radius
                inertX = 2 / 5 * mass * radius * radius;
                // inertia.y = 2/5 * mass * radius * radius
                inertY = 2 / 5 * mass * radius * radius;
                // inertia.z = 2/5 * mass * radius * radius
                inertZ = 2 / 5 * mass * radius * radius;
                break;
            }
            case ParticleShape.CYLINDER:
            {
                // inertia.x = 1/12 * mass * height*height + 1/4 * mass * radius*radius
                inertX = 1 / 12 * mass * height * height + 1 / 4 * mass * radius * radius;
                // inertia.y = 1/12 * mass * height*height + 1/4 * mass * radius*radius
                inertY = 1 / 12 * mass * height * height + 1 / 4 * mass * radius * radius;
                // inertia.z = 1/2 * mass * radius*radius
                inertZ = 1 / 2 * mass * radius * radius;
                break;
            }
            case ParticleShape.CONE:
            {
                // inertia.x = 3/80 * mass * height*height + 3/20 * mass * radius*radius
                inertX = 3 / 80 * mass * height * height + 3 / 20 * mass * radius * radius;
                // inertia.y = 3/80 * mass * height*height + 3/20 * mass * radius*radius
                inertY = 3 / 80 * mass * height * height + 3 / 20 * mass * radius * radius;
                // inertia.z = 3/10 * mass * radius * radius
                inertZ = 3 / 10 * mass * radius * radius;
                break;
            }
            default:
                break;
        }

        inertia.Set(inertX, inertY, inertZ);
    }

    public Vector2 GetPosition()
    {
        return particleMovement.position;
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
        // D'Alembert's Law (French Guy!)
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

    // Converts torque to angular acceleration and then resets torque
    public void UpdateAngularAcceleration()
    {
        //  Newton-2 Integration for AngularAcceleration
        //  torque = inertia * alpha
        //  alpha = inverseInertia * torque

        // 
        // float alpha = inertiaInv * particleRotation.torque;
        // particleRotation.angularAccel = inertia * alpha;

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

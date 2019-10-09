using UnityEngine;

// Class containing Variables and Methods relevant to General Particle Operation
// Require a Particle2D Movement & Rotation component on Particle2D objects
[RequireComponent(typeof(Particle2DMovement), typeof(Particle2DRotation))]
public class Particle2DComponent : MonoBehaviour
{
    // Stores variables for a particle's movement
    public Particle2DMovement movement
    {
        get; private set;
    }

    // Stores variables for a particle's rotation
    public Particle2DRotation rotation
    {
        get; private set;
    }
    
    // Shapes for Torque-based rotation in 2D
    public enum ParticleShape
    {
        INVALID_TYPE = -1,
        DISK,
        RECTANGLE,
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
    public float length, width, height, radius, innerRadius, outerRadius;

    // Return a particle's starting mass
    public float GetStartingMass()
    {
        return movement.startingMass;
    }

    // Set mass to the passed mass, and update inverse mass
    public void SetMass(float newMass)
    {
        // Newton-2 Integration for Force
        mass = Mathf.Max(0.0f, newMass);
        massInv = mass > 0.0f ? 1.0f / mass : 0.0f;
    }

    // Sets inertia for objects in Two-Dimensional space.
    public void SetInertia()
    {
        // Inertia Calculations based on Shape from Millington 2nd Ed. Appendix A (Useful Inertia Tensors)
        switch (particleShape)
        {
            // Inertia Formula for a Disk-like object
            case ParticleShape.DISK:
                {
                    // I = 1/2 * mass * radius * radius
                    inertia = 0.5f * mass * radius * radius;
                    break;
                }
            // Inertia Formula for a Rectangle-like object
            case ParticleShape.RECTANGLE:
                {
                    // I = 1/12 * mass * (dx*dx + dy*dy) where dx = length and dy = height
                    inertia = 0.083f * mass * length * length + height * height;
                    break;
                }
            // If the shape is invalid / default, log an Error for the user
            case ParticleShape.INVALID_TYPE:
            default:
                Debug.LogError($"Particle Shape is of type {ParticleShape.INVALID_TYPE}. Please set a valid Particle Shape");
                inertia = 0.0f;
                break;
        }

        // If inertia is greater than 0, inverse inertia is 1 / inertia
        inertiaInv = inertia > 0.0f ? 1.0f / inertia : 0.0f;
    }

    // Either returns the Particle's position or the GO transform position
    public Vector2 GetPosition()
    {
        if (movement && shouldMove)
            return movement.position;
        else
            return transform.position;
    }

    // Lab 02 Step 02 - Declaring Force Variables
    // Total force acting on a particle
    private Vector2 force;

    // Variables to store possible forces
    private Vector2 f_gravity, f_normal, f_sliding, f_friction;

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
        Vector2 surfaceNormal = new Vector2(Mathf.Cos(movement.surfaceAngle * Mathf.Deg2Rad), Mathf.Sin(movement.surfaceAngle * Mathf.Deg2Rad));

        if (movement.useGravity)
        {
            f_gravity = ForceGenerator.GenerateForce_Gravity(mass, Vector2.up);
            AddForce(f_gravity);
        }
        else
            f_gravity = Vector2.zero;

        if (movement.useNormal)
        {
            // Gravity used in this formula, must be ticked on
            if (!movement.useGravity && movement.surfaceAngle > 0.0f)
                movement.useGravity = true;
            f_normal = ForceGenerator.GenerateForce_Normal(f_gravity, surfaceNormal);
            AddForce(f_normal);
        }
        else
            f_normal = Vector2.zero;

        if (movement.useSliding)
        {
            // Gravity used in this formula, must be ticked on
            if (!movement.useGravity)
                movement.useGravity = true;
            // Normal used in this formula, must be ticked on
            if (!movement.useNormal)
                movement.useNormal = true;
            f_sliding = ForceGenerator.GenerateForce_Sliding(f_gravity, f_normal);
            AddForce(f_sliding);
        }
        else
            f_sliding = Vector2.zero;

        if (movement.useFriction)
        {
            // Sliding used in this formula, must be ticked on, will tick-on Gravity & Normal
            if (!movement.useSliding)
                movement.useSliding = true;
            f_friction = ForceGenerator.GenerateForce_Friction_Standard(f_normal, movement.velocity, f_sliding, movement.coeffStaticFriction, movement.coeffKineticFriction);
            AddForce(f_friction);
        }
        else
            f_friction = Vector2.zero;
    }

    // Converts force and inverse mass to acceleration
    public void UpdateAcceleration()
    {
        // Newton2
        movement.acceleration = force * massInv;

        // reset because they're coming back next frame probably
        force = Vector2.zero;
    }

    // Apply a given torque-force to the rotating objects
    public void ApplyTorque()
    {
        // D'Alembert's Principle:
        // T = cross(pf, F) where T is torque being applied, pf is the moment-of-inertia-arm, and F is the force applied at the Moment ARm
        // Center of mass not necessarily object center, so two variables exist: localCenterOfMass & worldCenterOfMass

        Vector2 momentArm = (rotation.pointOfAppliedForce - rotation.worldCenterOfMass);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rotation.torque -= momentArm.x * rotation.appliedForce.y - momentArm.y * rotation.appliedForce.x;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rotation.torque += momentArm.x * rotation.appliedForce.y - momentArm.y * rotation.appliedForce.x;
        }
    }

    // Converts torque to angular acceleration and then resets torque
    public void UpdateAngularAcceleration()
    {
        //  Newton-2 Integration for AngularAcceleration
        //  torque = inertia * alpha
        //  alpha = inverseInertia * torque
        float alpha = inertiaInv * rotation.torque;
        rotation.angularAccel = alpha;

        // Reset torque as it is getting updated again next frame
        rotation.torque = 0.0f;
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
        movement.position += (movement.velocity * dt) + (0.5f * movement.acceleration * (dt * dt));

        // Update velocity based on acceleration
        movement.velocity += movement.acceleration * dt;

        // Update GO position based on calculated rotational physics
        transform.position = movement.position;
    }

    // Update's a particle's rotation based on KINEMATIC integration
    public void UpdateRotation(float dt)
    {
        // Use Kinematic formula for rotation integration
        rotation.rotation += (rotation.angularVelocity * dt) + (0.5f * rotation.angularAccel * (dt * dt));

        // Update rotational velocity based on angular acceleration
        rotation.angularVelocity += rotation.angularAccel * dt;

        rotation.angularVelocity = Mathf.Clamp(rotation.angularVelocity, -Mathf.PI, Mathf.PI);

        // Clamp rotation to -PI, PI
        rotation.rotation = Mathf.Clamp(rotation.rotation, -Mathf.PI, Mathf.PI);

        // Update GO rotation based on calculated rotational physics
        transform.Rotate(new Vector3(0.0f, 0.0f, rotation.rotation));
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

        movement = gameObject.GetComponent<Particle2DMovement>();
        rotation = gameObject.GetComponent<Particle2DRotation>();

        movement.position = transform.position;
        rotation.rotation = transform.rotation.eulerAngles.z;
    }
}

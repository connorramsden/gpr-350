using UnityEngine;

public class Particle2DComponent : MonoBehaviour
{
    private const float MAX_VELOCITY = 10.0f;
    private const float MAX_ACCELERATION = 10.0f;

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
        mass = Mathf.Max(0.0f, newMass);
        massInv = mass > 0.0f ? 1.0f / mass : 0.0f;

    }

    // Lab 02 Step 02
    // Total force acting on a particle
    Vector2 force;

    public void AddForce(Vector2 newForce)
    {
        // D'Alembert's Law (French Guy!)
        force += newForce;
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

    /*
    public enum IntegrationType
    {
        EULER,
        KINEMATIC
    }

    [Header("Integration Type")]
    public IntegrationType currentIntegration;
    */

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

        // SET AS CONSTANT LATER
        // Vector2 f_gravity = mass * new Vector2(0.0f, -9.8f);

        // Must pass negative gravitationalConstant
        AddForce(ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up));
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

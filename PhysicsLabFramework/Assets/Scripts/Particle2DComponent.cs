using UnityEngine;

public class Particle2DComponent : MonoBehaviour
{
    // Position Components
    public Vector2 position, velocity, acceleration;
    // Rotation Components
    public float rotation, angularVelocity, angularAccel;

    [Header("Move || Rotate")]
    public bool shouldMove;
    public bool shouldRotate;

    // Bonus Bell's and Whistles
    public enum IntegrationType
    {
        EULER,
        KINEMATIC
    }

    [Header("Integration Type")]
    public IntegrationType currentIntegration;

    // Updates a particle's position based on current i+ntegration type
    public void UpdatePosition(float dt)
    {
        // Utilize Euler Explicit Integration formula
        // x(t+dt) = x(t) + v(t)dt
        if (currentIntegration.Equals(IntegrationType.EULER))
        {
            position += velocity * dt;
        }
        // Use the Kinematic formula for movement integration
        // x(t+dt) = x(t) + v(t)dt + 1/2(a(t)dt^2)
        else if (currentIntegration.Equals(IntegrationType.KINEMATIC))
        {
            position += (velocity * dt) + (0.5f * acceleration * (dt * dt));
        }

        velocity += acceleration * dt;

        // step4
        // test by faking motion along a curve
        acceleration.x = -Mathf.Sin(Time.time);

        transform.position = new Vector3(position.x, transform.position.y);
    }

    public void UpdateRotation(float dt)
    {
        // Utilize Euler Explicit Integration formula for rotation
        if (currentIntegration.Equals(IntegrationType.EULER))
        {
            rotation += angularVelocity * dt;
        }
        // Utilize Kinematic formula for rotation integration
        else if (currentIntegration.Equals(IntegrationType.KINEMATIC))
        {
            rotation += (angularVelocity * dt) + (0.5f * angularAccel * (dt * dt));
        }

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

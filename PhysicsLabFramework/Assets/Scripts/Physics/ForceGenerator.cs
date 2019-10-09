using UnityEngine;

// Class containing Methods relevant to Force Generation
public class ForceGenerator : MonoBehaviour
{
    // Gravitational Constant
    public const float GRAVITY = 9.8f;

    // Must pass negative gravitationalConstant
    public static Vector2 GenerateForce_Gravity(float particleMass, Vector2 worldUp)
    {
        // f = mg
        Vector2 f_gravity = particleMass * -GRAVITY * worldUp;

        return f_gravity;
    }

    public static Vector2 GenerateForce_Normal(Vector2 f_gravity, Vector2 surfaceNormal_unit)
    {
        // f = proj(f_gravity2, surfaceNormal_unit)
        return Vector3.ProjectOnPlane(-f_gravity, surfaceNormal_unit);
    }

    public static Vector2 GenerateForce_Sliding(Vector2 f_gravity, Vector2 f_normal)
    {
        // f = f_gravity + f_normal
        Vector2 f_sliding = f_gravity + f_normal;

        return f_sliding;
    }

    public static Vector2 GenerateForce_Friction_Static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient_static)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)

        float max = frictionCoefficient_static * f_normal.magnitude;
        float opposingForce = f_opposing.magnitude;

        if (opposingForce < max)
            return -f_opposing;
        else
            return -f_opposing * max / opposingForce;

    }

    public static Vector2 GenerateForce_Friction_Kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f = -coeff*abs(f_normal) * unit(vel)
        Vector2 f_friction_kinetic = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized;

        return f_friction_kinetic;
    }

    public static Vector2 GenerateForce_Friction_Standard(Vector2 f_normal, Vector2 particleVelocity, Vector2 f_opposing, float frictionCoeff_static, float frictionCoeff_kinetic)
    {

        if (particleVelocity.magnitude <= 0.0f)
        {
            return GenerateForce_Friction_Static(f_normal, f_opposing, frictionCoeff_static);
        }
        else
        {
            return GenerateForce_Friction_Kinetic(f_normal, particleVelocity, frictionCoeff_static);
        }
    }
}

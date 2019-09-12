using UnityEngine;

public class ForceGenerator : MonoBehaviour
{
    // Must pass negative gravitationalConstant
    public static Vector2 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        // f = mg
        Vector2 f_gravity = particleMass * gravitationalConstant * worldUp;

        return f_gravity;
    }

    public static Vector2 GenerateForce_Normal(Vector2 f_gravity, Vector2 surfaceNormal_unit)
    {
        // f = proj(f_gravity, surfaceNormal_unit)
        Vector2 f_normal = Vector3.Project(f_gravity, surfaceNormal_unit);

        return f_normal;
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

        Vector2 f_friction_static;

        float max = frictionCoefficient_static * Mathf.Abs(f_normal.magnitude);
        
        return f_friction_static;
    }

    public static Vector2 GenerateForce_Friction_Kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f = -coeff*abs(f_normal) * unit(vel)
        Vector2 f_friction_kinetic = Vector2.zero;
        return f_friction_kinetic;
    }

    public static Vector2 GenerateForce_Drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // f = (p * u^2 * area * coeff) / 2
        Vector2 f_drag = Vector2.zero;

        return f_drag;
    }

    public static Vector2 GenerateForce_Spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // f = -coeff*(spring length - spring resting length)
        Vector2 f_spring = Vector2.zero;

        return f_spring;
    }
}

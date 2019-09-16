using UnityEngine;

public class ForceGenerator : MonoBehaviour
{
    // Must pass negative gravitationalConstant
    public static Vector3 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector3 worldUp)
    {
        // f = mg
        Vector3 f_gravity = particleMass * gravitationalConstant * worldUp;

        return f_gravity;
    }

    public static Vector3 GenerateForce_Normal(Vector3 f_gravity, Vector3 surfaceNormal_unit)
    {
        // f = proj(f_gravity, surfaceNormal_unit)
        Vector3 f_normal = Vector3.ProjectOnPlane(f_gravity, surfaceNormal_unit);

        return f_normal;
    }

    public static Vector3 GenerateForce_Sliding(Vector3 f_gravity, Vector3 f_normal)
    {
        // f = f_gravity + f_normal
        Vector3 f_sliding = f_gravity + f_normal;

        return f_sliding;
    }
                        
    public static Vector3 GenerateForce_Friction_Static(Vector3 f_normal, Vector3 f_opposing, float frictionCoefficient_static)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)

        float max = frictionCoefficient_static * f_normal.magnitude;

        if (f_opposing.magnitude < max)
            return -f_opposing;
        else
            return -frictionCoefficient_static * f_normal;

    }
                        
    public static Vector3 GenerateForce_Friction_Kinetic(Vector3 f_normal, Vector3 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f = -coeff*abs(f_normal) * unit(vel)
        Vector3 f_friction_kinetic = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized;

        return f_friction_kinetic;
    }
                        
    public static Vector3 GenerateForce_Drag(Vector3 particleVelocity, Vector3 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // f = (p * v^2 * area * coeff) / 2
        Vector3 velocity = particleVelocity - fluidVelocity;
        Vector3 f_drag = (fluidDensity * (velocity.sqrMagnitude) * objectArea_crossSection * objectDragCoefficient) / 2.0f;

        return f_drag;
    }
                        
    public static Vector3 GenerateForce_Spring(Vector3 particlePosition, Vector3 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // f = -coeff*(spring length - spring resting length)
        Vector3 springLengthVector = particlePosition - anchorPosition;
        float springLength = springLengthVector.magnitude;
        // Vector3 f_spring = -springStiffnessCoefficient * (springLength - springRestingLength);

        return Vector3.zero;
    }
}

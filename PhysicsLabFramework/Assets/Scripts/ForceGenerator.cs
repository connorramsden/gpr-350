﻿using UnityEngine;

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
        // f = proj(f_gravity2, surfaceNormal_unit)
        return Vector3.ProjectOnPlane(f_gravity, surfaceNormal_unit);
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
        
        if (-f_opposing.x < max)
            return -f_opposing;
        else
            return -frictionCoefficient_static * f_normal;

    }

    public static Vector2 GenerateForce_Friction_Kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f = -coeff*abs(f_normal) * unit(vel)
        Vector2 f_friction_kinetic = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized;

        return f_friction_kinetic;
    }

    public static Vector2 GenerateForce_Drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // f = (p * v^2 * area * coeff) / 2
        Vector2 velocity = particleVelocity - fluidVelocity;

        return 0.5f * objectDragCoefficient * fluidDensity * Vector2.Scale(velocity, velocity) * objectArea_crossSection;
    }

    public static Vector2 GenerateForce_Spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // f = -coeff*(spring length - spring resting length)
        Vector2 springLengthVector = particlePosition - anchorPosition;
        Vector2 springRestingLengthVector = new Vector2(0.0f, springRestingLength);
        Vector2 f_spring = -springStiffnessCoefficient * (springLengthVector - springRestingLengthVector);

        return f_spring;
    }
}

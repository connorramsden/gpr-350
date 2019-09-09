using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator : MonoBehaviour
{
    // Must pass negative gravitationalConstant
    public static Vector2 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        // f = mg
        Vector2 fGravity = particleMass * gravitationalConstant * worldUp;
        return fGravity;
    }

    public static Vector2 GenerateForce_Normal()
    {

    }

    public static Vector2 GenerateForce_Sliding()
    {

    }

    public static Vector2 GenerateForce_Friction_Static()
    {

    }

    public static Vector2 GenerateForce_Friction_Kinetic()
    {

    }

    public static Vector2 GenerateForce_Drag()
    {

    }

    public static Vector2 GenerateForce_Spring()
    {

    }
}

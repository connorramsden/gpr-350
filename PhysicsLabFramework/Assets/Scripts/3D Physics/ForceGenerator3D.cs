using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ForceGenerator3D 
{
    const float GRAVITY = 9.8f;

    public static Vector3 GF_Gravity(float partMass, Vector3 worldUp)
    {
        Vector3 f_gravity = partMass * -GRAVITY * worldUp;

        return f_gravity;
    }
}

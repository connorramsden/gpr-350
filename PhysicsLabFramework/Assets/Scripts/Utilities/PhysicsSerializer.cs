using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using NS_Collision_3D;
using NS_Physics3D;
using Object = UnityEngine.Object;

public static class PhysicsSerializer
{
    public static void SerializeHull(CollisionHull3D hull)
    {
        var hullType = hull.hullType;

        switch (hullType)
        {
            case CollisionHull3D.CollisionHullType3D.HULL_SPHERE:
            {
                SphereCollisionHull tempHull = hull as SphereCollisionHull;;
                string hullData = JsonUtility.ToJson(tempHull);
                break;
            }
            case CollisionHull3D.CollisionHullType3D.HULL_AABB_3D:
            {
                AABBCollisionHull3D tempHull = hull as AABBCollisionHull3D;
                string hullData = JsonUtility.ToJson(tempHull);
                
                SaveJson(tempHull, hullData);
                RustPlugin.deserialize_hull();
                
                break;
            }
            case CollisionHull3D.CollisionHullType3D.HULL_OBB_3D:
            {
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void SaveJson(Object obj, string objData)
    {
        using (var writer = new BinaryWriter(File.OpenWrite($"{obj.name}.json")))
        {
            writer.Write(objData);
            writer.Close();
        }
    }
}

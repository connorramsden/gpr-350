using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using NS_Collision_3D;

public static class PhysicsSerializer
{
    public static void SerializeHull(CollisionHull3D hull)
    {
        var hullType = hull.hullType;

        string hullData;

        switch (hullType)
        {
            case CollisionHull3D.CollisionHullType3D.HULL_SPHERE:
            {
                SphereCollisionHull tempHull = hull as SphereCollisionHull;
                ;
                hullData = JsonUtility.ToJson(tempHull);
                break;
            }
            case CollisionHull3D.CollisionHullType3D.HULL_AABB_3D:
            {
                AABBCollisionHull3D tempHull = hull as AABBCollisionHull3D;
                hullData = JsonUtility.ToJson(tempHull);

                break;
            }
            case CollisionHull3D.CollisionHullType3D.HULL_OBB_3D:
            {
                hullData = "";
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }

        string hullName = hull.name;

        var t = Task.Run(() => SaveJson(hullName, hullData));

        t.Wait();
    }

    private static void SaveJson(string name, string objData)
    {
        var writer = new BinaryWriter(File.OpenWrite($"{name}.json"));
        writer.Write(objData);
        writer.Close();
    }
}
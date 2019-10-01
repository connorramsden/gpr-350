using UnityEngine;

[RequireComponent(typeof(Particle2DComponent))]
public abstract class CollisionHull2D : MonoBehaviour
{
    // All collision types must be enumerators in this set
    public enum CollisionHullType2D
    {
        INVALID_TYPE = -1,
        HULL_CIRCLE,
        HULL_AABB,
        HULL_OBB,
    }

    // Public getter, private setter
    public CollisionHullType2D type
    {
        get; private set;
    }

    protected void SetType(CollisionHullType2D newType)
    {
        type = newType;
    }

    protected Particle2DComponent particle;

    // Architecture Style 1 //
    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b)
    {
        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other);

    public abstract bool TestCollisionVsAABB(AABBCollisionHull2D other);

    public abstract bool TestCollisionVsOBB(OBBCollisionHull2D other);

    public abstract void UpdateCenterPos();
    public abstract void UpdateExtents();

    private void Awake()
    {
        particle = gameObject.GetComponent<Particle2DComponent>();
    }
}

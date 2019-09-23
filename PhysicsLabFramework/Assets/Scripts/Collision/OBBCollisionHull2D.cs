// Object-Bounding-Box Collision Hull for 2D Space
public class OBBCollisionHull2D : CollisionHull2D
{
    public OBBCollisionHull2D(CollisionHullType2D newType) : base(CollisionHullType2D.HULL_OBB)
    {

    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        /// <see cref="CircleCollisionHull2D.TestCollisionVsCircle(CircleCollisionHull2D)"/>

        return false;
    }

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other)
    {
        /// <see cref="AABBCollisionHull2D.TestCollisionVsAABB(AABBCollisionHull2D)"/>

        return false;
    }

    public override bool TestCollisionVsOBB(OBBCollisionHull2D other)
    {
        // same as AABB-OBB part 2, twice

        return false;
    }
}

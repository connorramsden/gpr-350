// Axis-Aligned-Bounding-Box Collision Hull for 2D Space
// Same Axis' as World-Axis
public class AABBCollisionHull2D : CollisionHull2D
{
    public AABBCollisionHull2D(CollisionHullType2D newType) : base(CollisionHullType2D.HULL_AABB)
    {
        
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        /// <see cref="CircleCollisionHull2D.TestCollisionVsCircle(CircleCollisionHull2D)"/>

        return false;
    }

    public override bool TestCollisionVsAABB(AABBCollisionHull2D other)
    {
        // pass if, for all axes, max extent of A is greather than min extent of B
        // Step 01: 

        return false;
    }

    public override bool TestCollisionVsOBB(OBBCollisionHull2D other)
    {
        // same as above twice:
        // first, find max extents of OBB, do AABB vs this box
        // then, transform this box into OBB's space, find max extents, repeat
        // Step 01:

        return false;
    }
}

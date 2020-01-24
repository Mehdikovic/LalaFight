using UnityEngine;

public static class TransformExtension
{
    public static Vector3 Flat(this Transform transform)
    {
        return transform.position.Flat();
    }

    public static Vector3 DirectionTo(this Transform from, Transform to)
    {
        return to.position.DirectionTo(to.position);
    }

    public static float sqrLengthTo(this Transform from, Transform to)
    {
        return from.position.sqrLengthTo(to.position);
    }
}

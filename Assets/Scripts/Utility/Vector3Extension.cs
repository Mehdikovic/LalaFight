using UnityEngine;

namespace LalaFight
{
    public static class Vector3Extension
    {
        public static Vector3 WithX(this Vector3 position, float x)
        {
            return new Vector3(x, position.y, position.z);
        }

        public static Vector3 WithY(this Vector3 position, float y)
        {
            return new Vector3(position.x, y, position.z);
        }

        public static Vector3 WithZ(this Vector3 position, float z)
        {
            return new Vector3(position.x, position.y, z);
        }

        public static Vector3 Flat(this Vector3 position)
        {
            return new Vector3(position.x, 0f, position.z);
        }

        public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
        {
            return (to - from).normalized;
        }

        public static float sqrLengthTo(this Vector3 fromPosition, Vector3 toPosition)
        {
            return (fromPosition - toPosition).sqrMagnitude;
        }
    }
}
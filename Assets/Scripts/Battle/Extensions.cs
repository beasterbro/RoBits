using UnityEngine;

namespace Extensions
{

    public static class VectorHelper
    {

        public static Vector3 MakeVector(float magnitude, float angle, float z)
        {
            var x = Mathf.Cos(angle * Mathf.PI / 180f) * magnitude;
            var y = Mathf.Sin(angle * Mathf.PI / 180f) * magnitude;
            return new Vector3(x, y, z);
        }

        public static Vector2 MakeVector(float magnitude, float angle)
        {
            var x = Mathf.Cos(angle * Mathf.PI / 180f) * magnitude;
            var y = Mathf.Sin(angle * Mathf.PI / 180f) * magnitude;
            return new Vector2(x, y);
        }

    }

    public static class TransformExtensions
    {

        public static void SetZ(this Transform transform, float newZ)
        {
            Vector3 position = transform.position;
            position.z = newZ;
            transform.position = position;
        }

        public static void CopyPositionAndRotation(this Transform self, Transform other)
        {
            self.SetPositionAndRotation(other.position, other.rotation);
        }

        public static void CopyLocal(this Transform transform, Transform from)
        {
            transform.localPosition = from.localPosition;
            transform.localRotation = from.localRotation;
            transform.localScale = from.localScale;
        }

    }

}
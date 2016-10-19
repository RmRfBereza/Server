using UnityEngine;


public class GeometryBasic {

    public const float RightAngleDeg = 90f;

    public static void RotateCs(Vector3 point, out Vector3 resultPoint, float angleDeg)
    {
        resultPoint.y = 0;
        resultPoint.x = point.x*Mathf.Cos(angleDeg*Mathf.Deg2Rad) +
                    point.z*Mathf.Sin(angleDeg*Mathf.Deg2Rad);
        resultPoint.z = -point.x*Mathf.Sin(angleDeg*Mathf.Deg2Rad) +
                    point.z*Mathf.Cos(angleDeg*Mathf.Deg2Rad);
    }
}

using UnityEngine;

public class scr_Math : MonoBehaviour {

    public static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.right, diference) * sign;
        if (angle < 0) { angle += 360; }
        return angle;
    }

    public static float AngleDiference(float ang1, float ang2)
    {
        if (ang1 > 180)
            ang1 -= 180;
        if (ang2 > 180)
            ang2 -= 180;

        return Mathf.Max(ang1, ang2) - Mathf.Min(ang1, ang2);
    }

    public static int GetSignAngles(float angle1, float angle2)
    {
        int _sign = 1;

        if (angle2 > angle1)
        {
            angle1 += 180;
            if (angle1 > 360)
            {
                angle1 -= 360;
                if (angle2 < angle1) { _sign = 1; }
            }
            else if (angle2 > angle1) { _sign = -1; }
        }
        else
        {
            _sign = -1;
            angle1 -= 180;
            if (angle1 < 0)
            {
                angle1 += 360;
                if (angle1 < angle2) { _sign = -1; }
            }
            else if (angle1 > angle2) { _sign = 1; }
        }

        return _sign;

    }
}

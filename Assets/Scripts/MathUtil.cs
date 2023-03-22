using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil
{
    public static Vector2 Vec3ToVec2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.z);
    }

    public static Vector3 Vec2ToVec3(Vector2 vec2, float? upValue = null)
    {
        if(upValue.HasValue)
        {
            return new Vector3(vec2.x, upValue.Value, vec2.y);
        }

        return new Vector3(vec2.x, 0.0f, vec2.y);
    }

    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static Vector2 Normalize(Vector2 vec)
    {
        Vector2 normalizedVec;

        var magnitude = vec.magnitude;
        normalizedVec.x = vec.x / magnitude;
        normalizedVec.y = vec.y / magnitude;

        return normalizedVec;
    }
}

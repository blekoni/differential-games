using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtil
{
    static List<GameObject> debugObjects = new List<GameObject>();

    public static void DrawPoint(Vector2 position, Color color, float? upValue, float? duration = null)
    {
        DrawLine(MathUtil.Vec2ToVec3(position - new Vector2(0.1f, 0.0f), upValue),
            MathUtil.Vec2ToVec3(position + new Vector2(0.1f, 0.0f), upValue), color, duration);
    }

    public static void DrawPoint(Vector3 position, Color color, float? duration = null)
    {
        DrawLine(position - new Vector3(0.1f, 0.0f), position + new Vector3(0.1f, 0.0f), color, duration);
    }

    public static void DrawLine(Vector2 start, Vector2 end, Color color, float? upValue, float? duration = null)
    {
        DrawLine(MathUtil.Vec2ToVec3(start, upValue), MathUtil.Vec2ToVec3(end, upValue), color, duration);
    }

    public static void DrawLineEx(Vector3 start, Vector3 end, Color color, float? duration = null)
    {
        GameObject line = new GameObject();
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Transparent/Diffuse"));
        lr.material.color = color;
        lr.SetColors(color, color);
        lr.SetWidth(0.2f, 0.2f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        debugObjects.Add(line);

        if (duration.HasValue)
        {
            GameObject.Destroy(line, duration.Value);
        }
        else
        {
            debugObjects.Add(line);
        }
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float? duration = null)
    {
        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        MeshRenderer lr = line.GetComponent<MeshRenderer>();
        lr.material = new Material(Shader.Find("Transparent/Diffuse"));
        lr.material.color = color;

        Vector3 target = (end - start).normalized;


        Rotate(line, Vector3.up, target);

        Vector3 pos = (start + end) * 0.5f;
        line.transform.localScale = new Vector3(0.1f, Vector3.Distance(start, end) * 0.5f, 0.1f);
        line.transform.position = new Vector3(pos.x, 0.5f, pos.z);

        if (duration.HasValue)
        {
            GameObject.Destroy(line, duration.Value);
        }
        else
        {
            debugObjects.Add(line);
        }
    }

    public static void DrawPolyline(List<Vector2> polyline, Color color, float? duration = null)
    {
        if(polyline.Count == 0)
        {
            return;
        }

        for(var i = 1; i <= polyline.Count; ++i)
        {
            var p1 = polyline[i - 1];
            var p2 = polyline[i % polyline.Count];

            DrawLine(MathUtil.Vec2ToVec3(p1, null), MathUtil.Vec2ToVec3(p2, null), color, duration);
        }
    }

    public static void Clean()
    {
        foreach (var debugObject in debugObjects)
        {
            GameObject.Destroy(debugObject);
        }

        debugObjects.Clear();
    }

    private static void Rotate(GameObject gameObject, Vector3 from, Vector3 to)
    {
        var angle = Vector3.Angle(from, to);
        var crossProduct = Vector3.Cross(from, to).normalized;

        float sign = crossProduct.y > 0.0f ? 1.0f : -1.0f;

        gameObject.transform.Rotate(crossProduct, angle * sign);

    }
}

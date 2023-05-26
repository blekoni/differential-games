using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFromArea : Behavior
{
    Vector2 m_dir = new Vector2(0.0f, 0.0f);
    Vector2 m_nearestPt = new Vector2(0.0f, 0.0f);

    public EscapeFromArea(Vector2 currentPos)
    {
        var gameAreaPolygon = GameManager.Get().GetGameAreaPolygon();

        float minDistance = float.MaxValue;
        for (int i = 1; i <= gameAreaPolygon.Count; ++i)
        {
            var p1 = gameAreaPolygon[i - 1];
            var p2 = gameAreaPolygon[i % gameAreaPolygon.Count];
            var currentPt = FindNearestPointOnLine(p1, p2 - p1, currentPos);
            var distance = Vector2.Distance(currentPos, currentPt);
            if (distance < minDistance)
            {
                minDistance = distance;
                m_nearestPt = currentPt;
            }
        }

        m_dir = (m_nearestPt - currentPos).normalized;

        m_behaviorType = BehaviorType.EscapeFromArea;
    }

    public override Vector2 GetNextStepDirection(Vector2 currentPos, Vector2 currentDir)
    {
        return m_dir;
    }

    public Vector2 FindNearestPointOnLine(Vector2 origin, Vector2 direction, Vector2 point)
    {
        direction.Normalize();
        Vector2 lhs = point - origin;

        float dotP = Vector2.Dot(lhs, direction);
        return origin + direction * dotP;
    }

}

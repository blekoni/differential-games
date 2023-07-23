using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFromArea : Behavior
{
    Vector2 m_dir = new Vector2(0.0f, 0.0f);
    Vector2 m_nearestPt = new Vector2(0.0f, 0.0f);

    public EscapeFromArea(Vector2 currentPos)
    {
        m_behaviorType = BehaviorType.EscapeFromArea;
        var pickedBounds = GridManager.Get().GetPickedBounds();
        if(!pickedBounds.HasValue)
        {
            return;
        }

        var closestPt = pickedBounds.Value.ClosestPoint(MathUtil.Vec2ToVec3(currentPos));
        m_nearestPt = MathUtil.Vec3ToVec2(closestPt);
        m_dir = (m_nearestPt - currentPos).normalized;
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

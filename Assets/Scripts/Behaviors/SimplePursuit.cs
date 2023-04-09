using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePursuit : Behavior
{
    public SimplePursuit()
    {
        m_behaviorType = BehaviorType.SimplePursuit;
    }

    public override Vector2 GetNextStepDirection(Vector2 currentPos, Vector2 currentDir)
    {
        Player closestEscaper = GetClosestEnemy(GameManager.PlayerRole.Escaper, currentPos, true);
        if (!closestEscaper)
        {
            return new Vector2(0.0f, 0.0f);
        }

        //DebugUtil.DrawLine(MathUtil.Vec2ToVec3(currentPos, true), closestEscaper.transform.position, Color.red);

        Vector2 nextStepDir = (MathUtil.Vec3ToVec2(closestEscaper.transform.position) - currentPos).normalized;
        return nextStepDir;
    }
}

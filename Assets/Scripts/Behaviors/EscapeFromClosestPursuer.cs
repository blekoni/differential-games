using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFromClosestPursuer : Behavior
{
    public EscapeFromClosestPursuer()
    {
        m_behaviorType = BehaviorType.EscapeFromClosestPursuer;
    }

    public override Vector2 GetNextStepDirection(Vector2 currentPos, Vector2 currentDir)
    {
        Player closestPursuer = GetClosestEnemy(GameManager.PlayerRole.Puriuer, currentPos, true);

        if (!closestPursuer)
        {
            return new Vector2(0.0f, 0.0f);
        }

        Vector2 nextStepDir = (currentPos - MathUtil.Vec3ToVec2(closestPursuer.transform.position)).normalized;
        return nextStepDir;
    }
}

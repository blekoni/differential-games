using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behavior 
{
    public enum BehaviorType
    {
        Unknown,
        SimplePursuit,
        ParallelPursuit,
        EscapeFromClosestPursuer,
        EscapeInStaticDirection,
        EscapeFromArea
    }

    protected BehaviorType m_behaviorType = BehaviorType.Unknown;

    public BehaviorType GetBehaviorType()
    {
        return m_behaviorType;
    }

    public abstract Vector2 GetNextStepDirection(Vector2 currentPos, Vector2 currentDir);

    public Player GetClosestEnemy(GameManager.PlayerRole enemyRole, Vector2 position, bool shouldBeAlive = false)
    {
        List<Player> enemies = (enemyRole == GameManager.PlayerRole.Escaper) ? GameManager.Get().GetEscapers() : GameManager.Get().GetPusuers();
        

        Player closestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var enemy in enemies)
        {
            if (!enemy)
            {
                continue;
            }

            if (shouldBeAlive && !enemy.IsAlive())
            {
                continue;
            }

            float distance = Vector3.Distance(position, MathUtil.Vec3ToVec2(enemy.transform.position));

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}

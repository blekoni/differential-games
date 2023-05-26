using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeInStaticDirection : Behavior
{
    [SerializeField] Vector2 m_direction;

    public EscapeInStaticDirection(Vector2 direction)
    {
        m_direction = direction;
        m_behaviorType = BehaviorType.EscapeInStaticDirection;
    }

    public void SetDirection(Vector2 direction)
    {
        m_direction = direction;
    }

    public Vector2 GetDirection()
    {
        return m_direction;
    }

    public override Vector2 GetNextStepDirection(Vector2 currentPos, Vector2 currentDir)
    {
        return m_direction.normalized;
    }
}

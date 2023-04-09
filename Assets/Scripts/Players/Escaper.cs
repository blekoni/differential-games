using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escaper : Player
{
    // Start is called before the first frame update
    void Start()
    {
        SetBehavior(Behavior.BehaviorType.EscapeFromClosestPursuer);
        SetIsVisible(IsVisible());
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (ShouldAliveOnCollision())
        {
            return;
        }

        if (collision.gameObject.CompareTag("Pursuer"))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (ShouldAliveOnCollision())
        {
            return;
        }

        if (other.gameObject.CompareTag("Pursuer"))
        {
            Die();
        }
    }

    public override void SetBehavior(Behavior.BehaviorType behaviorType)
    {
        if (behaviorType == Behavior.BehaviorType.EscapeFromClosestPursuer)
        {
            m_behaviorHelper = new EscapeFromClosestPursuer();
        }
        else if (behaviorType == Behavior.BehaviorType.EscapeFromArea)
        {
            m_behaviorHelper = new EscapeFromArea(MathUtil.Vec3ToVec2(transform.position));
        }
        else if (behaviorType == Behavior.BehaviorType.EscapeInStaticDirection)
        {
            m_behaviorHelper = new EscapeInStaticDirection(new Vector2(1.0f, 0.0f));
        }
    }
}

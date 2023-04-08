using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escaper : Player
{
    enum Behaviour
    {
        EscapeFromClosestPursuer,
        EscapeInStaticDirection,
        EscapeFromArea
    }

    [SerializeField] Behaviour m_behaviour = Behaviour.EscapeFromClosestPursuer;
    [SerializeField] Vector2 m_staticDirection = new Vector2(1.0f,0.0f);

    bool m_bflag = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_behaviour == Behaviour.EscapeFromClosestPursuer)
        {
            m_behaviorHelper = new EscapeFromClosestPursuer();
        }
        else if(m_behaviour == Behaviour.EscapeFromArea)
        {
            m_behaviorHelper = new EscapeFromArea(MathUtil.Vec3ToVec2(transform.position));
        }
        else if(m_behaviour == Behaviour.EscapeInStaticDirection)
        {
            m_behaviorHelper = new EscapeInStaticDirection(m_staticDirection);
        }

        //m_moveDirection = MathUtil.Vec2ToVec3(m_behaviorHelper.GetNextStepDirection(GetPosition(), GetDirection()));
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

    public override int GetBehavior()
    {
        return (int)m_behaviour;
    }

    public override void SetBehavior(int behavior)
    {
        if (behavior < 0)
        {
            return;
        }

        if (behavior > 2)
        {
            return;
        }

        m_behaviour = (Behaviour)behavior;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuer : Player
{
    // Start is called before the first frame update
    void Start()
    {
        SetBehavior(Behavior.BehaviorType.SimplePursuit);

        SetIsVisible(IsVisible());
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (ShouldAliveOnCollision())
        {
            return;
        }

        if (GameManager.Get().GetGameStatus() != GameManager.GameStatus.InProgress)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Escaper"))
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

        if (GameManager.Get().GetGameStatus() != GameManager.GameStatus.InProgress)
        {
            return;
        }

        if (other.gameObject.CompareTag("Escaper"))
        {
            Die();
        }
    }

    public override void SetBehavior(Behavior.BehaviorType behaviorType)
    {
        if (behaviorType == Behavior.BehaviorType.SimplePursuit)
        {
            m_behaviorHelper = new SimplePursuit();
        }
        else if (behaviorType == Behavior.BehaviorType.ParallelPursuit)
        {
            m_behaviorHelper = new ParallelPursuit();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuer : Player
{
    enum Behaviour
    {
        SimplePursuit,
        ParallelPursuit
    }

    [SerializeField] Behaviour m_behaviour = Behaviour.SimplePursuit;

    bool m_bflag = false;

    // Start is called before the first frame update
    void Start()
    {
        if(m_behaviour == Behaviour.SimplePursuit)
        {
            m_behaviorHelper = new SimplePursuit();
        }
        else if (m_behaviour == Behaviour.ParallelPursuit)
        {
            m_behaviorHelper = new ParallelPursuit();
        }

        SetIsVisible(IsVisible());
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (ShouldAliveOnCollision())
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

        if (other.gameObject.CompareTag("Escaper"))
        {
            Die();
        }
    }
}

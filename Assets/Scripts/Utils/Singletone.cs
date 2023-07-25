using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletoneObject<T> : MonoBehaviour
{
    protected static T m_instance;

    public static T Get()
    {
        return m_instance;
    }

    public abstract void Awake();
}

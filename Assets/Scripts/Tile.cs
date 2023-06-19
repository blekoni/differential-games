using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color m_baseColor, m_offsetColor;
    [SerializeField] private SpriteRenderer m_renderer;

    public void Init(bool isOffset)
    {
        if (m_renderer)
        {
            m_renderer.color = isOffset ? m_offsetColor : m_baseColor;
        }
    }
}

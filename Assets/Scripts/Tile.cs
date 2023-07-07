using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color m_baseColor, m_offsetColor;
    [SerializeField] private SpriteRenderer m_renderer;
    bool m_bIsOffset = false;

    public void Init(bool isOffset)
    {
        m_bIsOffset = isOffset;
        SetColor();
    }

    private void SetColor()
    {
        if (m_renderer)
        {
            m_renderer.color = m_bIsOffset ? m_offsetColor : m_baseColor;
        }
    }

    private void OnMouseEnter()
    {
        m_renderer.color = Color.red;
    }

    private void OnMouseExit()
    {
        SetColor();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct BBox
{
    public Vector2 minPos;
    public Vector2 maxPos;
}

public class Tile : MonoBehaviour
{
    [SerializeField] private Color m_baseColor, m_offsetColor;
    [SerializeField] private SpriteRenderer m_renderer;

    BBox m_bbox;
    
    bool m_bIsPicked = false;
    bool m_bIsOffset = false;

    GridManager m_gridManager = null;

    public void Init(GridManager gridManager, bool isOffset, Vector2 position)
    {
        m_gridManager = gridManager;
        m_bbox.minPos = position;
        m_bbox.maxPos = position + new Vector2(2.0f, 2.0f);
        m_bIsOffset = isOffset;
        SetColor(DefaultColor());
    }

    public void DeSelect()
    {
        m_bIsPicked = false;

        SetColor(DefaultColor());
    }

    public Vector2 Position()
    {
        return m_bbox.minPos;
    }

    private void SetColor(Color color)
    {
        if (m_renderer)
        {
            m_renderer.color = color;
        }
    }

    private void OnMouseEnter()
    {
        m_renderer.color = Color.blue;
    }

    private void OnMouseExit()
    {
        SetActiveColor();
    }

    private void OnMouseDown()
    {
        if(GameManager.Get().GetGameType() != GameManager.GameType.UntilOutOfZone)
        {
            return;
        }

        m_bIsPicked = !m_bIsPicked;
        m_renderer.color = Color.red;
        m_gridManager.AddPickedTile(this);
    }

    private Color DefaultColor()
    {
        return m_bIsOffset ? m_offsetColor : m_baseColor;
    }

    public void SetActiveColor()
    {
        SetColor(m_bIsPicked ? Color.red : DefaultColor());
    }

    public void SetDefaultColor()
    {
        SetColor(DefaultColor());
    }
}

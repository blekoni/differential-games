using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color m_baseColor, m_offsetColor;
    [SerializeField] private SpriteRenderer m_renderer;

    Bounds m_bbox;
    
    bool m_bIsPicked = false;
    bool m_bIsOffset = false;

    GridManager m_gridManager = null;

    public void Init(GridManager gridManager, bool isOffset, Vector2 position)
    {
        m_gridManager = gridManager;
        m_bbox.min = position;
        m_bbox.max = position + new Vector2(2.0f, 2.0f);
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
        return m_bbox.min;
    }

    public Bounds GetBounds()
    {
        return m_bbox;
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
        if (GameManager.Get().GetGameStatus() != GameManager.GameStatus.NotStarted)
        {
            return;
        }

        if(GameManager.Get().GetGameType() != GameManager.GameType.EscapeToSafeZone)
        {
            return;
        }

        m_renderer.color = Color.blue;
    }

    private void OnMouseExit()
    {
        SetActiveColor();
    }

    private void OnMouseDown()
    {
        if(GameManager.Get().GetGameType() != GameManager.GameType.EscapeToSafeZone)
        {
            return;
        }

        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (GameManager.Get().GetGameStatus() == GameManager.GameStatus.InProgress)
        {
            return;
        }

        m_bIsPicked = !m_bIsPicked;

        if (m_bIsPicked)
        {
            m_gridManager.AddPickedTile(this);
        }
        else
        {
            m_gridManager.RemovedPickedTile(this);
        }
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

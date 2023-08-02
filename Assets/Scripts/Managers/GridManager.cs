using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int m_width, m_height;

    [SerializeField] private Tile m_tilePrefab;
    [SerializeField] private Tile m_xAxisPrefab;
    [SerializeField] private Tile m_yAxisPrefab;

    List<Tile> m_pickedTiles = new List<Tile>();

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Quaternion quaternion = new Quaternion();
        quaternion.SetLookRotation(new Vector3(0, 1, 0));

        for (int x = -m_width; x < m_width; x+=2)
        {
            for (int y = -m_height; y < m_height; y+=2)
            {

                Tile gridTile = null;

                // X axis
                if(y == 0)
                {
                    Tile axisTile = Instantiate(m_xAxisPrefab, new Vector3(x, 0.22f, y), quaternion);
                }

                // Y axis
                if (x == 0)
                {
                    Tile axisTile = Instantiate(m_yAxisPrefab, new Vector3(x, 0.22f, y), quaternion);
                }

                gridTile = Instantiate(m_tilePrefab, new Vector3(x + 1.0f, 0.2f, y + 1.0f), quaternion);



                if (gridTile)
                {
                    var isOffset = (x % 4 == 0 && y % 4 != 0) || (x % 4 != 0 && y % 4 == 0);
                    gridTile.Init(this, isOffset, new Vector2(x, y));
                    gridTile.name = $"Tile {x} {y}";
                }
            }
        }
    }

    public void AddPickedTile(Tile tile)
    {
        if(PickShouldBeReset(tile.Position()))
        {
            foreach (var pickedTile in m_pickedTiles)
            {
                pickedTile.DeSelect();
            }
            m_pickedTiles.Clear();
        }

        m_pickedTiles.Add(tile);

        GameManager.Get().GetUIManager().SetStartButtonActive(true);
    }

    private bool PickShouldBeReset(Vector2 newTilePosition)
    {
        foreach(var tile in m_pickedTiles)
        {
            var pickedTilePosition = tile.Position();
            var distanceToPicked = Mathf.Abs(pickedTilePosition.x - newTilePosition.x) + Mathf.Abs(pickedTilePosition.y - newTilePosition.y);
            if (distanceToPicked <= 2.5f)
            {
                return false;
            }
        }

        return true;
    }

    public void SetActiveColorToGrid()
    {
        foreach (var pickedTile in m_pickedTiles)
        {
            pickedTile.SetActiveColor();
        }
    }

    public void SetDefaultColorToGrid()
    {
        foreach (var pickedTile in m_pickedTiles)
        {
            pickedTile.SetDefaultColor();
        }
    }

    public Bounds? GetPickedBounds()
    {
        if(m_pickedTiles.Count == 0)
        {
            return null;
        }

        Vector2 min = m_pickedTiles[0].Position();
        Vector2 max = m_pickedTiles[0].Position();
        foreach (var tile in m_pickedTiles)
        {
            var tileBounds = tile.GetBounds();
            min.x = tileBounds.min.x < min.x ? tileBounds.min.x : min.x;
            min.y = tileBounds.min.y < min.y ? tileBounds.min.y : min.y;
            max.x = tileBounds.max.x > max.x ? tileBounds.max.x : max.x;
            max.y = tileBounds.max.y > max.y ? tileBounds.max.y : max.y;
        }

        Bounds bounds = new Bounds();
        bounds.SetMinMax(MathUtil.Vec2ToVec3(min), MathUtil.Vec2ToVec3(max));
        return bounds;
    }

    public bool IsAnyPickedTiles()
    {
        return m_pickedTiles.Count > 0;
    }
}

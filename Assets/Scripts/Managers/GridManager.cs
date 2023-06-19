using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int m_width, m_height;

    [SerializeField] private Tile m_tilePrefab;
    [SerializeField] private Tile m_xAxisPrefab;
    [SerializeField] private Tile m_yAxisPrefab;


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
                    gridTile.Init(isOffset);
                    gridTile.name = $"Tile {x} {y}";
                }
            }
        }
    }
}

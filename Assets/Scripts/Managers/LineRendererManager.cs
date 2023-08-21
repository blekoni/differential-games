using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererManager : MonoBehaviour
{
    [SerializeField] LineRenderer m_mainRenderer;
    List<LineRenderer> m_lineRenders = new List<LineRenderer>();

    private static LineRendererManager m_instance;

    // Start is called before the first frame update
    void Start()
    {
        m_mainRenderer.SetWidth(0.2f, 0.2f);
        //m_mainRenderer.sharedMaterial.SetColor("_Color", Color.gray);
    }

    public void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        m_mainRenderer.positionCount = 2;
        m_mainRenderer.SetPosition(0, startPos);
        m_mainRenderer.SetPosition(1, endPos);
    }

    public void Awake()
    {
        m_instance = this;
    }

    public static LineRendererManager Get()
    {
        return m_instance;
    }

    private LineRenderer InitiateRenderer(float width, Color color)
    {
        LineRenderer lineRenderer = LineRenderer.Instantiate(m_mainRenderer);
        lineRenderer.SetColors(color, color);
        lineRenderer.SetWidth(width, width);
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.sharedMaterial.SetColor("_Color", color);
        m_lineRenders.Add(lineRenderer);
        return lineRenderer;
    }

    public void DrawLine(List<Vector2> positions, Color color)
    {
        LineRenderer lineRenderer = InitiateRenderer(2.3f, color);
        lineRenderer.positionCount = positions.Count;
        for(int i = 0; i < positions.Count; ++i)
        {
            lineRenderer.SetPosition(i, MathUtil.Vec2ToVec3(positions[i], 1.0f));
        }
    }    

    public void Clear()
    {
        foreach (var lineRenderer in m_lineRenders)
        {
            Destroy(lineRenderer.gameObject);
        }
        m_lineRenders.Clear();
    }
}

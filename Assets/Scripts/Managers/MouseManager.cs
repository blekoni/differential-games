using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    GameObject m_selectedObject;
    List<Color> m_oldColors = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        ClearSelection();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Input.GetMouseButtonUp(0))
        {
            return;
        }

        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;


        if(Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.transform.gameObject;
            if(!hitObject)
            {
                return;
            }

            if(hitObject.CompareTag("Pursuer") || hitObject.CompareTag("Escaper"))
            {
                SelectObject(hitObject);
            }
            else
            {
                ClearSelection();
            }
        }
        else
        {
            ClearSelection();
        }
    }

    void SelectObject(GameObject obj)
    {
        if(!obj)
        {
            return;
        }

        if(m_selectedObject != null)
        {
            if(obj == m_selectedObject)
            {
                ClearSelection();
                return;
            }

            ClearSelection();
        }

        m_selectedObject = obj;

        PlayerUI.Get().Show(obj);
        HighlightObject(m_selectedObject);
    }

    public void ClearSelection()
    {
        ClearHighlightObject(m_selectedObject);
        m_selectedObject = null;

        PlayerUI.Get().Hide();
    }

    void HighlightObject(GameObject gameObject)
    {
        if (!gameObject)
        {
            return;
        }

        Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            Material m = r.material;
            m_oldColors.Add(m.color);
            m.color = Color.red;
            r.material = m;
        }
    }

    void ClearHighlightObject(GameObject gameObject)
    {
        if (!gameObject)
        {
            return;
        }

        Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();
        int i = 0;
        foreach (Renderer r in rs)
        {
            Material m = r.material;
            m.color = m_oldColors[i];
            r.material = m;
        }

        m_oldColors.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] List<Camera> m_cameras = new List<Camera>();

    int m_activeCamera = 0;
    float m_cameraSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        SetFrontView();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            SetFrontView();
        }

        if (Input.GetKey("e"))
        {
            SetTopView();
        }

        if (Input.GetKey("w"))
        {
            MoveUp();
        }

        if (Input.GetKey("s"))
        {
            MoveDown();
        }

        if (Input.GetKey("a"))
        {
            MoveLeft();
        }

        if (Input.GetKey("d"))
        {
            MoveRight();
        }

        if (Input.GetKey("z"))
        {
            ZoomIn();
        }

        if (Input.GetKey("x"))
        {
            ZoomOut();
        }
    }

    void SetFrontView()
    {
        SetActiveCamera(0);
    }

    void SetTopView()
    {
        SetActiveCamera(1);
    }

    void ZoomIn()
    {
        var activeCamera = GetActiveCamera();
        if(activeCamera == null)
        {
            return;
        }

        var moveDir = activeCamera.transform.forward;
        activeCamera.transform.position += moveDir * m_cameraSpeed * Time.deltaTime;
    }

    void ZoomOut()
    {
        var activeCamera = GetActiveCamera();
        if (activeCamera == null)
        {
            return;
        }

        var moveDir = -activeCamera.transform.forward;
        activeCamera.transform.position += moveDir * m_cameraSpeed * Time.deltaTime;
    }

    void MoveUp()
    {
        var activeCamera = GetActiveCamera();
        if (activeCamera == null)
        {
            return;
        }

        var moveDir = activeCamera.transform.up;
        activeCamera.transform.position += moveDir * m_cameraSpeed * Time.deltaTime;
    }

    void MoveDown()
    {
        var activeCamera = GetActiveCamera();
        if (activeCamera == null)
        {
            return;
        }

        var moveDir = -activeCamera.transform.up;
        activeCamera.transform.position += moveDir * m_cameraSpeed * Time.deltaTime;
    }

    void MoveRight()
    {
        var activeCamera = GetActiveCamera();
        if (activeCamera == null)
        {
            return;
        }

        var moveDir = activeCamera.transform.right;
        activeCamera.transform.position += moveDir * m_cameraSpeed * Time.deltaTime;
    }

    void MoveLeft()
    {
        var activeCamera = GetActiveCamera();
        if (activeCamera == null)
        {
            return;
        }

        var moveDir = -activeCamera.transform.right;
        activeCamera.transform.position += moveDir * m_cameraSpeed * Time.deltaTime;
    }

    public Camera GetActiveCamera()
    {
        if(m_activeCamera >= m_cameras.Capacity)
        {
            return null;
        }

        return m_cameras[m_activeCamera];
    }

    void SetActiveCamera(int index)
    {
        for(var i = 0; i < m_cameras.Capacity; i++)
        {
            var camera = m_cameras[i];
            if (camera != null)
            {
                camera.gameObject.SetActive(i == index);
            }
        }

        m_activeCamera = index;
    }

    public Vector3 GetPosition()
    {
        var activeCamera = GetActiveCamera();
        if(activeCamera)
        {
            return activeCamera.transform.position;
        }

        return new Vector3();
    }
}

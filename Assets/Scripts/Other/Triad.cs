using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var cameraManager = GameManager.Get().GetCameraManager();
        if(!cameraManager)
        {
            return;
        }

        var activeCamera = cameraManager.GetActiveCamera();
        if (!activeCamera)
        {
            return;
        }

        transform.position = activeCamera.ScreenToWorldPoint(new Vector3(50.0f, 50.0f, 5.0f));
        //transform.position = GameManager.Get().GetCameraManager().GetPosition() + new Vector3(-5.0f, 2.0f, 0.0f);
    }
}

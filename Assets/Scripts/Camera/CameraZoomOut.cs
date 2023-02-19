using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField]
    private float cameraZoomOutSize = 12f;
    [SerializeField]
    private float zoomOutSpeed = 0.5f;

    CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        // Get the current orthographic size of the virtual camera
        float currentOrthographicSize = virtualCamera.m_Lens.OrthographicSize;

        // Calculate the new orthographic size based on the zoom speed
        float newOrthographicSize = currentOrthographicSize + zoomOutSpeed;

        // Set the new orthographic size of the virtual camera
        virtualCamera.m_Lens.OrthographicSize = newOrthographicSize;

        if(virtualCamera.m_Lens.OrthographicSize >= cameraZoomOutSize)
        {
            this.enabled = false;
        }
    }


}

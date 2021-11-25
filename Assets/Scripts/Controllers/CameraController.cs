using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public enum CameraZoomState { Out, Mid, In };

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera vCameraOut;
    [SerializeField] private CinemachineVirtualCamera vCameraMid;
    [SerializeField] private CinemachineVirtualCamera vCameraIn;

    [Header("Camera Settings")]
    [NonSerialized] public float zoomSizeOut = 30f;
    [NonSerialized] public float zoomSizeNormal = 5f;
    [NonSerialized] public float zoomNormalDuration = 1f;

    private CameraZoomState cameraZoomState = CameraZoomState.Out;
    private CameraZoomState previousZoomState = CameraZoomState.Out;
    private bool firstInputRecieved = false;

    [Header("Signals Listening On")]
    [SerializeField] private VoidSignalSO firstInputSignal; 
    [SerializeField] private VoidSignalSO restartLevelSignal; 
    [SerializeField] private CamSignalSO camStateSignal;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    private void OnEnable()
    {
        inputReader.zoomEvent += Zoom;
        firstInputSignal.OnSignalRaised += FirstInput;
        restartLevelSignal.OnSignalRaised += RestartLevel;
        camStateSignal.OnSignalRaised += SetCamState;
    }

    private void OnDisable()
    {
        inputReader.zoomEvent -= Zoom;
        firstInputSignal.OnSignalRaised -= FirstInput;
        restartLevelSignal.OnSignalRaised -= RestartLevel;
        camStateSignal.OnSignalRaised -= SetCamState;
    }

    public void FirstInput()
    {
        firstInputRecieved = true;
        SetCamState(CameraZoomState.In);
    }

    public void RestartLevel()
    {
        firstInputRecieved = false;
        SetCamState(CameraZoomState.Out);
    }

    public void Zoom()
    {
        //Debug.Log("Zoom Called");
        if (firstInputRecieved)
        {
            if (cameraZoomState == CameraZoomState.In)
            {
                previousZoomState = CameraZoomState.In;
                //Debug.Log("Was In setting to Mid");
                cameraZoomState = CameraZoomState.Mid;
            }

            else if (cameraZoomState == CameraZoomState.Mid)
            {
                if (previousZoomState == CameraZoomState.In)
                {
                    previousZoomState = CameraZoomState.Mid;
                    //Debug.Log("Was Mid setting to Out");
                    cameraZoomState = CameraZoomState.Out;
                }

                else if (previousZoomState == CameraZoomState.Out)
                {
                    previousZoomState = CameraZoomState.Mid;
                    //Debug.Log("Was Mid setting to In");
                    cameraZoomState = CameraZoomState.In;
                }
            }

            else if (cameraZoomState == CameraZoomState.Out)
            {
                previousZoomState = CameraZoomState.Out;
                //Debug.Log("Was Out setting to Mid");
                cameraZoomState = CameraZoomState.Mid;
            }

            UpdateCamera();
        }
    }

    private void UpdateCamera()
    {
        if (cameraZoomState == CameraZoomState.In)
        {
            vCameraIn.gameObject.SetActive(true);
            vCameraOut.gameObject.SetActive(false);
            vCameraMid.gameObject.SetActive(false);
        } else if (cameraZoomState == CameraZoomState.Mid)
        {
            vCameraMid.gameObject.SetActive(true);
            vCameraIn.gameObject.SetActive(false);
            vCameraOut.gameObject.SetActive(false);
        } else if (cameraZoomState == CameraZoomState.Out)
        {
            vCameraOut.gameObject.SetActive(true);
            vCameraIn.gameObject.SetActive(false);
            vCameraMid.gameObject.SetActive(false);
        }
    }

    private void SetCamState(CameraController.CameraZoomState state)
    {
        previousZoomState = cameraZoomState;
        cameraZoomState = state;
        UpdateCamera();
    }
}

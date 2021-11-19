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

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    private void OnEnable()
    {
        inputReader.zoomEvent += Zoom;
        firstInputSignal.OnSignalRaised += FirstInput;
        restartLevelSignal.OnSignalRaised += RestartLevel;
    }

    private void OnDisable()
    {
        inputReader.zoomEvent -= Zoom;
        firstInputSignal.OnSignalRaised -= FirstInput;
        restartLevelSignal.OnSignalRaised -= RestartLevel;
    }

    public void FirstInput()
    {
        //animator.SetTrigger("FirstInputRecieved");
        firstInputRecieved = true;
        //isZoomedOut = false;
        previousZoomState = cameraZoomState;
        cameraZoomState = CameraZoomState.In;
        UpdateCamera();
    }

    public void RestartLevel()
    {
        firstInputRecieved = false;
        previousZoomState = cameraZoomState;
        cameraZoomState = CameraZoomState.Out;
        UpdateCamera();
        //isZoomedOut = false;
        //animator.SetBool("isZoomedOut", false);
        //animator.SetTrigger("LevelRestart");
    }

    public void Zoom()
    {
        //Debug.Log("Zoom Called");
        if (firstInputRecieved)
        {
            //isZoomedOut = !isZoomedOut;
            //animator.SetBool("isZoomedOut", isZoomedOut);
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

    // private IEnumerator ResizeCamera()
    // {
    //     float elapsed = 0f;

    //     while (elapsed <= zoomNormalDuration)
    //     {
    //          float t = Mathf.Clamp01(elapsed / zoomNormalDuration);
    //          vCamera.m_Lens.OrthographicSize = Mathf.Lerp(zoomSizeOut, zoomSizeNormal, t);

    //          elapsed += Time.deltaTime;
    //     }

    //     yield return null;
    // }
}

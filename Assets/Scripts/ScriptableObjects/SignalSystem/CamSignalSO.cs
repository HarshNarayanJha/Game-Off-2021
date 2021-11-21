using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewCamSignal", menuName = "Signals/Cam Signal")]
public class CamSignalSO : SignalBaseSO
{
    public UnityAction<CameraController.CameraZoomState> OnSignalRaised;

    public void RaiseSignal(CameraController.CameraZoomState state)
    {
        if (OnSignalRaised != null)
            OnSignalRaised.Invoke(state);
    }
}

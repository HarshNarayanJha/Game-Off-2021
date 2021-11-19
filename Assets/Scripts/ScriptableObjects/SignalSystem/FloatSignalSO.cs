using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewFloatSignal", menuName = "Signals/Float Signal")]
public class FloatSignalSO : SignalBaseSO
{
    public UnityAction<float> OnSignalRaised;

    public void RaiseSignal(float val)
    {
        if (OnSignalRaised != null)
            OnSignalRaised.Invoke(val);
    }
}

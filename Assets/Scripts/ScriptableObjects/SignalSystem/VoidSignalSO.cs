using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewVoidSignal", menuName = "Signals/Void Signal")]
public class VoidSignalSO : SignalBaseSO
{
    public UnityAction OnSignalRaised;

    public void RaiseSignal()
    {
        if (OnSignalRaised != null)
            OnSignalRaised.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewStringSignal", menuName = "Signals/String Signal")]
public class StringSignalSO : SignalBaseSO
{
    public UnityAction<string> OnSignalRaised;

    public void RaiseSignal(string val)
    {
        if (OnSignalRaised != null)
            OnSignalRaised.Invoke(val);
    }
}

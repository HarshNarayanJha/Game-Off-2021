using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewIntSignal", menuName = "Signals/Int Signal")]
public class IntSignalSO : SignalBaseSO
{
    public UnityAction<int> OnSignalRaised;

    public void RaiseSignal(int val)
    {
        if (OnSignalRaised != null)
            OnSignalRaised.Invoke(val);
    }
}

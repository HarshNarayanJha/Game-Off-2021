using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoidSignalListener : MonoBehaviour
{
    [Header("Signal")]
    [SerializeField] private VoidSignalSO signal;

    public UnityEvent signalEvent;

    private void OnEnable()
    {
        signal.OnSignalRaised += OnSignalRaised;
    }

    private void OnDisable()
    {
        signal.OnSignalRaised -= OnSignalRaised;
    }

    private void OnSignalRaised()
    {
        signalEvent.Invoke();
    }
}

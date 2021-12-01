using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidSignalRaiser : MonoBehaviour
{
    [SerializeField] private VoidSignalSO signal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            signal.RaiseSignal();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO restartLevelSignal;
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO fadedOutSignal;

    [SerializeField] private InputReader inputReader;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inputReader.DisablePlayerInput();
            fadeInSignal.RaiseSignal();

            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.ResetVelocity();
            playerController.DisableColliders();

            StartCoroutine(ControlFading());
        }
    }

    private IEnumerator ControlFading()
    {
        yield return new WaitForSeconds(1f);
        restartLevelSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);
        fadedOutSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);

        inputReader.EnablePlayerInput();
    }
}

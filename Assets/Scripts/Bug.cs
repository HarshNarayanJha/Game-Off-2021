using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bug : MonoBehaviour
{
    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO fadeOutSignal;
    [SerializeField] private VoidSignalSO restartLevelSignal;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    [Header("Others")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();    
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inputReader.DisablePlayerInput();
            other.gameObject.GetComponent<PlayerController>().Kill();
            boxCollider.enabled = false;

            StartCoroutine(RestartLevel());
        }
    }

    private IEnumerator RestartLevel()
    {
        impulseSource.GenerateImpulse();
        yield return new WaitForSeconds(0.8f);

        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);
        restartLevelSignal.RaiseSignal();
        yield return new WaitForSeconds(0.1f);
        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);

        boxCollider.enabled = true;
        inputReader.EnablePlayerInput();
    }
}
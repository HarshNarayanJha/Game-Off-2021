using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraPickup : MonoBehaviour
{
    [SerializeField] private GameObject auraGauge;
    [SerializeField] private GameObject beforeAuraMessage;
    [SerializeField] private GameObject afterAuraMessage;

    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO fadeOutSignal;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))    
        {
            other.GetComponent<PlayerController>().ResetVelocity();
            inputReader.DisablePlayerInput();
            fadeInSignal.RaiseSignal();
            StartCoroutine(GetAura());
        }
    }

    private IEnumerator GetAura()
    {
        yield return new WaitForSeconds(0.5f);

        this.GetComponent<SpriteRenderer>().enabled = false;

        auraGauge.SetActive(true);
        beforeAuraMessage.SetActive(false);
        afterAuraMessage.SetActive(true);

        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);

        inputReader.EnablePlayerInput();
        
        this.gameObject.SetActive(false);
    }
}

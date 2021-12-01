using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraPickup : MonoBehaviour
{
    [SerializeField] private GameObject auraGauge;
    [SerializeField] private GameObject beforeAuraMessage;
    [SerializeField] private GameObject afterAuraMessage;

    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO fadeOutSignal;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    bool fadeOutAura;
    float fade;
    Material mat;

    private void Start()
    {
        fadeOutAura = false;
        fade = 1f;
        mat = GetComponent<SpriteRenderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))    
        {
            other.GetComponent<PlayerController>().ResetVelocity();
            inputReader.DisablePlayerInput();
            // fadeInSignal.RaiseSignal();
            StartCoroutine(GetAura());
        }
    }

    private void Update()
    {
        if (fadeOutAura)
        {
            fade -= Time.deltaTime * 5f;

            if (fade <= 0f)
                fade = 0f;

            mat.SetFloat("_Fade", fade);

            if (fade == 0f)
            {
                fadeInSignal.RaiseSignal();
                fadeOutAura = false;
            }
        }
    }

    private IEnumerator GetAura()
    {
        fadeOutAura = true;
        
        yield return new WaitWhile(GetFadeOutAura);

        // Wait for FadeIn
        yield return new WaitForSeconds(0.5f);

        auraGauge.SetActive(true);
        beforeAuraMessage.SetActive(false);
        afterAuraMessage.SetActive(true);

        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);

        inputReader.EnablePlayerInput();
        
        this.gameObject.SetActive(false);
    }

    private bool GetFadeOutAura()
    {
        return fadeOutAura;
    }
}

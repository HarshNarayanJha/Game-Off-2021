using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuraGauge : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float auraAmount;
    [SerializeField] private float auraFillRate;
    [SerializeField] private float auraReleaseRate;
    [SerializeField] private Image auraFillImage;
    [SerializeField] private PlayerAura playerAura;

    private void Update()
    {
        // if (auraAmount == 0)
        // {
        //     playerAura.SetCanUseAura(false);
        //     StartCoroutine(WaitBeforeRefill());
        // }
        // else
        //     playerAura.SetCanUseAura(true);

        if (playerAura.IsAuraActive)
        {
            if (auraAmount > 0f)
            {
                auraAmount -= Time.deltaTime * auraReleaseRate;
            }
            if (auraAmount <= 0f)
            {
                auraAmount = 0f;
                playerAura.SetCanUseAura(false);
            }
            
            auraFillImage.fillAmount = auraAmount;
        } else {

            if (auraAmount < 1f)
                auraAmount += Time.deltaTime * auraFillRate;

            if (auraAmount > 0.2f)
                playerAura.SetCanUseAura(true);

            if (auraAmount >= 1f)
            {
                auraAmount = 1f;
                playerAura.SetCanUseAura(true);
            }

            auraFillImage.fillAmount = auraAmount;
        }
    }
}

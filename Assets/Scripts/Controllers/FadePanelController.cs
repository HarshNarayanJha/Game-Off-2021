using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanelController : MonoBehaviour
{
    [Header("Signals Listening On")]
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO fadeOutSignal;

    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        fadeInSignal.OnSignalRaised += FadeIn;
        fadeOutSignal.OnSignalRaised += FadeOut;
    }

    private void OnDisable()
    {
        fadeInSignal.OnSignalRaised -= FadeIn;
        fadeOutSignal.OnSignalRaised -= FadeOut;
    }
    
    private void FadeIn()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"))
            animator.SetTrigger("FadeIn");
    }

    private void FadeOut()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut"))
            animator.SetTrigger("FadeOut");
    }
}

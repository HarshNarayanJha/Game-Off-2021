using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO fadeOutSignal;
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO skipIntroTextSignal;

    [Header("Signals Listening On")]
    [SerializeField] private VoidSignalSO introTextDone;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    [Header("Others")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject introText;
    bool isIntroDone;

    private void OnEnable()
    {
        introTextDone.OnSignalRaised += IntroTextDone;
        // inputReader.spaceEvent += SpacePressed;
    }

    private void OnDisable()
    {
        introTextDone.OnSignalRaised -= IntroTextDone;
        // inputReader.spaceEvent -= SpacePressed;
    }

    private void Start()
    {
        inputReader.DisablePlayerInput();
        fadeOutSignal.RaiseSignal();
        introText.SetActive(true);
        isIntroDone = false;
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isIntroDone)
                StartCoroutine(Intro());
            else
                skipIntroTextSignal.RaiseSignal();
        }
    }

    private void IntroTextDone()
    {
        isIntroDone = true;   
    }

    private IEnumerator Intro()
    {
        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);
        
        introPanel.SetActive(false);

        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(0.2f);

        player.SetActive(true);
        
        inputReader.EnablePlayerInput();
    }
}

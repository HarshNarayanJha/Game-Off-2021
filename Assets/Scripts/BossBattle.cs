using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [Header("Signals Listening On")]
    [SerializeField] private VoidSignalSO bugsKilledSignal;
    [SerializeField] private VoidSignalSO bossBugKilledSignal;

    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO fadeOutSignal;

    [SerializeField] private Bug[] allBugs;
    [SerializeField] private GameObject platforms;
    [SerializeField] private GameObject miniBossBug;
    [SerializeField] private GameObject bossBug;
    [SerializeField] private PlayerController player;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject beforeBossHelp;
    [SerializeField] private GameObject afterBossHelp;
    [SerializeField] private GameObject winMessage;
    [SerializeField] private GameObject portal;
    

    private void OnEnable()
    {
        bugsKilledSignal.OnSignalRaised += BugsKilled;
        bossBugKilledSignal.OnSignalRaised += BossBugKilled;
    }


    private void OnDisable()
    {
        bugsKilledSignal.OnSignalRaised -= BugsKilled;
        bossBugKilledSignal.OnSignalRaised -= BossBugKilled;
    }

    private void BugsKilled()
    {
        inputReader.DisablePlayerInput();
        for (int i = 0; i < allBugs.Length; i++)
        {
            allBugs[i].Kill();
        }

        StartCoroutine(StartBossBattle());
    }

    private IEnumerator StartBossBattle()
    {
        yield return new WaitForSeconds(1f);
        fadeInSignal.RaiseSignal();

        yield return new WaitForSeconds(0.5f);

        player.ResetPosition();
        platforms.SetActive(false);
        miniBossBug.SetActive(false);
        bossBug.SetActive(true);
        beforeBossHelp.SetActive(false);
        afterBossHelp.SetActive(true);

        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);

        inputReader.EnablePlayerInput();        
    }

    private void BossBugKilled()
    {
        afterBossHelp.SetActive(false);
        winMessage.transform.position = bossBug.transform.position;
        winMessage.SetActive(true);

        portal.SetActive(true);
    }
}

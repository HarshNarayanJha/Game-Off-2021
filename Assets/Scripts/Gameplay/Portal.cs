using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private IntValueSO playerScore;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inputReader.DisablePlayerInput();
            fadeInSignal.RaiseSignal();

            StartCoroutine(LoadScene());
        }
    }

    private IEnumerator LoadScene()
    {
        playerScore.Save();
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(level.SceneBuildIndex, LoadSceneMode.Single);

    }
}

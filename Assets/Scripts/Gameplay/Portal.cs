using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private bool loadMenu;
    [SerializeField] private MenuSO menu;
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private IntValueSO playerScore;
    [SerializeField] private Volume globalVolume;

    Bloom bloomComponent;
    bool increaseGlow;

    private void Start()
    {
        increaseGlow = false;
        globalVolume.profile.TryGet<Bloom>(out bloomComponent);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            increaseGlow = true;

            inputReader.DisablePlayerInput();

            StartCoroutine(LoadScene());
        }
    }

    private void Update()
    {
        if (increaseGlow)
        {
            bloomComponent.intensity.value += Time.deltaTime * 1000f; // to 800f;
            bloomComponent.scatter.value += Time.deltaTime * 1f; // to 1f;

            if (bloomComponent.intensity.value >= 800f)
                bloomComponent.intensity.value = 800f;

            if (bloomComponent.scatter.value >= 1f)
                bloomComponent.scatter.value = 1f;

            if ((bloomComponent.intensity.value == 800f) && (bloomComponent.scatter.value == 1f))
                increaseGlow = false;
            
        }
    }

    private IEnumerator LoadScene()
    {
        playerScore.Save();
        
        yield return new WaitForSeconds(0.6f);
        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);

        if (loadMenu)
            SceneManager.LoadSceneAsync(menu.SceneBuildIndex, LoadSceneMode.Single);
        else
            SceneManager.LoadSceneAsync(level.SceneBuildIndex, LoadSceneMode.Single);


    }
}

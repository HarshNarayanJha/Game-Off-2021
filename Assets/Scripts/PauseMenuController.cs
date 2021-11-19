using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenuObject;

    bool isPaused = false;

    private void OnEnable()
    {
        //inputReader.pauseEvent += Pause;
    }

    private void OnDisable()
    {
        //inputReader.pauseEvent -= Pause;
    }

    public void Pause()
    {
        Debug.Log("Pause Key Pressed");
        isPaused = !isPaused;
        Debug.Log(isPaused);

        if (isPaused)
        {
            pauseMenuObject.SetActive(true);
            inputReader.DisablePlayerInput();
            Time.timeScale = 0;
        } else {
            pauseMenuObject.SetActive(false);
            inputReader.EnablePlayerInput();
            Time.timeScale = 1f;
        }
    }
}

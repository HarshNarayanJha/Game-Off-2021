using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    [Header("Signals Listening On")]
    [SerializeField] private IntSignalSO scoreUpdateSignal;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO fadeInSignal;

    [Header("Others")]
    [SerializeField] private MenuSO mainMenu;

    private StringBuilder stringBuilder;
    private int currentScore;

    private void Start()
    {
        stringBuilder = new StringBuilder("000");
        UpdateScore(0);
    }

    private void OnEnable()
    {
        scoreUpdateSignal.OnSignalRaised += UpdateScore;
    }

    private void OnDisable()
    {
        scoreUpdateSignal.OnSignalRaised -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        stringBuilder.Clear();
        
        if (currentScore + score > 999)
            currentScore = 999;
        else
            currentScore += score;
        
        if (currentScore < 100)
        {
            if (currentScore > 10)
                stringBuilder.Append("0");
            else
                stringBuilder.Append("00");
        }

        stringBuilder.Append(currentScore);
        scoreText.SetText(stringBuilder.ToString());
    }

    public void ToMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(mainMenu.SceneBuildIndex);
    }
}

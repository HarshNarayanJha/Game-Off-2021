using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO fadeOutSignal;
    [SerializeField] private Animator pressStartAnimator;
    [SerializeField] private LevelSO level1;
    [SerializeField] private IntValueSO playerScore;
    [SerializeField] private TextMeshProUGUI playerScoreText;

    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject optionsMenuObject;
    [SerializeField] private GameObject helpMenuObject;


    private void OnEnable()
    {
        inputReader.spaceEvent += SpacePressed;
    }

    private void OnDisable()
    {
        inputReader.spaceEvent -= SpacePressed;
    }

    private void Start()
    {
        fadeOutSignal.RaiseSignal();
        playerScore.LoadValueFromSave();
        
        playerScoreText.SetText(PadScore(playerScore.Value));
        Application.targetFrameRate = 30;
    }

    private void SpacePressed()
    {
        StartCoroutine(GoToOptionsMenu());
    }

    private IEnumerator GoToOptionsMenu()
    {
        pressStartAnimator.SetTrigger("Started");
        yield return new WaitForSeconds(0.45f);
        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);

        mainMenuObject.SetActive(false);
        optionsMenuObject.SetActive(true);

        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator GoToHelpMenu()
    {
        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);

        optionsMenuObject.SetActive(false);
        helpMenuObject.SetActive(true);

        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator BackToOptionsMenu()
    {
        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);

        helpMenuObject.SetActive(false);
        optionsMenuObject.SetActive(true);

        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);
    }

    public void StartButton()
    {
        Debug.Log("Start the Game");
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(level1.SceneBuildIndex);
    }

    public void HelpButton()
    {
        StartCoroutine(GoToHelpMenu());
    }

    public void HelpToOptions()
    {
        //StartCoroutine(BackToOptionsMenu());
        // Or More Better, just Reload the Scene
        fadeInSignal.RaiseSignal();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private string PadScore(int score)
    {
        StringBuilder sb = new StringBuilder("000000");
        sb.Clear();

        if (score < 100000)
        {
            sb.Append(0);
            if (score < 10000)
            {
                sb.Append(0);
                if (score < 1000)
                {
                    sb.Append(0);
                    if (score < 100)
                    {
                        sb.Append(0);
                        if (score < 10)
                        {
                            sb.Append(0);
                        }
                    }
                }
            }
        }
        sb.Append(score);
        return sb.ToString();
    }
}

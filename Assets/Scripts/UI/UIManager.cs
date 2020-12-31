using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }

    }

    #region Score
    [Header("Score")]
    [SerializeField] TextMeshProUGUI collectableAmountText;
    [SerializeField] TextMeshProUGUI bestTimeScore;
    [SerializeField] TextMeshProUGUI currentTimeScore;
    #endregion
    [Header("SceneManagement")]
    [SerializeField] Button restartLevelButton;
    [SerializeField] Button quitButton;

    private void Start()
    {
        restartLevelButton.onClick.AddListener(ScenesManager.Instance.ReloadCurrentScene);
        quitButton.onClick.AddListener(ScenesManager.Instance.QuitGame);

        ScoreManager.Instance.OnScoreUpdate += UpdateCollectableAmountText;
    }

    public void UpdateCollectableAmountText(int newScore)
    {
        collectableAmountText.text = newScore.ToString();
    }

    public void SetTimeScore()
    {
        float currentScore = ScoreManager.Instance.GetScoreTime();

        //Si on a un temps en secondes de 3621

        //On va obtenir 1 heure ( 3621 / 3600 = 1)
        int hours = (int)Mathf.Floor(currentScore / 3600);
        //Il va nous rester 21 secondes (3621 % 3600 = 21)
        int remain = (int)currentScore % 3600;
        //Donc 0 minutes, car (int_floor)(21 / 60) = 0
        int minutes = (int)Mathf.Floor(remain / 60);
        //Et le reste de ce qu'il reste, 21 % 60 = 21, en secondes
        int seconds = remain % 60;

        string hoursStr = hours < 10 ? "0" + hours.ToString() : hours.ToString();
        string minutesStr = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        string secondsStr = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

        currentTimeScore.text = hoursStr + ":" + minutesStr + ":" + secondsStr;

        if (currentScore < PlayerPrefs.GetFloat("BestTimeScore"))
        {
            PlayerPrefs.SetFloat("BestTimeScore", currentScore);
            bestTimeScore.text = hoursStr + ":" + minutesStr + ":" + secondsStr;
            Debug.Log(bestTimeScore);
        }
        else
        {
            float bestScore = PlayerPrefs.GetFloat("BestTimeScore");
            hours = (int)Mathf.Floor(bestScore / 3600);
            remain = (int)bestScore % 3600;
            minutes = (int)Mathf.Floor(remain / 60);
            seconds = remain % 60;

            hoursStr = hours < 10 ? "0" + hours.ToString() : hours.ToString();
            minutesStr = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
            secondsStr = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

            bestTimeScore.text = hoursStr + ":" + minutesStr + ":" + secondsStr;
        }
    }

}

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

    [SerializeField] TextMeshProUGUI collectableAmountText;
    [SerializeField] TextMeshProUGUI bestTimeScore;
    [SerializeField] TextMeshProUGUI currentTimeScore;

    private void Start()
    {
        ScoreManager.Instance.OnScoreUpdate += UpdateCollectableAmountText;
    }

    public void UpdateCollectableAmountText(int newScore)
    {
        collectableAmountText.text = newScore.ToString();
    }

    public void SetTimeScore()
    {
        float currentScore = ScoreManager.Instance.GetScoreTime();

        int hours = (int)Mathf.Ceil(currentScore / 3600);
        int remain = (int)currentScore - hours * 3600;
        int minutes = remain % 60;
        remain = remain - minutes * 60;
        int seconds = remain % 60;

        currentTimeScore.text = hours + ":" + minutes + ":" + seconds;

        if (currentScore < PlayerPrefs.GetFloat("BestTimeScore"))
        {
            PlayerPrefs.SetFloat("BestTimeScore", currentScore);
            bestTimeScore.text = currentTimeScore.text;
        }
        else
        {
            float bestScore = PlayerPrefs.GetFloat("BestTimeScore");
            hours = (int)Mathf.Ceil(bestScore / 3600);
            remain = (int)bestScore - hours * 3600;
            minutes = remain % 60;
            remain = remain - minutes * 60;
            seconds = remain % 60;
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
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

    int collectedAmount = 0;
    float bestTime = float.MaxValue;
    float currentTime = 0;
    bool stopTime = false;

    public delegate void OnScoreUpdated(int score);
    public event OnScoreUpdated OnScoreUpdate;

    private void Start()
    {
        UpdateCollectedAmount(collectedAmount);

        if (PlayerPrefs.GetFloat("BestTimeScore") != 0)
        {
            bestTime = PlayerPrefs.GetFloat("BestTimeScore");
        }
    }

    private void Update()
    {
        if (!stopTime)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void UpdateCollectedAmount(int amount)
    {
        collectedAmount += amount;
        OnScoreUpdate(collectedAmount);
    }

    public void SetStopTime(bool toggle) => stopTime = toggle;

    public float GetScoreTime() => currentTime;
}

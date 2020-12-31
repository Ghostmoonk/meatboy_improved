using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chronometre : MonoBehaviour
{
    float chrono = 0f;
    float minutes;
    float secondes;
    float fractions;

    [SerializeField] Text chronoUI;

    float bestScore = float.MaxValue;
    string bestScoreString;

    private void Awake()
    {
        if (PlayerPrefs.GetFloat("bestScore") < bestScore)
            bestScore = PlayerPrefs.GetFloat("bestScore");

        float minutes = (int)(bestScore / 60f);
        float secondes = (int)(bestScore % 60);
        float fractions = (int)((bestScore * 100f) % 100f);
        bestScoreString = "Best : " + minutes + ":" + secondes + ":" + fractions;
    }

    void Update()
    {
        chrono += Time.deltaTime;
        minutes = (int)(chrono / 60f);
        secondes = (int)(chrono % 60);
        fractions = (int)((chrono * 100f) % 100f);

        chronoUI.text = bestScoreString + "\n" + "Temps : " + minutes + ":" + secondes + ":" + fractions;
    }

    public void End()
    {
        if (chrono < bestScore)
        {
            Debug.Log(chrono);
            PlayerPrefs.SetFloat("bestScore", chrono);
        }
        SceneManager.LoadScene("Game");
    }
}

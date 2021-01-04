using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ScenesManager : MonoBehaviour
{
    private static ScenesManager instance;
    public static ScenesManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }

    public void SetTimeScale(int newTimeScale) => Time.timeScale = newTimeScale;

    bool gamePaused = false;

    [SerializeField] UnityEvent OnGameUnPaused;
    [SerializeField] UnityEvent OnGamePaused;

    public void PauseGame(bool toggle)
    {
        gamePaused = toggle;
        if (gamePaused)
            OnGamePaused?.Invoke();
        else
            OnGameUnPaused?.Invoke();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && Input.GetButtonDown("Pause"))
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                OnGamePaused?.Invoke();
            }
            else
            {
                OnGameUnPaused?.Invoke();
            }
        }
    }
}

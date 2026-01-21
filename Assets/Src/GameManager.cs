using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;

    public static event Action OnResetGame;
    private bool isGameOver = false;
    private bool isGameWin = false;


    void Awake()
    {
        PlayerHealth.onPlayerDied += GameOver;
    }

    void OnDestroy()
    {
        PlayerHealth.onPlayerDied -= GameOver;
    }

    void Start()
    {
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Pause the game

        MusicManager.PauseBackground();

        gameOverUI.SetActive(true);
        SoundEffectManager.Play("OverGame");
    }

    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0f;
        gameWinUI.SetActive(true);
        SoundEffectManager.Play("WinGame");
    }

    public void ResetGame()
    {
        isGameOver = false;
        Time.timeScale = 1f;

        MusicManager.ResumeBackground();

        SceneManager.LoadScene("Level 1");
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }
}

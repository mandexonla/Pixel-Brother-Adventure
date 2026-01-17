using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public Slider progessSlider;

    public GameObject player;
    public GameObject LoadCanavas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    private int survivedlevelsCount;

    public static event Action OnReset;

    // Start is called before the first frame update
    void Start()
    {
        progressAmount = 0;
        progessSlider.value = 0;
        Fruit.OnFruitCollect += IncreaseProgessAmount;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        PlayerHealth.onPlayerDied += GameOverScreen;
        LoadCanavas.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        MusicManager.PauseBackground();
        survivedText.text = "You survived " + survivedlevelsCount + " level";
        if (survivedlevelsCount != 1) survivedText.text += "s";
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);
        survivedlevelsCount = 0;
        LoadLevel(0, false);
        OnReset.Invoke();
        Time.timeScale = 1;
    }

    void IncreaseProgessAmount(int amount)
    {
        progressAmount += amount;
        progessSlider.value = progressAmount;

        if (progressAmount > 100)
        {
            LoadCanavas.SetActive(true);
        }
    }

    void LoadLevel(int level, bool wantSurvivvedIncrease)
    {
        LoadCanavas.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[level].gameObject.SetActive(true);

        player.transform.position = new Vector3(0, 0, 0);

        currentLevelIndex = level;
        progressAmount = 0;
        progessSlider.value = 0;
        if (wantSurvivvedIncrease) survivedlevelsCount++;
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, true);
    }

}

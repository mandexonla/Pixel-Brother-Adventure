using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;

    //public static event Action OnResetGame;
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

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        MusicManager.ResumeBackground();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene("SelectLevel");

            return;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }


    public bool IsGameOver() => isGameOver;
    public bool IsGameWin() => isGameWin;
}

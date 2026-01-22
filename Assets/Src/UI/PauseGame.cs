using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    private bool isPaused;

    void Start()
    {
        SetPause(false);
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;
        pauseUI.SetActive(pause);
        Time.timeScale = pause ? 0f : 1f;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        SetPause(!isPaused);
    }

    public void ButtonPause()
    {
        SetPause(!isPaused);
    }
}

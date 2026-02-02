using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OnStartClip()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void OnStartHome()
    {
        SceneManager.LoadScene("MenuGame");
    }

    public void OnExitClip()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectLevel : MonoBehaviour
{
    public void BackToMenuScene()
    {
        SceneManager.LoadScene("MenuGame");
    }

    public void OnStartClip()
    {
        SceneManager.LoadScene("SelectLevel");
    }
}

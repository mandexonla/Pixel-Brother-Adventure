using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{

    public void BackMenuScene()
    {
        SceneManager.LoadScene("MenuGame");
    }

    public void OnLevel1Button()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void OnLevel2Button()
    {
        SceneManager.LoadScene("Level 2");
    }
}

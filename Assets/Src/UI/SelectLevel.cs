using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{

    public void CharacterSelection()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void OnLevel0Button()
    {
        SceneManager.LoadScene("Level 0");
    }

    public void OnLevel1Button()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void OnLevel2Button()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void OnLevel3Button()
    {
        SceneManager.LoadScene("Level 3");
    }
}

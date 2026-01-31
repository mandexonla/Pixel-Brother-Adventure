using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectLevel : MonoBehaviour
{
    public void OnStartClip()
    {
        SceneManager.LoadScene("SelectLevel");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MenuPauseGame : MonoBehaviour
{
    [SerializeField] private GameObject panelMenuGame;
    [SerializeField] private Button buttonPauseGame;
    [SerializeField] private PauseGame pauseGame;

    private void Awake()
    {
        panelMenuGame.SetActive(false);

        buttonPauseGame.onClick.AddListener(() =>
        {
            panelMenuGame.SetActive(!panelMenuGame.activeSelf);
            pauseGame.ButtonPause();
        });
    }
}

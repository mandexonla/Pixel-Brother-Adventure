using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Button settingButton;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        settingPanel.SetActive(false);
        settingButton.onClick.AddListener(() =>
        {
            settingPanel.SetActive(true);
        });
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() =>
            {
                settingPanel.SetActive(false);

            });
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;
    private static AudioSource auidoSource;
    private static SoundEffectLibraly soundEffectLibraly;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            auidoSource = GetComponent<AudioSource>();
            soundEffectLibraly = GetComponent<SoundEffectLibraly>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundname)
    {
        AudioClip aucioClip = soundEffectLibraly.GetRandomClip(soundname);
        if (aucioClip != null)
        {
            auidoSource.PlayOneShot(aucioClip);
        }
    }

    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }
    public static void SetVolume(float volume)
    {
        auidoSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}



using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string isMusicOnPref = "is_music_on";
    private const string isSoundOnPref = "is_sound_on";

    public static SoundManager Instance { get; private set; }

    public bool IsMusicOn
    {
        get => isMusicOn;
        set
        {
            isMusicOn = value;
            PlayerPrefs.SetInt(isMusicOnPref, value ? 1 : 0);
            PlayerPrefs.Save();
            musicSource.mute = !value;
        }
    }
    public bool IsSoundOn
    {
        get => isSoundOn;
        set
        {
            isSoundOn = value;
            PlayerPrefs.SetInt(isSoundOnPref, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private SoundSource soundSourcePrefab;
    [SerializeField] private AudioClip[] musicClips;

    private bool isMusicOn;
    private bool isSoundOn;

    public void ToggleMusic()
    {
        IsMusicOn = !IsMusicOn;
        PlaySound(Sound.Click);
    }

    public void ToggleSound()
    {
        IsSoundOn = !IsSoundOn;
        PlaySound(Sound.Click);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayMusic()
    {
        var index = Random.Range(0, musicClips.Length);
        musicSource.clip = musicClips[index];
        musicSource.Play();
    }

    public void PlaySound(Sound sound)
    {
        if (!IsSoundOn) return;
        Instantiate(soundSourcePrefab).Init(sound);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        IsMusicOn = PlayerPrefs.GetInt(isMusicOnPref, 1) == 1;
        IsSoundOn = PlayerPrefs.GetInt(isSoundOnPref, 1) == 1;

        PlayMusic();
    }
}
using System;
using System.Collections;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip gameOverClip;
    private AudioSource source;

    public void Init(Sound sound)
    {
        var clip = GetSoundClip(sound);
        name = $"SOUND {sound}";
        source.clip = clip;
        source.Play();
        StartCoroutine(DestroyOnEnd());
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private AudioClip GetSoundClip(Sound sound)
    {
        switch (sound)
        {
            case Sound.Click: return clickClip;
            case Sound.GameOver: return gameOverClip;
            default: throw new NotImplementedException();
        }
    }

    private IEnumerator DestroyOnEnd()
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}

public enum Sound
{
    Click,
    GameOver,
}
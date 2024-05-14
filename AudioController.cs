using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource ASShuffle;
    public AudioSource ASMatchMade;
    public AudioSource ASGameMusic;
    public AudioSource ASWrongMatch;
    public AudioSource ASEmber;

    public AudioClip ingameAudio;

    public static AudioController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayGameMusic(ingameAudio);
    }

    public void PlayShuffleSE(AudioClip clip)
    {
        ASShuffle.PlayOneShot(clip);
    }

    public void PlayMatchMadeSE(AudioClip clip)
    {
        ASMatchMade.PlayOneShot(clip);
    }

    public void PlayWrongMatchSE(AudioClip clip)
    {
        ASWrongMatch.PlayOneShot(clip);
    }
    public void PlayEmberSE(AudioClip clip)
    {
        ASEmber.PlayOneShot(clip);
    }
    public void PlayGameMusic(AudioClip clip)
    {
        ASGameMusic.PlayOneShot(clip);
    }

    public void RestartGameMusic()
    {
        if(!ASGameMusic.isPlaying)
        {
            ASGameMusic.Play();
        }
    }
}

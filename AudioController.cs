using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource ASShuffle;
    public AudioSource ASMatchMade;
    public AudioSource ASGameMusic;
    public AudioSource ASWrongMatch;

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

    public void PlayGameMusic(AudioClip clip)
    {
        ASGameMusic.PlayOneShot(clip);
    }
}

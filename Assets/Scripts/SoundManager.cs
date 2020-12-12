using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    public AudioClip eatingPills;
    public AudioClip eatingVirus;
    public AudioClip virusMove;
    public AudioClip pacmanDies;
    public AudioClip syringeUse;

    private AudioSource pacmanAudioSource;
    private AudioSource virusAudioSource;
    private AudioSource oneShotAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();

        pacmanAudioSource = audioSources[0];
        virusAudioSource = audioSources[1];
        oneShotAudioSource = audioSources[2];

        playInLoop(pacmanAudioSource, eatingPills);

    }

    public void playOnce(AudioClip clip)
    {
        oneShotAudioSource.PlayOneShot(clip);
    }

    public void playInLoop(AudioSource aS,
        AudioClip clip)
    {
        if(aS != null && clip != null)
        {
            aS.loop = true;
            aS.volume = 0.2f;
            aS.clip = clip;
            aS.Play();
        }
    }

    public void pausePacman()
    {
        if(pacmanAudioSource != null && pacmanAudioSource.isPlaying)
        {
            pacmanAudioSource.Stop();
        }
    }

    public void unPausePacman()
    {
        if (pacmanAudioSource != null && !pacmanAudioSource.isPlaying)
        {
            pacmanAudioSource.Play();
        }
    }

    // ---- VIRUS STUFF ---
    public void pauseVirus()
    {
        if (virusAudioSource != null && virusAudioSource.isPlaying)
        {
            virusAudioSource.Stop();
        }
    }

    public void unPauseVirus()
    {
        if (virusAudioSource != null && !virusAudioSource.isPlaying)
        {
            virusAudioSource.Play();
        }
    }
}

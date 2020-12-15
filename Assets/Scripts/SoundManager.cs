using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // jest to wzorzec projektowy Singleton, w całym programie jest tylko jedna instancja tej klasy
    public static SoundManager Instance = null;

    // pliki audio
    public AudioClip eatingPills;
    public AudioClip eatingVirus;
    public AudioClip virusMove;
    public AudioClip pacmanDies;
    public AudioClip syringeUse;

    // trzy "głośniki" do odtwarzania dźwięków
    private AudioSource pacmanAudioSource; // głośnik dla dźwięków Pacmana
    private AudioSource virusAudioSource; // głośnik dla dźwięków wirusów
    private AudioSource oneShotAudioSource; // głośnik dla pojedyńczych dźwięków

    // funkcja uzywana do inicjalizacji
    void Start()
    {
        // zastosowanie wzorca projektowego Singleton
        if(Instance == null) Instance = this;
        else if (Instance != this)  Destroy(gameObject); 

        // pobranie "głośników"
        AudioSource[] audioSources = GetComponents<AudioSource>();

        // zapisanie "głośników" w zmiennych
        pacmanAudioSource = audioSources[0];
        virusAudioSource = audioSources[1];
        oneShotAudioSource = audioSources[2];

        // te dwa "głośniki" grają ten sam dźwięk w nieskończoność
        pacmanAudioSource.loop = true;
        virusAudioSource.loop = true;
    }

    // metoda do uruchamiania pojedyńczych dźwięków
    public void playOnce(AudioClip clip)
    {
        oneShotAudioSource.PlayOneShot(clip);
    }

    // metoda do urchamiania dźwięku w nieskończoność, na wybranym "głośniku"
    public void playInLoop(AudioSource aS, AudioClip clip)
    {
        // sprawdzamy czy mamy "głośnik" i plik dźwiękowy
        if(aS != null && clip != null)
        {
            aS.loop = true; // tryb pętli
            aS.volume = 0.2f; // ustawienie głośności
            aS.clip = clip; // ustawienie pliku dźwiękowego
            aS.Play(); // uruchmienie dźwięku
        }
    }

    // metoda zatrzymująca dźwięki Pacmana
    public void pausePacman()
    {
        if(pacmanAudioSource != null && pacmanAudioSource.isPlaying)
        {
            pacmanAudioSource.Stop();
        }
    }

    // metoda uruchamiająca dźwięki Pacmana
    public void unPausePacman()
    {
        if (pacmanAudioSource != null && !pacmanAudioSource.isPlaying)
        {
            pacmanAudioSource.Play();
        }
    }

    // metoda zatrzymująca dźwięki wirusów
    public void pauseVirus()
    {
        if (virusAudioSource != null && virusAudioSource.isPlaying)
        {
            virusAudioSource.Stop();
        }
    }

    // metoda uruchamiająca dźwięki wirusów
    public void unPauseVirus()
    {
        if (virusAudioSource != null && !virusAudioSource.isPlaying)
        {
            virusAudioSource.Play();
        }
    }
}

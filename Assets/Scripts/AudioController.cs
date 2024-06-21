using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance; // Para acessar o AudioController de qualquer lugar

    public AudioSource audioSourceMusicaDeFundo;
    public AudioSource audioSourceSFX;
    public AudioClip[] musicasDeFundo;

    public Slider volumeSliderMusica; // Slider para controlar o volume da música de fundo
    public Slider volumeSliderSFX; // Slider para controlar o volume dos efeitos sonoros

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garante que o AudioController persista entre as cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int IndexDaMusicaDeFundo = Random.Range(0, musicasDeFundo.Length);
        AudioClip musicasDeFundoDessaFase = musicasDeFundo[IndexDaMusicaDeFundo];
        audioSourceMusicaDeFundo.clip = musicasDeFundoDessaFase;
        audioSourceMusicaDeFundo.loop = true;
        audioSourceMusicaDeFundo.Play();
    }

    public void ToqueSFX(AudioClip clip)
    {
        audioSourceSFX.PlayOneShot(clip);
    }

    // Método para atualizar o volume da música de fundo
    public void UpdateMusicaVolume(float volume)
    {
        audioSourceMusicaDeFundo.volume = volume;
    }

    // Método para atualizar o volume dos efeitos sonoros
    public void UpdateSFXVolume(float volume)
    {
        audioSourceSFX.volume = volume;
    }
}
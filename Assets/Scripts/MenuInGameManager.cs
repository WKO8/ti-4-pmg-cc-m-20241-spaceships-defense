using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MenuInGameManager : MonoBehaviour
{
    [SerializeField] private string nomeDaCenaDoJogo;
    [SerializeField] private string nomeDaCenaDoMenuInicial;
    [SerializeField] private GameObject painelMenuInGame;

    public Slider musicSliderMenuInGame;
    public Slider sfxSliderMenuInGame;

    private AudioController audioControllerInstance;

    private void Start()
    {
        audioControllerInstance = AudioController.Instance;
        painelMenuInGame.SetActive(false);

        musicSliderMenuInGame.value = audioControllerInstance.audioSourceMusicaDeFundo.volume;
        sfxSliderMenuInGame.value = audioControllerInstance.audioSourceSFX.volume;
    }


    // Método para atualizar o volume da música de fundo
    public void OnChangeMusicSlider()
    {
        audioControllerInstance.UpdateMusicaVolume(musicSliderMenuInGame.value);
    }

    // Método para atualizar o volume dos efeitos sonoros
    public void OnChangeSFXSlider()
    {
        audioControllerInstance.UpdateSFXVolume(sfxSliderMenuInGame.value);
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    public void ToggleMenu()
    {
        // Check if the menu is active
        if (painelMenuInGame.activeSelf)
        {
            // If the menu is active, pause the game
            Time.timeScale = 1f;
        }
        else
        {
            // If the menu is not active, unpause the game
            Time.timeScale = 0f;
        }

        painelMenuInGame.SetActive(!painelMenuInGame.activeSelf);
    }

    public void Sair()
    {
        SceneManager.LoadScene(nomeDaCenaDoMenuInicial);
    }
}

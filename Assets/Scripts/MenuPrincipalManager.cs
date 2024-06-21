using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    // [SerializeField] private string nomeDoLevelDoJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelAjuda;
    [SerializeField] private GameObject painelCreditos;
    [SerializeField] private GameObject painelDificuldades;

    public Slider musicSlider;
    public Slider sfxSlider;

    private AudioController audioController;

    private void Start()
    {
        audioController = AudioController.Instance;
        painelOpcoes.SetActive(false);

        musicSlider.value = audioController.audioSourceMusicaDeFundo.volume;
        sfxSlider.value = audioController.audioSourceSFX.volume;

    }

    public void Jogar(string nomedoLevelDoJogo)
    {
        GlobalVariables.difficulty = nomedoLevelDoJogo;
        SceneManager.LoadScene(nomedoLevelDoJogo);
    }

    public void AbrirDificuldades()
    {
        painelMenuInicial.SetActive(false);
        painelDificuldades.SetActive(true);
    }

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void AbrirAjuda()
    {
        painelMenuInicial.SetActive(false);
        painelAjuda.SetActive(true);
    }

    public void AbrirCreditos()
    {
        painelMenuInicial.SetActive(false);
        painelCreditos.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelMenuInicial.SetActive(true);
        painelOpcoes.SetActive(false);
    }

    public void FecharAjuda()
    {
        painelMenuInicial.SetActive(true);
        painelAjuda.SetActive(false);
    }

    public void FecharCreditos()
    {
        painelMenuInicial.SetActive(true);
        painelCreditos.SetActive(false);
    }

    public void SairJogo()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }
}

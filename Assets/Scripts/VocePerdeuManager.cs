using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VocePerdeuManager : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelDoJogo;
    [SerializeField] private string nomeDoMenuInicial;

    public void Jogar()
    {
        // Carrega o jogo novamente
        SceneManager.LoadScene(nomeDoLevelDoJogo);
    }

    public void AbrirMenuInicial()
    {
        // Carrega o menu inicial
        SceneManager.LoadScene(nomeDoMenuInicial);
    }
}

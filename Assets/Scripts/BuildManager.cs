using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
    Script para organizar todas as ações feitas durante a execução do jogo
*/

public class BuildManager : MonoBehaviour
{


    public static BuildManager instance;    // Um ponteiro para a própria classe
    private BlueprintTorre torreSelecionada;
    private TextMeshProUGUI moedasText;
    private bool modoExclusao = false;

    public GameObject torreComumPrefab;
    public GameObject lancadorMissilPrefab;
    public GameObject laserPrefab;

    void Start()
    {
        GlobalVariables.isFirtTime = true;

        // Encontra o objeto pela tag "coins"
        GameObject coinsObject = GameObject.FindWithTag("coins");

        // Verifica se o objeto foi encontrado
        if (coinsObject != null)
        {
            // Obtém o componente TextMeshProUGUI
            moedasText = coinsObject.GetComponent<TextMeshProUGUI>();
        }
    }

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogError ("ERRO: Existe mais de um BuildManager.");
        }

        instance = this;

    }

    public BlueprintTorre GetTorreSelecionada ()
    {
        return (torreSelecionada);
    }

    public bool podeConstruir 
    { 
        get {return torreSelecionada != null;}
    }


    public void setTorreSelecionada (BlueprintTorre torre)
    {
        torreSelecionada = torre;
        Debug.Log(torreSelecionada + "<---");
    }

    public void setMinaSelecionada(BlueprintTorre mina)
    {
        torreSelecionada = mina;
    }

    public void construirTorre(Node node)
    {
        if (GlobalVariables.isFirtTime)
        {
            // Ajusta o preço da torre selecionada com base na dificuldade
            switch (GlobalVariables.difficulty)
            {
                case "Lua Facil":
                    torreSelecionada.preco -= 20;
                    break;
                case "Lua Dificil":
                    torreSelecionada.preco += 10;
                    break;
                default:
                    // Mantém o preço original se a dificuldade for Médio
                    break;
            }

            GlobalVariables.isFirtTime = false;
        }


        // Verifica se o preço ajustado é menor que zero e redefine para zero se for
        if (torreSelecionada.preco < 0)
        {
            torreSelecionada.preco = 0;
        }

        // Verifica se o jogador tem moedas suficientes para comprar a torre
        if (GlobalVariables.coins >= torreSelecionada.preco)
        {
            GlobalVariables.coins -= torreSelecionada.preco;
            moedasText.text = "Moedas: " + GlobalVariables.coins.ToString();

            Debug.Log(torreSelecionada.prefab + " FOI CONSTRUIDA!");

            GameObject torre = (GameObject)Instantiate(torreSelecionada.prefab, node.getPosicao(), Quaternion.identity);
            node.torre = torre;
            torreSelecionada = null; // Limpa a torre selecionada após a construção
        }
        else
        {
            Debug.Log("Dinheiro Insuficiente");
        }
    }

    public void destruirTorre (Node node)
    {
        GlobalVariables.coins += 10;
        Destroy (node.torre);
    }

    public void mudarModoExclusao ()
    {
        modoExclusao = !modoExclusao;
    }

    public bool getModoExclusao ()
    {
        return (modoExclusao);
    }


}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EncontrarRota : MonoBehaviour {
    GerenciadorCaminho gerenciadorCaminho;
    Mapa mapa;

    void Awake() {
        gerenciadorCaminho = GetComponent<GerenciadorCaminho>();
        mapa = GetComponent<Mapa>();
    }

    public void IniciarProcura(Vector3 posInicial, Vector3 posFinal) {
        StartCoroutine(EncontrarCaminho(posInicial, posFinal));
    }
    // Algoritmo A* encontra caminho mais curto entre dois pontos
    IEnumerator EncontrarCaminho(Vector3 inicio, Vector3 fim) {
        Vector3[] caminhos = new Vector3[0];
        bool caminhoTracado = false;
        mapa.AtualizarCustosTorres();
        // Converte vetores em nós
        NodeGrafos noInicio = mapa.NoMapa(inicio);
        NodeGrafos noDestino = mapa.NoMapa(fim);
        noInicio.pai = noInicio;
        // Verifica se os nós são transitaveis
        if (noInicio.transitavel && noDestino.transitavel) {
            Heap<NodeGrafos> fronteiraAtiva = new Heap<NodeGrafos>(mapa.tamanhoMaximo);
            HashSet<NodeGrafos> fronteiraInativa = new HashSet<NodeGrafos>();
            fronteiraAtiva.Add(noInicio);
            // Enquanto houver nós na fronteira ativa, adiciona o nó atual no conjunto fechado
            while (fronteiraAtiva.Count > 0) {
                NodeGrafos noAtual = fronteiraAtiva.RemoveFirst();
                fronteiraInativa.Add(noAtual);
                // Encerra caso o caminho fora encontrado
                if (noAtual == noDestino) {
                    caminhoTracado = true;
                    break;
                }
                // Explora os vizinhos do nó atual
                foreach (NodeGrafos vizinho in mapa.GetVizinhos(noAtual)) {
                    if (!vizinho.transitavel || fronteiraInativa.Contains(vizinho)) continue;
                    int custoMovimentoVizinho = noAtual.g + GetDistancia(noAtual, vizinho) + vizinho.torreAtiva;
                    if (custoMovimentoVizinho < vizinho.g || !fronteiraAtiva.Contains(vizinho)) {
                        vizinho.g = custoMovimentoVizinho;
                        vizinho.h = GetDistancia(vizinho, noDestino);
                        vizinho.pai = noAtual;
                        // Adiciona ao conjunto ativo
                        if (!fronteiraAtiva.Contains(vizinho)) {
                            fronteiraAtiva.Add(vizinho);
                        } else { // Atualiza os custos para a vizinhança. Levado em consideração quando há torres.
                            fronteiraAtiva.AtualizaItem(vizinho);
                        }
                    }
                }
            }
        }
        yield return null;
        if (caminhoTracado) {
            caminhos = RefazerCaminho(noInicio, noDestino);
        }
        gerenciadorCaminho.ProcessamentoFinalizado(caminhos, caminhoTracado);
    }

    // Reconstroi o caminho do nó inicial para o destino
    Vector3[] RefazerCaminho(NodeGrafos inicio, NodeGrafos fim) {
        List<NodeGrafos> caminho = new List<NodeGrafos>();
        NodeGrafos noAtual = fim;
        // Percorre os nós do fim ao início
        while (noAtual != inicio) {
            caminho.Add(noAtual);
            noAtual = noAtual.pai;
        }
        Vector3[] caminhos = SimplificarCaminho(caminho); // Inverte a ordem, para que o caminho comece da nó inicial
        Array.Reverse(caminhos);
        return caminhos;
    }
    // Reconstroi o caminho, removendo pontos desnecessários da direçãoAntiga que estão na mesma linha reta. Melhorando a eficiência
    Vector3[] SimplificarCaminho(List<NodeGrafos> caminho) {
        List<Vector3> caminhos = new List<Vector3>();
        Vector2 direcaoAntiga = Vector2.zero;

        for (int i = 1; i < caminho.Count; i++) {
            Vector2 direcaoNova = new Vector2(caminho[i - 1].mapaX - caminho[i].mapaX, caminho[i - 1].mapaY - caminho[i].mapaY);
            if (direcaoNova != direcaoAntiga) caminhos.Add(caminho[i].posicaoMapa);
            direcaoAntiga = direcaoNova;
        }
        return caminhos.ToArray();
    }
    // Distância de Manhattan
    int GetDistancia(NodeGrafos inicio, NodeGrafos fim) {
        int distanciaX = Mathf.Abs(inicio.mapaX - fim.mapaX);
        int distanciaY = Mathf.Abs(inicio.mapaY - fim.mapaY);

        if (distanciaX > distanciaY) {
            return distanciaY * 14 + (distanciaX - distanciaY) * 10;
        }

        return distanciaX * 14 + (distanciaY - distanciaX) * 10;
    }
}
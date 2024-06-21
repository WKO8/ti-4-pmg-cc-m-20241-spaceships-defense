using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GerenciadorCaminho : MonoBehaviour
{
    // Fila para armazenar os caminhos solicitados
    private Queue<EncontrarCaminho> caminhoSolicitadoQueue = new Queue<EncontrarCaminho>();
    private EncontrarCaminho caminhoAtual;
    private static GerenciadorCaminho instancia;
    private EncontrarRota encontrar;
    private bool processando;

    void Awake()
    {
        instancia = this;
        encontrar = GetComponent<EncontrarRota>();
    }

    // Cria um novo caminho e o adiciona na fila, funcionamento parecido com uma fila de espera e chama o método de processamento
    public static void SolicitaCaminho(Vector3 inicio, Vector3 fim, Action<Vector3[], bool> retorno)
    {
        if (instancia == null)
        {
            Debug.LogError("GerenciadorCaminho não inicializado.");
            return;
        }

        EncontrarCaminho novoCaminho = new EncontrarCaminho(inicio, fim, retorno);
        lock (instancia.caminhoSolicitadoQueue)
        {
            instancia.caminhoSolicitadoQueue.Enqueue(novoCaminho);
        }
        instancia.ProximoProcessamento();
    }

    // Processa a fila de caminhos
    private void ProximoProcessamento()
    {
        if (!processando)
        {
            lock (caminhoSolicitadoQueue)
            {
                if (caminhoSolicitadoQueue.Count > 0)
                {
                    caminhoAtual = caminhoSolicitadoQueue.Dequeue(); // Define o caminho atual
                    processando = true; // Limita o processamento da fila 
                    encontrar.IniciarProcura(caminhoAtual.inicio, caminhoAtual.fim); // Inicia proxima procura
                }
            }
        }
    }

    // Método chamado quando o processamento é finalizado.
    public void ProcessamentoFinalizado(Vector3[] caminho, bool encontrado)
    {
        caminhoAtual.retorno(caminho, encontrado); // Retorna o caminho atual
        processando = false; // Libera a fila para outro processamento
        ProximoProcessamento();
    }

    private struct EncontrarCaminho
    {
        public Vector3 inicio;
        public Vector3 fim;
        public Action<Vector3[], bool> retorno;

        public EncontrarCaminho(Vector3 inicio, Vector3 fim, Action<Vector3[], bool> retorno)
        {
            this.inicio = inicio;
            this.fim = fim;
            this.retorno = retorno;
        }
    }
}

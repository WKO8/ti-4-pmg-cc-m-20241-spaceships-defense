using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Atributos")]
    public float speed = 10f; // Velocidade do objeto
    public int dano = 10; // Dano causado quando o inimigo chega ao ponto final
    public double vida = 2f; // Quantidade de tiros comuns para derrotar o inimigo
    private Transform target; // O próximo ponto de referência para onde o objeto se moverá
    private int wavepointIndex = 0; // Índice do ponto de referência atual
    private bool debilitado = false;
    private Coroutine danoContinuoCoroutine;
    Vector3[] caminho;
    public WaveSpawner waveSpawner;

    void Start()
    {
        WaveSpawner waveSpawner = GameObject.FindObjectOfType<WaveSpawner>();

        if (waveSpawner != null)
        {
            this.waveSpawner = waveSpawner;
            target = waveSpawner.target;
        }
        else
        {
            Debug.LogError("WaveSpawner não encontrado.");
        }
    }

    void Update()
    {
        if (vida <= 0f)
        {
            if (danoContinuoCoroutine != null)
            {
                StopCoroutine(danoContinuoCoroutine);
            }

            Destroy(gameObject);

            GlobalVariables.enemiesAlive--;
            GlobalVariables.coins += 10;
            vida = 2f;
        }
    }

    public void CaminhoEncontrado(Vector3[] novoCaminho, bool caminhoEncontrado)
    {
        if (caminhoEncontrado)
        {
            caminho = novoCaminho;
            wavepointIndex = 0;
            StopCoroutine(nameof(SeguirCaminho));
            StartCoroutine(nameof(SeguirCaminho));
        }
    }

    IEnumerator SeguirCaminho()
    {
        Vector3 posicaoAtual = caminho[0];
        Vector3 direcao;
        while (true)
        {
            direcao = posicaoAtual - transform.position;
            if (transform.position == posicaoAtual)
            {
                wavepointIndex++;
                if (wavepointIndex >= caminho.Length)
                {
                    Destroy(gameObject);
                    waveSpawner.DecreaseHearts();
                    yield break;
                }
                posicaoAtual = caminho[wavepointIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, posicaoAtual, speed * Time.deltaTime);
            if (direcao != Vector3.zero)
            {
                transform.forward = direcao;
            }
            yield return null;
        }
    }

    public void SolicitarNovoCaminho(Vector3 targetPosition, Vector3 spawnPosition)
    {
        GerenciadorCaminho.SolicitaCaminho(spawnPosition, targetPosition, CaminhoEncontrado);
    }

    public void toggleDebilitado()
    {
        if (debilitado == false)
        {
            debilitado = true;
            speed = speed - 2;
        }
    }

    public void IniciarDanoContinuo(float danoPorSegundo)
    {
        if (danoContinuoCoroutine == null)
        {
            danoContinuoCoroutine = StartCoroutine(DanoContinuoCoroutine(danoPorSegundo));
        }
    }

    public void PararDanoContinuo()
    {
        if (danoContinuoCoroutine != null)
        {
            StopCoroutine(danoContinuoCoroutine);
            danoContinuoCoroutine = null;
        }
    }

    IEnumerator DanoContinuoCoroutine(float danoPorSegundo)
    {
        while (true)
        {
            vida -= danoPorSegundo * Time.deltaTime;
            yield return null;
        }
    }
}

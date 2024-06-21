using UnityEngine;
using System.Collections;
using TMPro;
public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab; // Prefab do inimigo que será instanciado
    private float countdown = 2f; // Tempo até a próxima onda de inimigos
    private int waveAmount = 1; // Número da onda atual
    public float timeBetweenWaves = 5f; // Tempo entre cada onda de inimigos
    public Transform[] spawnpoints; // Pontos onde os inimigos serão instanciados
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI heartsText;
    public string enemyTag = "Inimigo 1";
    private TextMeshProUGUI moedasText;
    [SerializeField] private GameObject vocePerdeu;

    private AudioController audioController;
    public AudioClip audioClipVocePerdeu;
    public Transform target;

    private void Start()
    {
        // Encontra o objeto pela tag "coins"
        GameObject coinsObject = GameObject.FindWithTag("coins");

        // Verifica se o objeto foi encontrado
        if (coinsObject != null)
        {
            // Obtém o componente TextMeshProUGUI
            moedasText = coinsObject.GetComponent<TextMeshProUGUI>();
        }

        // Descongela o jogo
        Time.timeScale = 1f;


        // Seta as variáveis globais
        GlobalVariables.coins = 60;
        GlobalVariables.hearts = 1;
        GlobalVariables.enemiesAlive = 0;

        moedasText.text = "Moedas: " + GlobalVariables.coins.ToString();
        heartsText.text = "Vidas: " + GlobalVariables.hearts.ToString();

        // Inicializa a referência ao AudioController
        audioController = AudioController.Instance;

        audioController.audioSourceMusicaDeFundo.Play();

    }

    void Update()
    {
        // Verifica se todos os inimigos foram eliminados
        if (GlobalVariables.enemiesAlive == 0 && countdown <= 0f)
        {
            StartCoroutine(SpawnWave()); // Inicia a coroutine para gerar uma onda de inimigos
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Debug.Log("Wave " + waveAmount + " Spawned"); // Imprime no console a wave atual
        waveText.text = "Rodada: " + waveAmount.ToString();

        for (int i = 0; i < waveAmount + 2; i++)
        {
            foreach (var spawnpoint in spawnpoints)
            {
                SpawnEnemy(spawnpoint); // Gera um inimigo em cada ponto de spawn
            }
            yield return new WaitForSeconds(0.3f); // Aguarda um curto período antes de gerar o próximo inimigo
        }

        waveAmount++;
        timeBetweenWaves++;
    }

    void SpawnEnemy(Transform spawnpoint)
    {
        // Incrementa o contador de inimigos vivos
        GlobalVariables.enemiesAlive++;

        // Instancia um inimigo no ponto de spawn especificado
        GameObject newEnemy = Instantiate(enemyPrefab, spawnpoint.position, spawnpoint.rotation).gameObject;
        Movement enemyMovement = newEnemy.GetComponent<Movement>();
        if (enemyMovement != null)
        {
            enemyMovement.SolicitarNovoCaminho(target.position, spawnpoint.position);
        }
    }

    public void DecreaseHearts()
    {
        GlobalVariables.hearts--;
        if (GlobalVariables.hearts == 0)
        {
            // Mostra mensagem "Você Perdeu"
            vocePerdeu.SetActive(true);

            // Congela o jogo
            Time.timeScale = 0f;

            // Pausar a música
            audioController.audioSourceMusicaDeFundo.Pause();

            // Tocar música de derrota
            audioController.audioSourceSFX.clip = audioClipVocePerdeu;
            audioController.audioSourceSFX.loop = false;
            audioController.audioSourceSFX.Play();

        }
        heartsText.text = "Vida: " + GlobalVariables.hearts.ToString();
    }
}

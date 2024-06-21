using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Turret : MonoBehaviour
{
    private Transform target; // O alvo atual da torre

    [Header("Atributos")]
    public float range = 15f; // Alcance da torre
    public float fireRate = 1f; // Taxa de disparo da torre
    private float fireContdown = 0f; // Contador para o próximo disparo

    private TextMeshProUGUI moedasText;

    [Header("Campos de Setup")]
    public string enemyTag = "Inimigo 1"; // Tag dos inimigos
    public Transform partToRotate; // Parte da torre que irá rotacionar para mirar nos inimigos
    public GameObject tiroPrefab; // Prefab do projétil que será disparado pela torre
    public Transform firePoint; // Ponto de origem do disparo
    public Transform destino;

    [Header("SFX")]
    // Efeito sonoro de disparo
    public AudioClip sfxDisparo; // Efeito sonoro
    private AudioController audioController; // AudioController

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);

        // Inicializa a referência ao AudioController
        audioController = AudioController.Instance;

        // Encontra o objeto pela tag "coins"
        GameObject coinsObject = GameObject.FindWithTag("coins");

        // Verifica se o objeto foi encontrado
        if (coinsObject != null)
        {
            // Obtém o componente TextMeshProUGUI
            moedasText = coinsObject.GetComponent<TextMeshProUGUI>();
        }
    }
    void Update()
    {
        if (target == null)
        {
            return;
        }

        // Calcula a direção para o alvo
        Vector3 dir = target.position - transform.position;
        // Calcula a rotação necessária para mirar no alvo
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        // Suaviza a rotação da torre 
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 10).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        // Verifica se a torre pode atirar
        if (fireContdown <= 0f)
        {
            Shoot();
            fireContdown = 1f / fireRate;
        }

        fireContdown -= Time.deltaTime;
    }

    // Função para atualizar o alvo da torre
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        double shortestDistanceToDestination = Mathf.Infinity;
        double lowestHealthPriority = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            double distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            double distanceToDestination = Vector3.Distance(destino.position, enemy.transform.position);

            // Verifica se o inimigo está dentro do alcance da torre
            if (distanceToEnemy <= range)
            {
                // Prioriza o inimigo mais próximo do destino
                if (distanceToDestination < shortestDistanceToDestination)
                {
                    shortestDistanceToDestination = distanceToDestination;
                    nearestEnemy = enemy;
                }

                // Em caso de empate na proximidade, considera o inimigo com menor vida
                if (nearestEnemy != null && distanceToDestination == shortestDistanceToDestination)
                {
                    double currentPriority = 1f / enemy.GetComponent<Movement>().vida;
                    if (currentPriority < lowestHealthPriority)
                    {
                        lowestHealthPriority = currentPriority;
                        nearestEnemy = enemy;
                    }
                }
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Função para disparar contra o alvo
    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(tiroPrefab, firePoint.position, firePoint.rotation);

        Tiro bullet = bulletGO.GetComponent<Tiro>();

        // Usa o AudioController para tocar o efeito sonoro de disparo
        audioController.ToqueSFX(sfxDisparo);

        moedasText.text = "Moedas: " + GlobalVariables.coins.ToString();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    // Função para representar o alcance da torre com uma esfera
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

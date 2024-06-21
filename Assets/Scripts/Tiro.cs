using UnityEngine;

/*
    Script para que o tiro caminhe e atinja o inimigo e então faça o inimigo desaparecer e adicionar dinheiro 
*/

public class Tiro : MonoBehaviour
{

    private Transform target; // O alvo que o projétil está seguindo
    
    private int raioExplosao = 5;
    
    [Header("Atributos")]
    public float speed = 70f; // Velocidade do projétil
    public float dano = 1; // Dano causado ao atingir alvo

    // Método para definir o alvo do projétil
    public void Seek(Transform _target)
    {
        target = _target;
    }


    void Update()
    {

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calcula a direção para o alvo
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Verifica se o projétil atingiu o alvo nesta frame
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Move o projétil na direção do alvo
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    // Método para lidar com o impacto no alvo
    void HitTarget()
    {
        Movement inimigo = target.GetComponent<Movement>();

        inimigo.vida -= dano;

        Destroy(gameObject); // Destroi o projétil
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioExplosao);
    }

}

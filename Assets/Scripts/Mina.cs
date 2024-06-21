using UnityEngine;

public class Mina : MonoBehaviour
{

    void OnCollisionEnter(Collision objColidido){ 
        if(objColidido.gameObject.CompareTag("Inimigo 1")){
            Destroy(gameObject); // Destroi a mina
            Destroy(objColidido.gameObject); // Destroi o alvo
            GlobalVariables.enemiesAlive--;
        }
    }
    
}

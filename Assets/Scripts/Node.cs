using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    Script para configurar cada quadrado que é possível colocar uma torre
*/

public class Node : MonoBehaviour
{

    BuildManager buildManager;

    public Color hoverColor;
    public Vector3 positionOffset;  // Variável para estipular a localização da torre no "Node"
    
    [Header ("Opcional")]
    public GameObject torre;
    public GameObject custoTorre;
    private Renderer rend; // Uma instância do próprio "Node"
    private Color startColor;

    void Start ()
    {
        rend = GetComponent<Renderer> ();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    void OnMouseEnter ()
    {

        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(!buildManager.podeConstruir)
        {
            return;
        }
        
        rend.material.color = hoverColor;

    }

    void OnMouseExit ()
    {
        rend.material.color = startColor;
    }

    void OnMouseDown ()
    {

        if (buildManager.getModoExclusao())
        {
            if (torre != null)
            {
                buildManager.destruirTorre(this);
                buildManager.mudarModoExclusao();  // Desativa o modo de exclusão após a torre ser destruída
                custoTorre.SetActive(false); // Desativa a camada de custo adicional das torres           
            }
            return;
        }

        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(!buildManager.podeConstruir)
        {
            return;
        }

        if(torre != null)
        {
            Debug.Log("Já existe uma construção neste lugar");
            return;
        }

        if(custoTorre != null)
        {
            custoTorre.SetActive(true); // Ativa o a camada de custo adicional das torres
        }

        buildManager.construirTorre(this);  
    }

    public Vector3 getPosicao ()
    {
        return this.transform.position + this.positionOffset;
    }

}


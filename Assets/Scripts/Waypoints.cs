using UnityEngine;
using UnityEngine.UIElements;

/*
    Script para armazenar os pontos que os inimigos devem passar até chegar ao ponto final
*/

public class Waypoints : MonoBehaviour
{

    public static Transform [] waypoints;

    void Awake()
    {

        waypoints = new Transform [transform.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }

    }

}

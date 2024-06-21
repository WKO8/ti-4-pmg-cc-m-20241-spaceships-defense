using UnityEngine;

/*
    Script para relacionar as opções do que foi selecionado na interface com o que vai ser colocado
*/

public class Shop : MonoBehaviour
{

    public BlueprintTorre torreComum;
    public BlueprintTorre lancadorDeMissil;
    public BlueprintTorre minaTerrestre;
    public BlueprintTorre laser;
    BuildManager buildManager;

    void Start ()
    {
        buildManager = BuildManager.instance;
    }

    public void SelecionarTorreComum ()
    {
        Debug.Log("Torre Comum");
        buildManager.setTorreSelecionada(torreComum);
    }

    public void SelecionarLancadorDeMissil ()
    {
        Debug.Log("Lancador de Missil");
        buildManager.setTorreSelecionada(lancadorDeMissil);
    }

    public void SelecionarLaser ()
    {
        Debug.Log("Laser");
        buildManager.setTorreSelecionada(laser);
    }

    public void SelecionarMinaTerrestre()
    {
        Debug.Log("Mina Terrestre");
        buildManager.setMinaSelecionada(minaTerrestre);
    }

    public void ExcluirObjeto()
    {
        Debug.Log("Excluir Objeto");
        buildManager.mudarModoExclusao();
    }


}

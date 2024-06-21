using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mapa : MonoBehaviour{
    public bool mostrarMapa;
    public LayerMask intransitavelMask; // Camada dos campos intransitaveis
    public Vector2 tamanhoMapa;
    public float noRaio;
    public LayerMask torreMask; // Adicione uma máscara para a camada da torre.
    public TipoTerreno[] terrenoTransitavel;
    Dictionary<int,int> regioesTransitaveis = new Dictionary<int, int>();
    NodeGrafos[,] mapa;

    float noDiametro;
    int mapaTamanhoX;
    int mapaTamanhoY;


    void Awake(){
        // Calcula o diâmetro do nó e o tamanho do mapa com a quantidade de nós
        noDiametro = 2*noRaio;
        mapaTamanhoX = Mathf.RoundToInt(tamanhoMapa.x/noDiametro);
        mapaTamanhoY = Mathf.RoundToInt(tamanhoMapa.y/noDiametro);

        foreach (TipoTerreno regiao in terrenoTransitavel) {
			torreMask.value |= regiao.terrenoMask.value;
			regioesTransitaveis.Add((int)Mathf.Log(regiao.terrenoMask.value,2),regiao.torrePeso);
		}

        CriarMapa();
    }

    // Retorna o tamanho do mapa
    public int tamanhoMaximo{
        get{
            return mapaTamanhoX * mapaTamanhoY;
        }
    }

    // Cria o mapa de caminhos
    void CriarMapa() {
        mapa = new NodeGrafos[mapaTamanhoX, mapaTamanhoY];
        Vector3 mapaLimiteInferior = transform.position - Vector3.right * tamanhoMapa.x / 2 - Vector3.forward * tamanhoMapa.y / 2;
        Vector3 deslocamentoX = Vector3.right * noDiametro;
        Vector3 deslocamentoY = Vector3.forward * noDiametro;

        // Precalcula posições para evitar recalcular dentro do loop
        Vector3[] posicoesX = new Vector3[mapaTamanhoX];
        Vector3[] posicoesY = new Vector3[mapaTamanhoY];
        
        for (int x = 0; x < mapaTamanhoX; x++) {
            posicoesX[x] = mapaLimiteInferior + deslocamentoX * x + Vector3.right * noRaio;
        }

        for (int y = 0; y < mapaTamanhoY; y++) {
            posicoesY[y] = deslocamentoY * y + Vector3.forward * noRaio;
        }

        // Percorre todas as posições do mapa verificando os pontos transitáveis ou não
        for (int x = 0; x < mapaTamanhoX; x++) {
            for (int y = 0; y < mapaTamanhoY; y++) {
                Vector3 posicaoMapa = posicoesX[x] + posicoesY[y];
                bool transitavel = !Physics.CheckSphere(posicaoMapa, noRaio, intransitavelMask);
                int acrescimoTorre = 0;

                // Verifica se há uma torre no raio de colisão
                if (transitavel) {
                    Ray ray = new Ray(posicaoMapa + Vector3.up * 50, Vector3.down);
                    if (Physics.Raycast(ray, out RaycastHit hit, 150, torreMask)) {
                        regioesTransitaveis.TryGetValue(hit.collider.gameObject.layer, out acrescimoTorre);
                    }
                }
                mapa[x, y] = new NodeGrafos(transitavel, acrescimoTorre, posicaoMapa, x, y);
            }
        }
    }


    // Vizinhos de um nó
    public List<NodeGrafos> GetVizinhos(NodeGrafos no) {
        List<NodeGrafos> vizinhos = new List<NodeGrafos>(8); // Pré-aloca espaço para até 8 vizinhos

        // Array de deslocamentos para os vizinhos
        int[,] deslocamentos = new int[,] {
            {-1, -1}, {-1, 0}, {-1, 1},
            {0, -1},          {0, 1},
            {1, -1}, {1, 0}, {1, 1}
        };

        for (int i = 0; i < deslocamentos.GetLength(0); i++) {
            int verificaX = no.mapaX + deslocamentos[i, 0];
            int verificaY = no.mapaY + deslocamentos[i, 1];

            // Se a posição do vizinho está dentro do mapa, adiciona o vizinho à lista.
            if (verificaX >= 0 && verificaX < mapaTamanhoX && verificaY >= 0 && verificaY < mapaTamanhoY) {
                vizinhos.Add(mapa[verificaX, verificaY]);
            }
        }

        return vizinhos;
    }


    // Método para obter o nó em uma posição do mapa.
    public NodeGrafos NoMapa(Vector3 mapaPosicao) {
        // Porcentagem da posição do mapa
        float porcentagemX = (mapaPosicao.x + tamanhoMapa.x / 2) / tamanhoMapa.x;
        float porcentagemY = (mapaPosicao.z + tamanhoMapa.y / 2) / tamanhoMapa.y;
        porcentagemX = porcentagemX < 0 ? 0 : (porcentagemX > 1 ? 1 : porcentagemX);
        porcentagemY = porcentagemY < 0 ? 0 : (porcentagemY > 1 ? 1 : porcentagemY);

        // Índice do nó no mapa
        int x = (int)(porcentagemX * (mapaTamanhoX - 1));
        int y = (int)(porcentagemY * (mapaTamanhoY - 1));

        return mapa[x, y];
    }


    public void AtualizarCustosTorres() {
        Vector3 mapaLimiteInferior = transform.position - Vector3.right * tamanhoMapa.x / 2 - Vector3.forward * tamanhoMapa.y / 2;

        for (int x = 0; x < mapaTamanhoX; x++) {
            for (int y = 0; y < mapaTamanhoY; y++) {
                Vector3 posicaoMapa = mapaLimiteInferior + Vector3.right * (x * noDiametro + noRaio) + Vector3.forward * (y * noDiametro + noRaio);
                NodeGrafos no = mapa[x, y];
                
                if (no.transitavel) {
                    Ray ray = new Ray(posicaoMapa + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    int acrescimoTorre = 0;

                    if (Physics.Raycast(ray, out hit, 150, torreMask)) {
                        regioesTransitaveis.TryGetValue(hit.collider.gameObject.layer, out acrescimoTorre);
                    }
                    
                    no.torreAtiva = acrescimoTorre;
                } else {
                    no.torreAtiva = 0;
                }
            }
        }
    }
    [System.Serializable]
	public class TipoTerreno {
		public LayerMask terrenoMask;
		public int torrePeso;
	}
}
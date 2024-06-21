using UnityEngine;
using System.Collections;

public class NodeGrafos: IHeapItem<NodeGrafos> {
    public bool transitavel;
    public int torreAtiva;
    public Vector3 posicaoMapa;
    public int mapaX;
    public int mapaY;

    public int g;
    public int h;
    public NodeGrafos pai;
    int heapIndex;

    public NodeGrafos(bool transitavel, int torreAtiva, Vector3 posicaoMapa, int mapaX, int mapaY){
        this.transitavel = transitavel;
        this.torreAtiva = torreAtiva;
        this.posicaoMapa = posicaoMapa;
        this.mapaX = mapaX;
        this.mapaY = mapaY;
    }

    public int F{
        get{
            return g + h;
        }
    }

    public int HeapIndex{
        get{
            return heapIndex;
        }
        set{
            heapIndex = value;
        }
    }

    public int CompareTo(NodeGrafos no){
        int comparar = F.CompareTo(no.F);
        if(comparar == 0){
            comparar = h.CompareTo(no.h);
        }
        return -comparar;
    }
}
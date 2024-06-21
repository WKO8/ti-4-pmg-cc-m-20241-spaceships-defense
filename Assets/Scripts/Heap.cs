using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> {
    // Array que armazena os itens no heap
    T[] itens;
    // Número atual de itens no heap
    int itemAtual;

    // Construtor que inicializa o array com o tamanho máximo especificado
    public Heap(int tamanhoMaximo) {
        itens = new T[tamanhoMaximo];
    }

    // Adiciona um item ao heap
    public void Add(T item) {
        // Define o índice do item no heap
        item.HeapIndex = itemAtual;
        // Armazena o item no array
        itens[itemAtual] = item;
        // Organiza o heap para cima
        Empilha(item);
        // Incrementa o contador de itens
        itemAtual++;
    }

    // Remove e retorna o primeiro item (raiz) do heap
    public T RemoveFirst() {
        T primeiroItem = itens[0];
        itemAtual--;
        itens[0] = itens[itemAtual];
        itens[0].HeapIndex = 0;
        // Organiza o heap para baixo
        Desempilha(itens[0]);
        return primeiroItem;
    }

    // Atualiza a posição de um item no heap
    public void AtualizaItem(T item) {
        Empilha(item);
    }

    // Propriedade que retorna o número de itens no heap
    public int Count {
        get { return itemAtual; }
    }

    // Verifica se o heap contém um item específico
    public bool Contains(T item) {
        return Equals(itens[item.HeapIndex], item);
    }

    // Organiza o heap para baixo (heapify down)
    void Desempilha(T item) {
        while (true) {
            int noEsq = item.HeapIndex * 2 + 1;
            int noDir = item.HeapIndex * 2 + 2;
            int troca = item.HeapIndex;

            // Verifica se o nó esquerdo deve ser trocado
            if (noEsq < itemAtual && itens[noEsq].CompareTo(itens[troca]) > 0)
                troca = noEsq;
            // Verifica se o nó direito deve ser trocado
            if (noDir < itemAtual && itens[noDir].CompareTo(itens[troca]) > 0)
                troca = noDir;

            // Se não houver troca, saia do laço
            if (troca != item.HeapIndex)
                Swap(item, itens[troca]);
            else
                break;
        }
    }

    // Organiza o heap para cima (heapify up)
    void Empilha(T item) {
        int pai = (item.HeapIndex - 1) / 2;

        while (item.HeapIndex > 0 && item.CompareTo(itens[pai]) > 0) {
            Swap(item, itens[pai]);
            pai = (item.HeapIndex - 1) / 2;
        }
    }

    // Troca a posição de dois itens no heap
    void Swap(T itemA, T itemB) {
        itens[itemA.HeapIndex] = itemB;
        itens[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

// Interface que deve ser implementada pelos itens que serão armazenados no heap
public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex { get; set; }
}

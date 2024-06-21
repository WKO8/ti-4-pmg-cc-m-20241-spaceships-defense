using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ColorButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI Texto;

    void Start()
    {
        Texto = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Texto != null)
        {
            Texto.color = new Color(1, 0.9490f, 0);
        }
        else
        {
            Debug.LogError("Texto não foi atribuído.");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Texto != null)
        {
            Texto.color = Color.white;
        }
        else
        {
            Debug.LogError("Texto não foi atribuído.");
        }
    }
}
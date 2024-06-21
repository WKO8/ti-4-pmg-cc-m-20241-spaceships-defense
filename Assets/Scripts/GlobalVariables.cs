using UnityEngine;
using TMPro;

public class GlobalVariables
{
    public static int coins = 60;
    public static int hearts = 100;
    public static int enemiesAlive = 0;
    public static bool isFirtTime = false;
    public static string difficulty = "Lua Facil";
    public static GameObject heartsGameObject; // Adiciona uma referência ao GameObject
    public static TextMeshProUGUI heartsText; // Referência ao componente TextMeshProUGUI
}
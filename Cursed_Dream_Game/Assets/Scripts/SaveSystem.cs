using UnityEngine;

/*
Aqui é para alterar os Stats iniciais do Player
*/

public class SaveSystem : MonoBehaviour
{
    public static float MaxHealth{
        get => PlayerPrefs.GetFloat("MaxHealth", 100f);
        set => PlayerPrefs.SetFloat("MaxHealth", value);
    }
    public static float Health{
        get => PlayerPrefs.GetFloat("Health", 100f);
        set => PlayerPrefs.SetFloat("Health", value);
    }

    public static float Damage{
        get => PlayerPrefs.GetFloat("Damage", 15f);
        set => PlayerPrefs.SetFloat("Damage", value);
    }

    public static int HealPotions{
        get => PlayerPrefs.GetInt("HealPotions", 3);
        set => PlayerPrefs.SetInt("HealPotions", value);
    }

    public static float Luck{
        get => PlayerPrefs.GetFloat("Luck", 0f);
        set => PlayerPrefs.SetFloat("Luck", value);
    }

    public static float Corruption{
        get => PlayerPrefs.GetFloat("Corruption", 0f);
        set => PlayerPrefs.SetFloat("Corruption", value);
    }

    public static float Defense{
        get => PlayerPrefs.GetFloat("Defense", 12f);
        set => PlayerPrefs.SetFloat("Defense", value);
    }

    public static int RoomLevel{
        get => PlayerPrefs.GetInt("RoomLevel", 0);
        set => PlayerPrefs.SetInt("RoomLevel", value);
    }
}
